using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character_Util;

public class Enemy_Controller : MonoBehaviour {
    /// ===
    /// FIELDS
    /// ===
    Transform transf = null; // cache transform
    [SerializeField] float vision_radius = 6f;
    [SerializeField] float combat_manoeuvre_radius = 4f;
    [SerializeField] float combat_radius = 3f;
    [SerializeField] float stunt_timer_init = 1f;
    [SerializeField] float maneuver_timer_init = 1; // used in ENEMY_C_has_maneuvered_long_enough
    [SerializeField] float health = 4;
    [SerializeField] float hit_animation_timer_init = 0.2f;
    [SerializeField] Movement movement = new Movement();
    [SerializeField] Dash dash = new Dash();
    [SerializeField] Material material_hit = null; // used to demo enemy getting hit. Flash enemy to this material when is_hit == true
    [SerializeField] new Renderer renderer = null;
    Combat combat = new Combat();
    [SerializeField] Vector3 attack_hitbox_offset = new Vector3();
    [SerializeField] Vector3 attack_hitbox_extents = new Vector3();

    Transform target_transform = null;
    float stun_timer;
    float maneuver_timer = 0; // used in ENEMY_C_has_maneuvered_long_enough
    float local_delta_time_scaler = 1;     // * this is used to slow down the enemy when they're hit.
    bool is_stunned = false;
    bool is_hit = false;
    bool is_spawned = false;
    [HideInInspector] public bool is_alive = true;
    bool is_knocked_back = false;
    bool trigger_hit_player = false;
    bool trigger_finished_attack_combo = false;
    Material material_normal = null;
    float hit_animation_timer = 0.2f;
    [HideInInspector] public Proto_Combat_Arena_Controller arena = null;
    /// init
    void Start() {
        // -- assert
        Debug.Assert(renderer != null);
        Debug.Assert(material_hit != null);
        // -- init fields
        transf = transform; // cache transform
        stun_timer = stunt_timer_init;
        target_transform = GameObject.FindGameObjectWithTag("Player").transform;
        maneuver_timer = maneuver_timer_init;
        material_normal = renderer.material;

        // -- attack delegates
        combat.attack_functions_start.Add(delegate_attack_1_start);
        combat.attack_functions_start.Add(delegate_attack_2_start);
        combat.attack_functions_start.Add(delegate_attack_3_start);
        combat.attack_functions_update.Add(delegate_attack_1_update);
        combat.attack_functions_update.Add(delegate_attack_2_update);
        combat.attack_functions_update.Add(delegate_attack_3_update);
    }
    /// called every physics frame
    void FixedUpdate() {
        // -- Behaviour
        if (ENEMY_C_is_alive()) {
            if (ENEMY_C_has_spawned()) {
                if (!ENEMY_C_is_knocked_back()) {
                    if (!ENEMY_C_is_stunned()) {
                        if (ENEMY_C_is_player_in_vision_range()) {
                            if (ENEMY_C_is_player_in_combat_manoeuvre_range()) {
                                if (ENEMY_C_has_maneuvered_long_enough()) {
                                    if (ENEMY_C_is_player_in_combat_range()) {
                                        if (ENEMY_C_is_ready_to_land_attack()) {
                                            if (ENEMY_C_is_swinging()) {
                                                if (!ENEMY_C_ATTACK_is_player_hit()) {
                                                    if (!ENEMY_C_ATTACK_end_of_attack_combo()) {
                                                        ENEMY_A_update_current_attack_swing();
                                                    } else {
                                                        trigger_finished_attack_combo = false;
                                                    }
                                                } else {
                                                    trigger_hit_player = false;
                                                    // TODO jump back
                                                }
                                            } else {
                                                ENEMY_A_queue_another_attack();
                                            }
                                        } else {
                                            ENEMY_A_get_ready_to_land_attack();
                                        }
                                    } else {
                                        ENEMY_A_approach_player();
                                    }
                                } else {
                                    ENEMY_A_maneuver_player();
                                }
                            } else {
                                ENEMY_A_approach_player();
                            }
                        } else {
                            // -- STAND GAURD
                        }
                    } else {
                        ENEMY_A_play_stunned_animation();
                    }
                } else {
                    ENEMY_A_apply_knockback();
                }
            } else {
                ENEMY_A_spawn();
            }
        } else {
            ENEMY_A_destroy_gameobject();
        }
        // -- update hit animation
        if (is_hit) {
            play_hit_animation();
        }
        // -- update local delta time
        if (local_delta_time_scaler < 1) {
            local_delta_time_scaler += Time.deltaTime;
        } else {
            local_delta_time_scaler = 1;
        }
        // TODO: update animation's speed based on local_delta_time_scaler
    }
    /// draw gizmos in the Unity editor to visualize some parameters
    void OnDrawGizmos() {
        // -- combat radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, combat_radius);
        Gizmos.color = Color.blue;
        // -- vision radius
        Gizmos.DrawWireSphere(transform.position, vision_radius);
        // -- attack hitbox
        Gizmos.DrawWireCube(transform.position + (attack_hitbox_offset).rotate(transform.rotation.eulerAngles.y), attack_hitbox_extents);
    }
    /// simplified lookAt. pass in a Vec3 to look at
    void look_at(Vector3 pos) {
        Vector3 current_pos = transf.position;
        current_pos.y = 0;
        Vector3 target_pos = pos;
        target_pos.y = 0;
        transf.LookAt(target_pos, Vector3.up);
    }
    /// hit this enemy
    public void hit(float damage) {
        is_hit = true;
        // -- start hit animation
        health -= damage;
        if (health <= 0) {
            is_alive = false;
            if (arena != null) {
                arena.update_status(); // refresh arena to register this enemy's death
            }
        }
        // -- slow down
        local_delta_time_scaler = 0.2f; // ! @incomplete MAGIC NUMBER
    }
    /// knock back and stun enemy
    public void knock_back(Vector2 direction) {
        dash.dash(transf.position, direction, Dash.TYPES.KNOCKBACK);
        is_knocked_back = true;
        //is_stunned = true;
    }
    /// stun the enemy and reset the timer
    public void stun() {
        stun_timer = stunt_timer_init;
        is_stunned = true;
    }
    void apply_velocity() {
        transf.position += movement.velocity * Time.deltaTime;
    }

    /// ==
    /// AI FUNCTIONS
    /// ==

    // !== CONDITIONS ==! //
    [AI_Function_Attribute]
    bool ENEMY_C_is_alive() {
        return is_alive;
    }
    /// returns true once the spawned animation is over. Take a look at ENEMY_spawn()
    [AI_Function_Attribute]
    bool ENEMY_C_has_spawned() {
        return is_spawned;
    }
    /// returns true if the enemy is stunt
    [AI_Function_Attribute]
    bool ENEMY_C_is_stunned() {
        return is_stunned;
    }
    /// returns true if the target_transform is within vision radius
    [AI_Function_Attribute]
    bool ENEMY_C_is_player_in_vision_range() {
        if (target_transform == null) return false;
        return (target_transform.position - transf.position).magnitude < vision_radius;
    }
    /// returns true when the player is within combat manoeuvre radius
    [AI_Function_Attribute]
    bool ENEMY_C_is_player_in_combat_manoeuvre_range() {
        return (target_transform.position - transf.position).magnitude <= combat_manoeuvre_radius;
    }
    /// updates the maneuver timer. Returns true if the timer has completed and resets the timer
    [AI_Function_Attribute]
    bool ENEMY_C_has_maneuvered_long_enough() {
        return true;
        // TODO figure out how to do maneuver for BETA
        if (maneuver_timer > 0) {
            maneuver_timer -= Time.deltaTime;
            return false;
        } else {
            maneuver_timer = maneuver_timer_init;
            return true;
        }
    }
    /// returns true if the target_transform is within combat radius
    [AI_Function_Attribute]
    bool ENEMY_C_is_player_in_combat_range() {
        if (target_transform == null) return false;
        return (transf.position - target_transform.position).magnitude <= combat_radius;
    }
    /// the suspense at the beginning of attacks
    [AI_Function_Attribute]
    bool ENEMY_C_is_ready_to_land_attack() {
        // TODO
        return true;
    }
    ///
    [AI_Function_Attribute]
    bool ENEMY_C_ATTACK_is_current_swing_over() {
        // TODO
        return true;
    }
    ///
    [AI_Function_Attribute]
    bool ENEMY_C_ATTACK_is_player_hit() {
        return trigger_hit_player;
    }
    ///
    [AI_Function_Attribute]
    bool ENEMY_C_ATTACK_end_of_attack_combo() {
        return trigger_finished_attack_combo;
    }
    /// have we been knocked back
    [AI_Function_Attribute]
    bool ENEMY_C_is_knocked_back() {
        return is_knocked_back;
    }

    // !== ACTIONS ==! //
    /// the enemy is being spwaned.
    /// returns false at all times
    [AI_Function_Attribute]
    bool ENEMY_A_spawn() {
        is_spawned = true; // TODO add animation for spawning. set spawned to true once the animation is over
        return false;
    }
    /// Destory this game object
    /// returns false
    [AI_Function_Attribute]
    bool ENEMY_A_destroy_gameobject() {
        Destroy(gameObject);
        return false;
    }
    /// Updates the stunned Play the stunt animation
    [AI_Function_Attribute]
    bool ENEMY_A_play_stunned_animation() {
        if (stun_timer > 0) {
            stun_timer -= Time.deltaTime;
        } else {
            stun_timer = stunt_timer_init;
            is_stunned = false;
        }
        return false;
    }
    /// the player is far. Run towards him to get closer.
    [AI_Function_Attribute]
    bool ENEMY_A_approach_player() {
        if (target_transform == null) return false;
        if (dash.is_in_progress) return false;
        // -- apply velocity
        movement.velocity = (target_transform.position - transf.position).normalized * movement.speed;
        movement.velocity.y = 0;
        apply_velocity();
        // -- face the target
        look_at(target_transform.position);
        
        return false;
    }
    /// The player is close enough, move around him a little before initiating attack.
    [AI_Function_Attribute]
    bool ENEMY_A_maneuver_player() {
        // TODO maneuver the player. Update the maneuver timer and set have_maneuvered long enough to true
        return false;
    }
    /// Get ready to land attack updates the animation for the suspense before landing an attack
    [AI_Function_Attribute]
    bool ENEMY_A_get_ready_to_land_attack() {
        // TODO update the animation for getting ready. Set is_ready_to_land_attack to true afterwards
        return false;
    }
    /// Update current attack swing animation. Update the state of current attack afterwards (was player hit?)
    [AI_Function_Attribute]
    bool ENEMY_A_update_current_attack_swing() {
        combat.update();
        return false;
    }
    /// Queue the next attack in the combo chain if not already queued
    [AI_Function_Attribute]
    bool ENEMY_A_queue_another_attack() {
        combat.queued_combo = true;
        return false;
    }
    /// Apply dash velocity whether it's jump or knock back
    [AI_Function_Attribute]
    bool ENEMY_A_apply_knockback() {
        if (dash.is_in_progress) {
            movement.velocity = dash.update(movement.velocity, transf);
            apply_velocity();
        } else {
            is_knocked_back = false;
        }
        return false;
    }
    /// Play the hit animation
    void play_hit_animation() {
        if (is_hit) {
            if (hit_animation_timer > 0) {
                hit_animation_timer -= Time.deltaTime;
                renderer.material = material_hit;
            } else {
                hit_animation_timer = hit_animation_timer_init;
                renderer.material = material_normal;
                is_hit = false;
            }
        }
    }

    // ! == ROOTS == ! //
    /// stand gaurd root
    [AI_Function_Attribute]
    bool ENEMY_R_stand_gaurd() {
        // does nothing for now
        // @incomplete
        return true;
    }
    /// return true if combat requires updating
    [AI_Function_Attribute]
    bool ENEMY_C_is_swinging() {
        return combat.is_attacking || combat.queued_combo;
    }
    
    // ! == ATTACK DELEGATES == ! //
    /// attack 1
    void delegate_attack_1_start() {
    }
    void delegate_attack_1_update() {
        attack_hit(1);
    }
    /// attack 2
    void delegate_attack_2_start() {
    }
    void delegate_attack_2_update() {
        attack_hit(2);
    }
    /// attack 3
    void delegate_attack_3_start() {
    }
    void delegate_attack_3_update() {
        attack_hit(3);
        trigger_finished_attack_combo = true; // @incomplete have this at the end of animation
    }
    /// attack hit
    void attack_hit(uint attack_combo_index) {
        if (!trigger_hit_player) {
            Collider[] colliders = Physics.OverlapBox(transf.position + (attack_hitbox_offset).rotate(transf.rotation.eulerAngles.y), attack_hitbox_extents, transf.rotation);
            foreach (Collider collider in colliders) {
                Player_Controller player = collider.gameObject.GetComponent<Player_Controller>();
                if (player != null) {
                    // -- apply damage
                    player.hit(1);
                    { // -- apply knockback
                        var knock_back_direction = player.transform.position - transf.position;
                        player.knock_back(new Vector2(knock_back_direction.x, knock_back_direction.z));
                    }
                    { // -- toggle has hit enemy so we don't hit more enemies or hit the same enemy multiple times
                        print("enemy has hit doggo at index: " + attack_combo_index.ToString());
                        trigger_hit_player = true;
                        break;
                    }
                }
            }
        }
    }
}
