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
        public float normal_dash_range = 5f;                // default values subject to change through the editor
        public float combat_dash_range = 2f;    // default values subject to change through the editor
        public float speed = 30f;
        [HideInInspector] public bool is_in_progress = false;
        float progression = 0;                     // range from 0 - 1
        Vector3 start = new Vector3();             // used to lerp between the two dash points when dashing. Updated in the Input_dash_performed()
        Vector3 end = new Vector3();
        // TODO add dash fx and combat dash fx. turn them on in dash() depedning on whether this is a combat dash or a normal dash. turn them off in update after is_in_progress is changed to false;

        /// updates the dash variables for a dash move. Use update() to perform the dash
        public void dash(Vector3 _start, Vector2 input_vec2, bool is_combat_dash = false) {
            if (!is_in_progress) { // ? not sure why this check should be here. Do we want the player to be able to reset dash once this function is called or not?
                is_in_progress = true;
                // -- get and adjust input
                Vector3 input = new Vector3(input_vec2.x, 0, input_vec2.y);
                // -- change the range of dash depending on is_combat_dash
                var dash_range = is_combat_dash ? combat_dash_range : normal_dash_range;
                start = _start;
                end = start + input * dash_range;
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
        [HideInInspector] public bool is_attacking = false;
        [HideInInspector] public int current_combo_index = 0; // starts from 0 - 2 (inclusive)
        [HideInInspector] public bool queued_combo = false;
        [HideInInspector] public delegate void Attack_Function_Start();
        [HideInInspector] public delegate void Attack_Function_Update();
        [HideInInspector] public List<Attack_Function_Start> attack_functions_start = new List<Attack_Function_Start>();
        [HideInInspector] public List<Attack_Function_Update> attack_functions_update = new List<Attack_Function_Update>();
        // !Temp @temp
        [HideInInspector] public float temp_attack_duration; // set to init value in the constructor
        public float temp_attack_duration_init = 0.2f;

        public Combat() {
            temp_attack_duration = temp_attack_duration_init;
        }
    }
}