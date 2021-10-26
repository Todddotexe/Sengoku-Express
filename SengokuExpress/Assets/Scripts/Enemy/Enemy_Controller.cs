using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour {
    /// ===
    /// FIELDS
    /// ===
    public AI_Tree ai_tree;  // * the ai_tree asset used for behaviour
    Transform transf = null; // cache transform
    public float vision_radius = 6;
    public float combat_radius = 3;
    public float speed = 10f;
    public float stunt_duration_init = 1f;

    Transform target_transform = null;
    float stunt_duration;
    bool stunted = false;
    /// init
    void Start() {
        transf = transform; // cache transform
        stunt_duration = stunt_duration_init;
    }
    /// called every physics frame
    void FixedUpdate() {
        Debug.Assert(ai_tree != null);
        target_transform = GameObject.FindGameObjectWithTag("Player").transform;
        ai_tree.update(this);
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

    }

    /// ==
    /// AI FUNCTIONS
    /// ==

    // !== CONDITIONS ==! //
    /// returns true if the target_transform is within vision radius
    [AI_Function_Attribute]
    bool ENEMY_is_player_in_range() {
        if (target_transform == null) return false;
        return (target_transform.position - transf.position).magnitude < vision_radius;
    }
    /// returns true if the target_transform is within combat radius
    [AI_Function_Attribute]
    bool ENEMY_is_player_in_combat_range() {
        if (target_transform == null) return false;
        return (transf.position - target_transform.position).magnitude <= combat_radius;
    }

    // !== ACTIONS ==! //
    [AI_Function_Attribute]
    bool ENEMY_approach_player() {
        if (target_transform == null) return false;
        // -- apply velocity
        Vector3 velocity = (target_transform.position - transf.position).normalized * speed;
        velocity.y = 0;
        transf.position += velocity * Time.deltaTime;
        // -- face the target
        look_at(target_transform.position);
        return false;
    }

    [AI_Function_Attribute]
    bool ENEMY_attack_1() {
        if (target_transform == null) return false;
        look_at(target_transform.position);
        return true;
    }
}
