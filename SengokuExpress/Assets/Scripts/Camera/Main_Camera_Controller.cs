using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Camera_Controller : MonoBehaviour {
    // ==
    // FIELDS
    // ==
    [SerializeField]
    Transform target;
    [SerializeField]
    float drag_stiffness = 3f;
    [SerializeField]
    float drag_ignore_radius = 1f;
    [SerializeField]
    Vector3 target_offset;
    new Transform transform;
    Quaternion rotation;

    // ==
    // MAIN FUNCTIONS
    // ==
    void Start() {
        transform = gameObject.transform;
        rotation = transform.rotation;
        if (target != null) {
            target_offset = target.position - transform.position;
        }
    }
    //
    void FixedUpdate() {
        transform.position += ((target.position - transform.position) - target_offset) * drag_stiffness * Time.deltaTime;
    }
}
