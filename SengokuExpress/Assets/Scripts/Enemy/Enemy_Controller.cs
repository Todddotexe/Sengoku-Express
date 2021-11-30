using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character_Util;

public class Enemy_Controller : MonoBehaviour {
    /// ===
    /// FIELDS
    /// ===
    Transform transf = null; // cache transform
    [SerializeField] Enemy_Audio audio_source = new Enemy_Audio();
    [SerializeField] ParticleSystem part_hit = null; // ! should be set through the editor
    [SerializeField] List<ParticleSystem> part_spawn = new List<ParticleSystem>();
    [SerializeField] Animator animator = null;       // ! meant to be assigned in the editor
    [SerializeField] Enemy_Animations animations = new Enemy_Animations();
    [SerializeField] float vision_radius = 6f;
    [SerializeField] float combat_maneuver_radius = 5f;
    [SerializeField] float combat_radius = 3f;
    [SerializeField] float stunt_timer_init = 1f;
    [SerializeField] float maneuver_timer_init = 0.5f; // used in ENEMY_C_has_maneuvered_long_enough
    [SerializeField] float maneuver_speed_deg_per_second = 40f;
    [SerializeField] float health = 4;
    [SerializeField] Movement movement = new Movement();
    [SerializeField] Dash dash = new Dash();
    [SerializeField] new Renderer renderer = null;
    Combat combat = new Combat();
    [SerializeField] Vector3 attack_hitbox_offset = new Vector3();
    [SerializeField] Vector3 attack_hitbox_extents = new Vector3();
    [SerializeField] AudioSource main_audio_source = null; // ! to be set through the editor
    [SerializeField] AudioSource walk_audio_source = null; // ! to be set through the editor

    Transform target_transform = null;
    float stun_timer;
    float maneuver_timer; // * used in ENEMY_C_has_maneuvered_long_enough, SET in Start()
    float local_delta_time_scaler = 1;     // * this is used to slow down the enemy when they're hit.
    bool is_stunned = false;
    bool is_hit = false;
    bool is_spawned = false;
    bool has_maneuvered_long_enough = false;
    [HideInInspector] public bool is_alive = true;
    bool is_knocked_back = false;
    bool trigger_hit_player = false;
    bool trigger_finished_attack_combo = false;
    bool trigger_is_jumping = false;
    Material material_normal = null;
    float hit_animation_timer = 0.2f;
    int maneuver_direction = -1;
    Enemy_Blackboard blackboard = null; // used for enemies to communicate with one another
    int ai_state_depth = 0; // used to calculate how deep we are in the ai heirarchy. Used to set values based on the depth such as setting blackboard.current_active_enemy to null when we're not attacking
    [HideInInspector] public Proto_Combat_Arena_Controller arena = null;
    /// init
    void Start() {
        // -- assert
        Debug.Assert(renderer != null);
        blackboard = Global.blackboard;
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

        // -- audio sources
        walk_audio_source.clip = audio_source.walk;
        walk_audio_source.loop = true;
        play_walk_audio(false);
    }
    /// called every physics frame
    void FixedUpdate() {
        // -- update blackboard
        blackboard.update();
        // -- Behaviour
        if (ENEMY_C_is_alive()) {
            if (ENEMY_C_has_spawned()) {
                if (!ENEMY_C_is_knocked_back()) {
                    if (!is_hit) {
                        if (!ENEMY_C_is_jumping()) {
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
                                                                ai_state_depth = 14;
                                                            } else {
                                                                trigger_finished_attack_combo = false;
                                                                ai_state_depth = 13;
                                                            }
                                                        } else { // -- hit the player in attack_hit()
                                                            // note that we moved "jump" to attack_hit() when the player is hit, because when we had that logic here, we encountered a problem where ENEMY_C_is_player_in_combat_range() check returned false before we even got here during the next frame, so we didn't get here in time
                                                            // ! we don't reach this ever so wtf is this doing here? note that we rely on attack_hit() to get called
                                                            ai_state_depth = 12;
                                                        }
                                                    } else {
                                                        ENEMY_A_queue_another_attack();
                                                        ai_state_depth = 11;
                                                    }
                                                } else {
                                                    ENEMY_A_get_ready_to_land_attack(); // in case we have the time to work on such animation
                                                    ai_state_depth = 10;
                                                }
                                                animator.SetBool(animations.RUN, false); // reset boolean parameter
                                            } else {
                                                // animator.SetBool(animations.SIDESTEP, false); // reset boolean parameter
                                                ENEMY_A_approach_player();
                                                ai_state_depth = 9;
                                            }
                                        } else {
                                            animator.SetBool(animations.RUN, false); // reset boolean parameter
                                            ENEMY_A_maneuver_player();
                                            ai_state_depth = 8;
                                        }
                                    } else {
                                        // animator.SetBool(animations.SIDESTEP, false); // reset boolean parameter
                                        ENEMY_A_approach_player();
                                        ai_state_depth = 7;
                                    }
                                } else {
                                    // -- STAND GAURD
                                    animator.SetBool(animations.RUN, false); // reset boolean parameter
                                    ai_state_depth = 6;
                                }
                            } else {
                                ENEMY_A_play_stunned_animation();
                                ai_state_depth = 5;
                            }
                        } else {
                            ENEMY_A_apply_jump();
                            ai_state_depth = 4;
                        }
                    } else {
                        play_hit_animation();
                        ai_state_depth = 3;
                    }
                } else {
                    ENEMY_A_apply_knockback();
                    ai_state_depth = 2;
                }
            } else {
                ENEMY_A_spawn();
                ai_state_depth = 1;
            }
        } else {
            // TODO play death animation
            ENEMY_A_destroy_gameobject();
            ai_state_depth = 0;
        }
        // -- set current_active_enemy to null when we're not attacking the player (HACK)
        if (ai_state_depth < 9 && blackboard.current_active_enemy == this) 
            blackboard.current_active_enemy = null;
        // -- update local delta time
        if (local_delta_time_scaler < 1) {
            local_delta_time_scaler += Time.deltaTime;
        } else {
            local_delta_time_scaler = 1;
        }
        // -- disable looping audio
        if (ai_state_depth != 7 && ai_state_depth != 8 && ai_state_depth != 9) play_walk_audio(false); // don't walk when not approaching or sidesteping
        // TODO: update animation's speed based on local_delta_time_scaler
        // print("state: " + ai_state_depth);
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
        Vector3 target_pos = pos;
        target_pos.y = transf.position.y; // no longer look down
        transf.LookAt(target_pos, Vector3.up);
    }
    /// hit this enemy
    public void hit(float damage) {
        is_hit = true;
        // -- start hit animation
        animator.SetTrigger(animations.HIT);
        health -= damage;
        if (health <= 0) {
            is_alive = false;
            play_audio(audio_source.death);
            animator.SetTrigger(animations.DEATH);
            if (arena != null) {
                arena.update_status(); // refresh arena to register this enemy's death
            }
        } else {
            play_audio(audio_source.get_hit);
        }
        // -- slow down
        local_delta_time_scaler = 0.2f; // ! @incomplete MAGIC NUMBER
    }
    /// knock back and stun enemy
    public void knock_back(Vector2 direction, float additional_magnitude = 0f) { // ! we're not using this at the moment so wtf
        // animator.SetTrigger(animations.);
        dash.dash(transf.position, direction, Dash.TYPES.KNOCKBACK, additional_magnitude);
        is_knocked_back = true;
        //is_stunned = true;
    }
    /// stun the enemy and reset the timer
    public void stun() {
        animator.SetTrigger(animations.STUNNED);
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
    bool ENEMY_C_is_alive() {
        return is_alive;
    }
    /// returns true once the spawned animation is over. Take a look at ENEMY_spawn()
    bool ENEMY_C_has_spawned() {
        return is_spawned;
    }
    /// returns true if the enemy is stunt
    bool ENEMY_C_is_stunned() {
        return is_stunned;
    }
    /// returns true if the target_transform is within vision radius
    bool ENEMY_C_is_player_in_vision_range() {
        if (target_transform == null) return false;
        return (target_transform.position - transf.position).magnitude < vision_radius;
    }
    /// returns true when the player is within combat manoeuvre radius
    bool ENEMY_C_is_player_in_combat_manoeuvre_range() {
        return (target_transform.position - transf.position).magnitude <= combat_maneuver_radius;
    }
    /// updates the maneuver timer. Returns true if the timer has completed and resets the timer
    bool ENEMY_C_has_maneuvered_long_enough() {
        // return true;
        return has_maneuvered_long_enough;
    }
    /// returns true if the target_transform is within combat radius
    bool ENEMY_C_is_player_in_combat_range() {
        if (target_transform == null) return false;
        return (transf.position - target_transform.position).magnitude <= combat_radius;
    }
    /// the suspense at the beginning of attacks
    bool ENEMY_C_is_ready_to_land_attack() {
        // TODO
        return blackboard.current_active_enemy == this;
    }
    ///
    bool ENEMY_C_ATTACK_is_current_swing_over() {
        // TODO
        return true;
    }
    ///
    bool ENEMY_C_ATTACK_is_player_hit() {
        return trigger_hit_player;
    }
    ///
    bool ENEMY_C_ATTACK_end_of_attack_combo() {
        return trigger_finished_attack_combo;
    }
    /// have we been knocked back
    bool ENEMY_C_is_knocked_back() {
        return is_knocked_back;
    }

    // !== ACTIONS ==! //
    /// the enemy is being spwaned.
    /// returns false at all times
    void ENEMY_A_spawn() {
        is_spawned = true; // TODO add animation for spawning. set spawned to true once the animation is over
        // -- spawn particle
        if (part_spawn.Count > 0) {
            foreach (ParticleSystem particle in part_spawn) {
                particle.Play();
            }
        }
    }
    /// Destory this game object
    /// returns false
    void ENEMY_A_destroy_gameobject() {
        print("--------------------- died ");
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
                Destroy(gameObject);
                Debug.LogError("over");
            }
        } else {
            animator.Play("Death");
            Debug.LogError("play death");
        }
    }
    /// Updates the stunned Play the stunt animation
    void ENEMY_A_play_stunned_animation() { // TODO change from timer to the duration of the animation itself
        if (stun_timer > 0) {
            stun_timer -= Time.deltaTime;
        } else {
            stun_timer = stunt_timer_init;
            is_stunned = false;
        }
    }
    /// the player is far. Run towards him to get closer.
    void ENEMY_A_approach_player() {
        if (target_transform == null) return;
        if (dash.is_in_progress) return;
        // -- apply velocity
        movement.velocity = (target_transform.position - transf.position).normalized * movement.speed;
        movement.velocity.y = 0;
        apply_velocity();
        // -- face the target
        look_at(target_transform.position);
        // -- play animation
        animator.SetBool(animations.RUN, true);
        // -- audio
        play_walk_audio(true);
    }
    /// The player is close enough, move around him a little before initiating attack.
    void ENEMY_A_maneuver_player() {
        // TODO replace the combat radius (where the enemy approaches the player until arrives at combat radius) with maneuver radius
        // -- update timer
        if (maneuver_timer > 0) {
            maneuver_timer -= Time.deltaTime;
        } else {
            maneuver_timer = maneuver_timer_init + Random.Range(-.5f, .5f); // add some randomness
            maneuver_direction *= -1; // reverse the direction of maneuver for next time
            has_maneuvered_long_enough = true;
        }
        // -- maneuver player (move the enemy in a straight line adjacent to the player)
        transf.RotateAround(target_transform.position, Vector3.up, maneuver_direction * maneuver_speed_deg_per_second * Time.deltaTime);
        look_at(target_transform.position);
        // -- animation
        animator.SetTrigger(animations.SIDESTEP);
        // -- audio
        play_walk_audio(true);
    }
    /// Get ready to land attack updates the animation for the suspense before landing an attack
    void ENEMY_A_get_ready_to_land_attack() {
        // TODO update the animation for getting ready. Set is_ready_to_land_attack to true afterwards
        if (!blackboard.queued_attacks.Contains(this)) {
            blackboard.queued_attacks.Enqueue(this);
        }
        // animator.SetTrigger(animations.ANIM_SIDESTEP); // ! we may not have time to implement this
    }
    /// Update current attack swing animation. Update the state of current attack afterwards (was player hit?)
    void ENEMY_A_update_current_attack_swing() {
        combat.update();
    }
    /// Queue the next attack in the combo chain if not already queued
    void ENEMY_A_queue_another_attack() {
        combat.queued_combo = true;
        combat.update(); // update combat to allow it to know that we've queued another attack
    }
    /// Apply dash velocity when it's knock back
    void ENEMY_A_apply_knockback() {
        if (dash.is_in_progress) {
            movement.velocity = dash.update(movement.velocity, transf);
            apply_velocity();
        } else {
            is_knocked_back = false;
        }
    }
    /// Apply jump back velocity
    void ENEMY_A_apply_jump() {
        if (dash.is_in_progress) {
            movement.velocity = dash.update(movement.velocity, transf);
            apply_velocity();
        } else {
            trigger_is_jumping = false;
        }
    }
    ///
    bool ENEMY_C_is_jumping() {
        return trigger_is_jumping;
    }
    /// Play the hit animation
    void play_hit_animation() { // todo change this to the duration of the actual animation
        if (is_hit) {
            if (part_hit != null) part_hit.Play();
            is_hit = false;
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
        return combat.is_attacking;// || combat.queued_combo;
    }
    
    // ! == ATTACK DELEGATES == ! //
    /// attack 1
    void delegate_attack_1_start() {
        animator.SetTrigger(animations.ATTACK1);
    }
    void delegate_attack_1_update() {
        attack_hit(1);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animations.ANIMATION_ATTACK1)) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
                combat.toggle_attack_current_combo_finished = true;
            }
        } else {
                combat.toggle_attack_current_combo_finished = true;
        }
    }
    /// attack 2
    void delegate_attack_2_start() {
        animator.SetTrigger(animations.ATTACK2);
    }
    void delegate_attack_2_update() {
        attack_hit(2);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animations.ANIMATION_ATTACK2)) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
                combat.toggle_attack_current_combo_finished = true;
            }
        } else {
                combat.toggle_attack_current_combo_finished = true;
        }
    }
    /// attack 3
    void delegate_attack_3_start() {
        animator.SetTrigger(animations.ATTACK3);
    }
    void delegate_attack_3_update() {
        attack_hit(3);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animations.ANIMATION_ATTACK1)) {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1) {
                combat.toggle_attack_current_combo_finished = true;
                trigger_finished_attack_combo = true; // should be at the end of animation
            }
        } else {
                combat.toggle_attack_current_combo_finished = true;
                trigger_finished_attack_combo = true; // should be at the end of animation
        }
    }
    /// attack hit
    void attack_hit(uint attack_combo_index) {
        if (!animator.GetBool(EACH.check_for_hit)) return; // if animation is not at a point where we want it to be for collision checking, return
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
                        // print("enemy has hit doggo at index: " + attack_combo_index.ToString());
                        trigger_hit_player = true; // TODO 'ere
                        // -- jump
                        animator.SetTrigger(animations.JUMPBACK);
                        dash.dash(transf.position, transf.position - target_transform.position, Dash.TYPES.NORMAL);
                        trigger_is_jumping = true;
                        trigger_hit_player = false; // TODO wtf look above
                        has_maneuvered_long_enough = false; // reset maneuver so we maneuver next time
                        if (blackboard.current_active_enemy == this) blackboard.current_active_enemy = null;
                        break;
                    }
                }
            }
        }
    }

    /// play a non looping audio
    void play_audio(AudioClip clip) {
        main_audio_source.clip = clip;
        main_audio_source.Play();
    }
    void play_walk_audio(bool value) {
        walk_audio_source.enabled = value;
    }

    [System.Serializable]
    private class Enemy_Animations {
        public string ATTACK1 = "Attack1";
        public string ATTACK2 = "Attack2";
        public string ATTACK3 = "Attack3";
        public string ANIMATION_ATTACK1 = "Attack 1";
        public string ANIMATION_ATTACK2 = "Attack 2";
        public string ANIMATION_ATTACK3 = "Attack 3";
        public string RUN      = "Run";
        public string HIT      = "Hit";
        public string DEATH    = "Death";
        public string JUMPBACK = "JumpBack";
        public string STUNNED  = "Stunned";
        public string SIDESTEP = "SideStep";
        // public string IDLE     = "Idle";
        // public string ANIM_SIDESTEP = "SideStep"; // * we probabily don't have time to create an animation for this
    }

    [System.Serializable]
    private class Enemy_Audio {
        public AudioClip get_hit = null;
        public AudioClip walk    = null;
        public AudioClip death   = null;
    }
}
/// blackboard
public class Enemy_Blackboard {
    public Enemy_Controller current_active_enemy = null;
    public Queue<Enemy_Controller> queued_attacks = new Queue<Enemy_Controller>(); // used to queue attacks for when enemies are ready to land attack. This is used to restrict the number of enemies attacking the player to 1

    public void update() {
        if (queued_attacks.Count > 0) {
            if (current_active_enemy == null) {
                current_active_enemy = queued_attacks.Dequeue();
            }
        }
    }
}