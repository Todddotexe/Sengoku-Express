﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proto_Enemy : MonoBehaviour {
    public AI_Tree ai_tree;
    Transform transf = null;
    public Transform target_transform = null;
    public float vision_radius = 6;
    public float combat_radius = 3;
    public float speed = 10f;

    void Start() {
        transf = transform;
    }

    void FixedUpdate() {
        if (ai_tree != null) {
            ai_tree.update(this);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, combat_radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, vision_radius);
    }

    // ==
    // AI FUNCTIONS
    // ==

    [AI_Function_Attribute]
    bool is_player_in_range() {
        return (target_transform.position - transf.position).magnitude < vision_radius;
    }

    [AI_Function_Attribute]
    bool approach_player_until_in_combat_range() {
        if ((transf.position - target_transform.position).magnitude <= combat_radius) {
            return true;
        }
        Vector3 velocity = (target_transform.position - transf.position).normalized * speed;
        velocity.y = 0;
        transf.position += velocity * Time.deltaTime;
        return false;
    }

    [AI_Function_Attribute]
    bool attack_1() {
        return true;
    }
}
