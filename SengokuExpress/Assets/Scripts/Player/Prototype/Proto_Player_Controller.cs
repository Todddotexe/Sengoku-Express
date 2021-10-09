using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Proto_Player_Controller : MonoBehaviour {

    // ===
    // fields 
    // ===

    // -- MOVEMENT
    [SerializeField]
    float speed = 50f;                 // speed applied to movement velocity
    Vector3 velocity = new Vector3();   // movement velocity
    Transform transf = null;            // cached transform

    // -- COMPONENTS
    PlayerInput player_input = null;

    // -- INPUT
    InputAction input_walk = null;

      // ===
     // main procedures
    // ===
    private void Awake() {
        // -- input
        player_input = GetComponent<PlayerInput>();
        input_walk = player_input.actions["Move"];
        transf = transform;
    }

    private void FixedUpdate() {
        { // -- movement
            var input = input_walk.ReadValue<Vector2>();
            velocity.x = input.x * speed * Time.deltaTime;
            velocity.z = input.y * speed * Time.deltaTime;

            // change facing direction

            // @TODO

            // apply velocity
            transf.position += velocity;
        }
    }
}
