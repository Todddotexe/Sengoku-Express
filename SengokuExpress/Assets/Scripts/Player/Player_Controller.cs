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
	// <!> the following private classes are used for grouping variables <!>
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
        public float range = 5f;    // default values subject to change through the editor
        public float speed = 30f;
        bool dashing = false;
        float progression = 0;                     // range from 0 - 1
        Vector3 start = new Vector3();             // used to lerp between the two dash points when dashing. Updated in the Input_dash_performed()
        Vector3 end = new Vector3();

        // updates the dash variables for a dash move. Use update() to perform the dash
        public void dash(Vector3 _start, Vector2 input_vec2) {
            if (!dashing) {
                dashing = true;
                // -- get and adjust input
                Vector3 input = new Vector3(input_vec2.x, 0, input_vec2.y);

                start = _start;
                end = start + input * range;
                progression = 0;
            }
        }

        // returns the velocity with applied dash force
        public Vector3 update(Vector3 velocity, Transform transf) {    // @icomplete try to see if removing transf will work. Meaning instead of dash_start - transf.position we could do dash_start - dash_start or 0, and dash_end - 0
            if (dashing) {
                progression += speed * Time.deltaTime;
                 
                velocity = Vector3.Lerp(start - transf.position, end - transf.position, progression);
                if (progression >= 1) {
                    dashing = false;
                }
            }
            return velocity;
        }

        // <!> for the stupid reason that I want this variable to be public but Unity will make it appear in the inspector, I have to write a getter function.
        // return dashing
        public bool is_dashing() {
            return dashing;
        }
    }
    /// Player Bark
    [System.Serializable]
    private class Player_Bark {
        public float radius = 3f;  // default value subject to change through the editor
    }
    /// Player Inputs
    private class Player_Inputs{
        public Vector3 input = new Vector3();
        public Vector2 input_vec2 = new Vector2();
        public InputAction walk = null;
        public InputAction dash = null;
        public InputAction bark = null;
        public InputAction reset = null;
        public InputAction temp_exit = null;
    }
    /// Player Components
    private class Player_Components {
        public PlayerInput input = null;
        public CharacterController character_controller = null;
        public Animator animator = null;
    }

    // =================================================================
    // ======================= Player_Controller =======================
    // =================================================================
    public ParticleSystem dash_part;

    // ===
    /// FIELDS
    // ===
    Transform transf = null; // cached transform
	[SerializeField]
	Player_Movement movement;
    [SerializeField]
    Player_Dash dash;
    [SerializeField]
    Player_Bark bark;
    Player_Components components;
    Player_Inputs inputs;
    const string animator_bark = "Bark";

    /// initialise fields
    void Start() {
        // -- setup fields
        transf = transform;
        movement = new Player_Movement();
        dash = new Player_Dash();
        bark = new Player_Bark();
        components = new Player_Components();
        inputs = new Player_Inputs();
        // -- setup components
        components.input = GetComponent<PlayerInput>();
        components.character_controller = GetComponent<CharacterController>();
        components.animator = GetComponent<Animator>();
        // -- setup inputs
        inputs.walk  = components.input.actions["Move"];
        inputs.dash  = components.input.actions["Dash"];
        inputs.bark  = components.input.actions["Bark"];
        inputs.temp_exit  = components.input.actions["Temp_Exit"];
        inputs.reset = components.input.actions["Reset_Level_Debug"]; // @incomplete @debug remove this from here and the PlayerInp Inputs after debugging is over
        inputs.dash.performed += delegate_dash;
        inputs.bark.performed += delegate_bark;
        inputs.temp_exit.performed += delegate_temp_exit;
    }

    /// physics update
    void FixedUpdate() {
        // -- get input
        { // -- translate vec2 to vec3
            var input_vec2 = inputs.walk.ReadValue<Vector2>();
            inputs.input_vec2 = input_vec2;
            inputs.input.x = input_vec2.x;
            inputs.input.y = 0;
            inputs.input.z = input_vec2.y;
        }

        // -- dash, find the movement velocity that the player is applying
        if (dash.is_dashing()) { // use dash velocity
            movement.velocity = dash.update(movement.velocity, transf);
        } else { // use input
            movement.velocity = inputs.input * movement.speed * Time.deltaTime;
        }

        // -- change facing direction
        Vector3 face_dir = new Vector3();
        face_dir = inputs.input;
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

    /// draw debugging aids
    void OnDrawGizmosSelected() {
        // -- bark area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bark.radius);
        // -- dash range
        Vector3 dash_offset = new Vector3(0, 1, 0) + transform.position;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(dash_offset, dash_offset + transform.forward * dash.range);
    }

    /// used as a delegate for Player_Inputs.dash
    void delegate_dash(InputAction.CallbackContext obj) {
        dash.dash(transf.position, inputs.input_vec2);
        if (dash_part != null) {
            dash_part.Play();
        }
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

    /// temporarily used to exit the game during build
    void delegate_temp_exit(InputAction.CallbackContext obj) {
        Application.Quit();
    }
}
