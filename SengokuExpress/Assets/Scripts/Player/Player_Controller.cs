/// Author: Matin Kamali
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player_Controller : MonoBehaviour {
	
    // !=================================================================
    // !======================= Player Controller =======================
    // !=================================================================
    // public ParticleSystem dash_part; // TODO add this back when we have a proper particle system

    // ===
    /// FIELDS
    // ===
    Transform transf = null; // cached transform
	[SerializeField] Player_Movement movement = new Player_Movement();   // ! the reason we initialise these here is because I'm testing whether we can change the values in Unity Editor and keep it. If we initialise them in Start(), all the changes in UnityEditor will be lost. Especially if there's an asset field like the Dash_Particle alex tried to put in.
    
    [SerializeField] Player_Dash dash = new Player_Dash();
    
    [SerializeField] Player_Bark bark = new Player_Bark();
    [SerializeField] Player_Binding binds = new Player_Binding();
    Player_Combat combat = new Player_Combat();
    Player_Components components = new Player_Components();
    Player_Inputs inputs = new Player_Inputs();
    const string animator_bark = "Bark";

    /// initialise fields
    void Start() {
        // -- setup fields
        transf = transform;
        // -- setup components
        components.input = GetComponent<PlayerInput>();
        components.character_controller = GetComponent<CharacterController>();
        components.animator = GetComponent<Animator>();
        // -- setup inputs
        inputs.walk       = components.input.actions[binds.WALK_INPUT_LABEL];
        inputs.dash       = components.input.actions[binds.DASH_INPUT_LABEL];
        inputs.bark       = components.input.actions[binds.BARK_INPUT_LABEL];
        inputs.attack     = components.input.actions[binds.ATTACK_INPUT_LABEL];
        inputs.temp_exit  = components.input.actions["Temp_Exit"];
        inputs.reset      = components.input.actions["Reset_Level_Debug"]; // @incomplete @debug remove this from here and the PlayerInp Inputs after debugging is over
        inputs.dash.performed += delegate_dash;
        inputs.bark.performed += delegate_bark;
        inputs.attack.performed += delegate_attack;
        inputs.temp_exit.performed += delegate_temp_exit;
        // -- setup attack combo chain
        combat.attack_functions_start.Add(delegate_attack_1_start);
        combat.attack_functions_start.Add(delegate_attack_2_start);
        combat.attack_functions_start.Add(delegate_attack_3_start);
        combat.attack_functions_update.Add(delegate_attack_1_update);
        combat.attack_functions_update.Add(delegate_attack_2_update);
        combat.attack_functions_update.Add(delegate_attack_3_update);
    }
    /// physics update
    void FixedUpdate() {
        { // -- update input
            var input_vec2    = inputs.walk.ReadValue<Vector2>();
            inputs.input_vec2 = input_vec2;
            inputs.input.x    = input_vec2.x;
            inputs.input.y    = 0;
            inputs.input.z    = input_vec2.y;
        }
        if (combat.is_attacking || combat.queued_combo) {
            PLAYER_update_simple_combat();
        } else
        if (dash.is_in_progress && PLAYER_apply_dash()) {
        } else
        if (inputs.input_vec2.magnitude > 0) {
            movement.velocity = inputs.input * movement.speed * Time.deltaTime;
        }
        PLAYER_apply_velocity();
    }
    /// draw debugging aids
    void OnDrawGizmosSelected() {
        // -- bark area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bark.radius);
        // -- dash range
        Vector3 dash_offset = new Vector3(0, 1, 0) + transform.position;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(dash_offset, dash_offset + transform.forward * dash.normal_dash_range);
    }
    /// used as a delegate for Player_Inputs.dash
    void delegate_dash(InputAction.CallbackContext obj) {
        dash.dash(transf.position, inputs.input_vec2);
        // if (dash_part != null) { // TODO add this back when we have a proper particle system
        //     dash_part.Play();
        // }
    }
    /// used as a delegate for Player_Inputs.bark
    void delegate_bark(InputAction.CallbackContext obj) {
        components.animator.SetTrigger(animator_bark);
        // -- init collision
        Collider[] colliders = Physics.OverlapSphere(transf.position, bark.radius);
        foreach (Collider collider in colliders) {
            print("found a collision");
            // proto_enemy_bark enemy = collider.gameObject.GetComponent<proto_enemy_bark>();
            // if (enemy != null) {
            //     enemy.stunt = true;
            //     print("stunted enemy");
            // }
        }
    }
    /// used to queue attack
    void delegate_attack(InputAction.CallbackContext obj) {
        combat.queued_combo = true;
    }
    /// first attack in the combo chain (Start)
    void delegate_attack_1_start() {
        print("attack 1 start");
        delegate_dash(new InputAction.CallbackContext());
    }
    /// first attack in the combo chain (Update)
    void delegate_attack_1_update() {
        PLAYER_apply_dash();
    }
    /// second attack in the combo chain (Start)
    void delegate_attack_2_start() {
        print("attack 2 start");
        delegate_dash(new InputAction.CallbackContext());
    }
    /// second attack in the combo chain (Update)
    void delegate_attack_2_update() {
        PLAYER_apply_dash();
    }
    /// third attack in the combo chain (Start)
    void delegate_attack_3_start() {
    }
    /// third attack in the combo chain (Update)
    void delegate_attack_3_update() {
    }
    /// temporarily used to exit the game during build
    void delegate_temp_exit(InputAction.CallbackContext obj) {
        Application.Quit();
    }

    // !=================================================================
    // !=======================    BEHAVIOURS     =======================
    // !=================================================================
    // == AI TREE FUNCTIONS == //
    /// applies dash to velocity. Returns true if the dash it in progress. 
    /// Returns false if dash is completed or the player is no longer dashing (dash.is_dashing() == true)
    bool PLAYER_apply_dash() {
        if (dash.is_in_progress) {
            movement.velocity = dash.update(movement.velocity, transf);
            Vector3 face_dir = inputs.input; // TODO duplicate code spotted ! does this need to here?
            transf.forward = Vector3.Slerp(transf.forward, face_dir, movement.rotation_speed * Time.deltaTime);
            return true;
        }
        return false;
    }
    /// apply velocity to the player. Returns true at all times
    void PLAYER_apply_velocity() {
        // -- update face dir to point at input direction not velocity
        Vector3 face_dir = inputs.input;
        transf.forward = Vector3.Slerp(transf.forward, face_dir, movement.rotation_speed * Time.deltaTime);
        // -- friction
        if (movement.velocity.x != 0 || movement.velocity.y != 0) {
            movement.velocity.x = Mathf.Lerp(movement.velocity.x, 0, 
                                            movement.friction * Time.deltaTime);
            movement.velocity.z = Mathf.Lerp(movement.velocity.z, 0, 
                                            movement.friction * Time.deltaTime);
        }
        // -- gravity
        if (!components.character_controller.isGrounded) {
            movement.velocity.y -= movement.gravity * Time.deltaTime;
        }
        // -- apply friction
        components.character_controller.Move(movement.velocity);
    }
    /// update simple combat goes through the attack queues and performs them.
    void PLAYER_update_simple_combat() {
        // TODO -- check if attack animation is finished. If so, set is_attacking to false and set current_combo to 0.
        if (combat.is_attacking) {
            if (combat.temp_attack_duration > 0) {
                combat.temp_attack_duration -= Time.deltaTime;
                // ! this is where attacks update()
                combat.attack_functions_update[combat.current_combo_index]();
            } else {
                combat.temp_attack_duration = combat.temp_attack_duration_init;
                if (combat.queued_combo) { // if another attack is queued, increase current_combo and keep is_attacking true
                    // ! this is where attacks start()
                    combat.queued_combo = false;
                    if (combat.current_combo_index < 2) combat.current_combo_index++; // if overflowing the maximum number of combos, reset back to zero
                    else combat.current_combo_index = 0;
                    combat.attack_functions_start[combat.current_combo_index]();
                } else { // if no other attack is queued, no more attacking
                    combat.is_attacking = false;
                    combat.current_combo_index = 0;
                }
            }
        } else if (combat.queued_combo) {
            // ! this is where attacks start
            combat.is_attacking = true;
            combat.queued_combo = false;
            combat.current_combo_index = 0; // !if we were not attacking, or in the progress of attacking, the current_combo_index should be zero. This is here to make that clear.
            combat.attack_functions_start[combat.current_combo_index]();
        }
    }

    // !=================================================================
    // !=======================  PRIVATE CLASSES  =======================
    // !=================================================================
    // ! the following private classes are used for grouping variables
	/// Player Movement
	[System.Serializable]
	private class Player_Movement {
		public float speed 				= 10f; // default values subject to change through the editor
		public float rotation_speed 	= 10f;
		public float friction 			= 10f;
		public float gravity  			= 10f;
		public Vector3 velocity;
	}
    /// Player Dash
    [System.Serializable]
    private class Player_Dash {
        public float normal_dash_range = 5f;                // default values subject to change through the editor
        public float combat_dash_range = 2f;    // default values subject to change through the editor
        public float speed = 30f;
        [HideInInspector] public bool is_in_progress = false;
        float progression = 0;                     // range from 0 - 1
        Vector3 start = new Vector3();             // used to lerp between the two dash points when dashing. Updated in the Input_dash_performed()
        Vector3 end = new Vector3();
        // TODO add dash fx and combat dash fx. turn them on in dash() depedning on whether this is a combat dash or a normal dash. turn them off in update after is_in_progress is changed to false;

        // updates the dash variables for a dash move. Use update() to perform the dash
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

        // returns the velocity with applied dash force
        public Vector3 update(Vector3 velocity, Transform transf) {    // @icomplete try to see if removing transf will work. Meaning instead of dash_start - transf.position we could do dash_start - dash_start or 0, and dash_end - 0
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
    /// Player Combat
    [System.Serializable]
    private class Player_Combat {
        [HideInInspector] public bool is_attacking = false;
        [HideInInspector] public int current_combo_index = 0; // starts from 0 - 2 (inclusive)
        [HideInInspector] public bool queued_combo = false;
        [HideInInspector] public delegate void Attack_Function_Start();
        [HideInInspector] public delegate void Attack_Function_Update();
        [HideInInspector] public List<Attack_Function_Start> attack_functions_start = new List<Attack_Function_Start>();
        [HideInInspector] public List<Attack_Function_Update> attack_functions_update = new List<Attack_Function_Update>();
        // !Temp @temp
        [HideInInspector] public float temp_attack_duration = 1;
        public float temp_attack_duration_init = 0.2f;

    }
    /// Player Bark
    [System.Serializable]
    private class Player_Bark {
        public float radius = 3f;  // default value subject to change through the editor
    }
    /// Binding with Unity Editor Animation and Input system
    [System.Serializable]
    private class Player_Binding {
        public string WALK_INPUT_LABEL = "Move";
        public string DASH_INPUT_LABEL = "Dash";
        public string BARK_INPUT_LABEL = "Bark";
        public string ATTACK_INPUT_LABEL = "LightAttack";
    }
    /// Player Inputs
    private class Player_Inputs{
        public Vector3 input = new Vector3();
        public Vector2 input_vec2 = new Vector2();
        public InputAction walk = null;
        public InputAction dash = null;
        public InputAction bark = null;
        public InputAction reset = null;
        public InputAction attack = null;
        public InputAction temp_exit = null;
    }
    /// Player Components
    private class Player_Components {
        public PlayerInput input = null;
        public CharacterController character_controller = null;
        public Animator animator = null;
    }

}
