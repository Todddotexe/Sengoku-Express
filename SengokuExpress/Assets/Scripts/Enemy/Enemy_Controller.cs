using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character_Util;

public class Enemy_Controller : MonoBehaviour {
    /// ===
    /// FIELDS
    /// ===
    public AI_Tree ai_tree;  // * the ai_tree asset used for behaviour
    Transform transf = null; // cache transform
    public float vision_radius = 6f;
    public float combat_manoeuvre_radius = 4f;
    public float combat_radius = 3f;
    public float stunt_duration_init = 1f;
    public Movement movement = new Movement();
    public Dash dash = new Dash();

    Transform target_transform = null;
    float stunt_duration;
    float local_delta_time_scaler = 1;     // * this is used to slow down the enemy when they're hit.
    bool is_stunted = false;
    bool is_spawned = false;
    bool is_hit = false;
    bool is_alive = true;
    /// init
    void Start() {
        transf = transform; // cache transform
        stunt_duration = stunt_duration_init;
        target_transform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    /// called every physics frame
    void FixedUpdate() {
        // -- update local delta time
        if (local_delta_time_scaler < 1) {
            local_delta_time_scaler += Time.deltaTime;
        } else {
            local_delta_time_scaler = 1;
        }
        // -- make sure we have the ai tree asset
        Debug.Assert(ai_tree != null);
        // -- run ai tree asset
        ai_tree.update(this);
        // TODO: update animation's speed based on local_delta_time_scaler
    }
    /// draw gizmos in the Unity editor to visualize some parameters
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, combat_radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, vision_radius);
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
    public void hit() { // TODO add a damage parameter
        // -- start hit animation
        // TODO
        is_hit = true;
        // -- slow down
        local_delta_time_scaler = 0.2f; // ! @incomplete MAGIC NUMBER
    }
    /// knock back and stunt enemy
    public void knock_back(Vector2 direction) {
        dash.dash(transf.position, direction, Dash.TYPES.KNOCKBACK);
        is_stunted = true;
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
    bool ENEMY_is_stunt() {
        return is_stunted;
    }
    /// returns true if the enemy is hit
    [AI_Function_Attribute]
    bool ENEMY_C_is_hit() {
        return is_stunted;
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
        if ((target_transform.position - transf.position).magnitude <= combat_manoeuvre_radius) {
            return true;
        }
        return false;
    }
    [AI_Function_Attribute]
    bool ENEMY_C_has_maneuvered_long_enough() {
        // TODO add a randomised timer to enemy and reset it at the end
        return true;
    }
    /// returns true if the target_transform is within combat radius
    [AI_Function_Attribute]
    bool ENEMY_is_player_in_combat_range() {
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
        // TODO
        return false;
    }
    ///
    [AI_Function_Attribute]
    bool ENEMY_C_ATTACK_end_of_attack_combo() {
        // TODO
        return false;
    }

    // !== ACTIONS ==! //
    /// the enemy is being spwaned.
    /// returns false at all times
    [AI_Function_Attribute]
    bool ENEMY_spawn() {
        is_spawned = true; // @incomplete // TODO add animation for spawning. set spawned to true once the animation is over
        return false;
    }
    /// Destory this game object
    /// returns false
    [AI_Function_Attribute]
    bool ENEMY_A_destroy_gameobject() {
        Destroy(gameObject);
        return false;
    }
    /// Play the stunt animation
    [AI_Function_Attribute]
    bool ENEMY_A_play_stunt_animation() {
        if (is_stunted) {
            // TODO add a timer here for now and set is_stunt to false after that timer
            is_stunted = false;
        }
        return false;
    }
    /// the player is far. Run towards him to get closer.
    [AI_Function_Attribute]
    bool ENEMY_A_approach_player() {
        if (target_transform == null) return false;
        // -- apply velocity
        movement.velocity = (target_transform.position - transf.position).normalized * movement.speed;
        movement.velocity.y = 0;
        transf.position += movement.velocity * Time.deltaTime;
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
        return false;
    }
    /// Queue the next attack in the combo chain if not already queued
    [AI_Function_Attribute]
    bool ENEMY_A_queue_another_attack() {
        return false;
    }

    // ! == ROOTS == ! //
    /// stand gaurd root
    [AI_Function_Attribute]
    bool ENEMY_R_stand_gaurd() {
        // does nothing for now
        // @incomplete
        return true;
    }
    ///
    [AI_Function_Attribute]
    bool ENEMY_R_attack() { // TODO change to are_we_swinging in the Enemy Decision Tree png from draw.io. It would fit the purpose better
        // does nothing for now
        // @incomplete
        return true;
    }

}
