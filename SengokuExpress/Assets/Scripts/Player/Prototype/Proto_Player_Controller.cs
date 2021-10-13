using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Proto_Player_Controller : MonoBehaviour {

    // ==
    // CONSTANTS
    // ==
    const string tag_environment = "enemy";

    // ===
    // FIELDS
    // ===

    // -- movement
    [SerializeField]
    float speed;                                    // speed applied to movement velocity
    [SerializeField]
    float rotation_speed;
    float friction = 10f;
    float gravity = 10f;
    Vector3 velocity = new Vector3();               // movement velocity
    Transform transf = null;                        // cached transform
    
    // -- dash
    [SerializeField]
    float dash_range;
    [SerializeField]
    float dash_speed;
    bool dashing = false;
    float dash_progression = 0;                     // range from 0 - 1
    Vector3 dash_start = new Vector3();             // used to lerp between the two dash points when dashing. Updated in the Input_dash_performed()
    Vector3 dash_end = new Vector3();

    // -- bark
    [SerializeField]
    float bark_radius;

    // -- components
    PlayerInput player_input = null;
    CharacterController controller = null;
    Animator animator = null;

    // -- input
    InputAction input_walk = null;
    InputAction input_dash = null;
    InputAction input_bark = null;
    InputAction input_reset = null;

    // ===
    // main procedures
    // ===
    private void Start() {
        // -- input
        player_input = GetComponent<PlayerInput>();
        input_walk = player_input.actions["Move"];
        input_dash = player_input.actions["Dash"];
        input_bark = player_input.actions["Bark"];
        input_reset = player_input.actions["Reset"];

        // -- other components
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // -- dash
        input_dash.performed += dash;
        // -- bark
        input_bark.performed += bark;
        // -- reset
        input_reset.performed += reset;
        
        // -- cache transform
        transf = transform;
    }

    private void FixedUpdate() {
        { // -- movement
            // -- get and adjust input
            Vector3 input = new Vector3();

            { // -- translate vec2 to vec3
                var input_vec2 = input_walk.ReadValue<Vector2>();
                input.x = input_vec2.x;
                input.y = 0;
                input.z = input_vec2.y;
            }

            //input = rotate_vector3(input, -40); // @incomplete decide what alex really wants deep inside
            
            if (dashing) {
                dash_progression += dash_speed * Time.deltaTime;
                 
                velocity = Vector3.Lerp(dash_start - transf.position, dash_end - transf.position, dash_progression);
                if (dash_progression >= 1) {
                    dashing = false;
                }
            } else {
                // -- update velocity based on input
                velocity = input * speed * Time.deltaTime;
            }

            // -- change facing direction
            Vector3 face_dir = new Vector3();
            face_dir = input;
            transf.forward = Vector3.Slerp(transf.forward, face_dir, rotation_speed * Time.deltaTime);

            // -- friction
            if (velocity.x != 0 || velocity.y != 0) {
                velocity.x = Mathf.Lerp(velocity.x, 0, friction * Time.deltaTime);
                velocity.z = Mathf.Lerp(velocity.z, 0, friction * Time.deltaTime);
            }

            // -- gravity
            if (!controller.isGrounded) {
                velocity.y -= gravity * Time.deltaTime;
            }

            // -- apply friction
            controller.Move(velocity);
        }
    }

    // draw debugging aids
    void OnDrawGizmosSelected() {
        // -- bark area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bark_radius);
        // -- dash range
        Vector3 dash_offset = new Vector3(0, 1, 0) + transform.position;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(dash_offset, dash_offset + transform.forward * dash_range);
    }

    private void dash(InputAction.CallbackContext obj) {        // @incomete cleanup the duplicate code of getting input
        if (!dashing) {
            dashing = true;
            // -- get and adjust input
            Vector3 input = new Vector3();

            { // -- translate vec2 to vec3
                var input_vec2 = input_walk.ReadValue<Vector2>();
                input.x = input_vec2.x;
                input.y = 0;
                input.z = input_vec2.y;
            }

            dash_start = transf.position;
            dash_end = transf.position + input * dash_range;
            dash_progression = 0;
        }
    }

    void bark(InputAction.CallbackContext obj) {
        animator.SetTrigger("Bark");
        // -- init collision
        Collider[] colliders = Physics.OverlapSphere(transf.position, bark_radius);
        foreach (Collider collider in colliders) {
            print("found a collision");
            proto_enemy_bark enemy = collider.gameObject.GetComponent<proto_enemy_bark>();
            if (enemy != null) {
                enemy.stunt = true;
                print("stunted enemy");
            }
        }
    }

    void reset(InputAction.CallbackContext obj) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ===
    // SIGNALS
    // ===
}
