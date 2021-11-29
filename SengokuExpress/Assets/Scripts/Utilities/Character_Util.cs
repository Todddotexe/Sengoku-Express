using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character_Util {
    /// Movement
    [System.Serializable]
    public class Movement {
        public float speed 				= 5f; // default values subject to change through the editor
        public float rotation_speed 	= 10f;
        public float friction 			= 10f;
        public float gravity  			= 10f;
        public Vector3 velocity;
    }
    /// Dash
    [System.Serializable]
    public class Dash {
        public enum TYPES {
            NORMAL, COMBAT, KNOCKBACK,
        }
        TYPES current_dash_type = TYPES.NORMAL;
        public float normal_dash_range = 5f;    // default values subject to change through the editor
        public float combat_dash_range = 2f;    // default values subject to change through the editor
        public float knockback_dash_range = 3f; // used when this enemy gets knocked back
        public float speed = 30f;
        [HideInInspector] public bool is_in_progress = false;
        float progression = 0;                     // range from 0 - 1
        Vector3 start = new Vector3();             // used to lerp between the two dash points when dashing. Updated in the Input_dash_performed()
        Vector3 end = new Vector3();
        // TODO add dash fx and combat dash fx. turn them on in dash() depedning on whether this is a combat dash or a normal dash. turn them off in update after is_in_progress is changed to false;

        /// updates the dash variables for a dash move. Use update() to perform the dash
        public void dash(Vector3 _start, Vector2 direction, TYPES type) {
            if (!is_in_progress) { // ? not sure why this check should be here. Do we want the player to be able to reset dash once this function is called or not? I think this is here to stop the player from being able to fly forward lol
                direction = direction.normalized;
                is_in_progress = true;
                // -- get and adjust input
                Vector3 input = new Vector3(direction.x, 0, direction.y);
                // -- change the range of dash depending on type
                float range = normal_dash_range;
                switch (type) {
                    case TYPES.NORMAL: range = normal_dash_range; break;
                    case TYPES.COMBAT: range = combat_dash_range; break;
                    case TYPES.KNOCKBACK: range = knockback_dash_range; break;
                }
                start = _start;
                end = start + input * range;
                progression = 0;
            }
        }

        /// returns the velocity with applied dash force
        public Vector3 update(Vector3 velocity, Transform transf) {    // @incomplete try to see if removing transf will work. Meaning instead of dash_start - transf.position we could do dash_start - dash_start or 0, and dash_end - 0
            if (is_in_progress) {
                progression += speed * Time.deltaTime;
                    
                velocity = Vector3.Lerp(start - transf.position, end - transf.position, progression);
                if (progression >= 1) {
                    is_in_progress = false;
                }
            }
            return velocity;
        }
    }
    /// Combat
    [System.Serializable]
    public class Combat {
        public TrailRenderer attack_trail = null;
        [HideInInspector] public bool is_attacking = false;
        [HideInInspector] public int current_combo_index = 0; // starts from 0 - 2 (inclusive)
        [HideInInspector] public bool queued_combo = false;
        [HideInInspector] public delegate void Attack_Function_Start();
        [HideInInspector] public delegate void Attack_Function_Update();
        [HideInInspector] public List<Attack_Function_Start> attack_functions_start = new List<Attack_Function_Start>();
        [HideInInspector] public List<Attack_Function_Update> attack_functions_update = new List<Attack_Function_Update>();

        // set this to true to indicate current attack combo is finished. This allows for outside timers or triggers
        public bool toggle_attack_current_combo_finished = true; // ! meant to be set externally

        public void update() {
            if (attack_functions_start.Count == 0) return;
            // TODO -- check if attack animation is finished. If so, set is_attacking to false and set current_combo to 0.
            if (is_attacking) {
                if (!toggle_attack_current_combo_finished) {
                    attack_functions_update[current_combo_index]();
                } else {
                    if (queued_combo) { // -- if another attack is queued, increase current_combo and keep is_attacking true
                        // * this is where attacks start()
                        queued_combo = false;
                        if (current_combo_index < attack_functions_start.Count - 1) current_combo_index++; // if overflowing the maximum number of combos, reset back to zero
                        else current_combo_index = 0;
                        attack_functions_start[current_combo_index]();
                    } else { // -- if no other attack is queued, no more attacking
                        is_attacking = false;
                        current_combo_index = 0;
                    }
                }
            } else if (queued_combo) {
                // * this is where attacks start
                is_attacking = true;
                queued_combo = false;
                current_combo_index = 0; // * if we were not attacking, or in the progress of attacking, the current_combo_index should be zero. This is here to make that clear.
                attack_functions_start[current_combo_index]();
            }
            // -- attack trail
            if (attack_trail != null) {
                attack_trail.enabled = is_attacking;
            }
        }
    }
    ///
    [System.Serializable]
    public class Dog_Audio {
        public AudioClip bark     = null;
        public AudioClip walk     = null;
        public AudioClip attack_1 = null;
        public AudioClip attack_2 = null;
        public AudioClip attack_3 = null;
        public AudioClip dash     = null;
        public AudioClip hurt     = null;
        public AudioClip death    = null;
    }
    ///
    public static class Vec3_Extension {
        public static Vector3 rotate(this Vector3 v, float degrees) {
            float sin = -Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
            
            float tx = v.x;
            float tz = v.z;
            v.x = (cos * tx) - (sin * tz);
            v.z = (sin * tx) + (cos * tz);
            return v;
        }
    }
}