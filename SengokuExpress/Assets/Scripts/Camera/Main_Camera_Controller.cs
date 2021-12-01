using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Camera_Controller : MonoBehaviour {
    // // ==
    // // FIELDS
    // // ==
    // [SerializeField]
    // Transform target;
    // [SerializeField]
    // float drag_stiffness = 3f;
    // [SerializeField]
    // float drag_ignore_radius = 1f;
    // [SerializeField]
    // Vector3 target_offset;
    // new Transform transform;
    // Quaternion rotation;

    // // ==
    // // MAIN FUNCTIONS
    // // ==
    // void Start() {
    //     transform = gameObject.transform;
    //     rotation = transform.rotation;
    //     
    // }
    // //
    // void FixedUpdate() {
    //     if (target == null) return;
    //     transform.position += ((target.position - transform.position) - target_offset) * drag_stiffness * Time.deltaTime;
    // }
    
    [SerializeField]
    Transform target;
    [SerializeField]
    Transform joe;
    [SerializeField]
    float drag_stiffness = 3f;
    [SerializeField]
    float drag_ignore_radius = 1f;
    [SerializeField]
    Vector3 target_offset = new Vector3(40, 40, 0);
    [SerializeField]
    Vector3 angle = new Vector3(11, -14, 12);
    // Quaternion rotation;
    Transform transf;
    bool joe_mode = false;

    // ==
    // MAIN FUNCTIONS
    // ==
    void Awake()
    {
        Global.main_camera = this;
    }
    void Start() {
        transf = gameObject.transform;
        transf.position = target.position;
        {
            var rot = transf.rotation;
            rot.eulerAngles = angle;
            transf.rotation = rot;
        }
        ////  if (target != null) {
        ////     target_offset = target.position - transform.position;
        //// }
    }
    //
    void FixedUpdate() {
        if (target == null) return;
        
        if (joe_mode)
        {
            joe.position += new Vector3(0, Time.deltaTime, 0);
        }

        transf.position += ((target.position - transf.position) - target_offset) * drag_stiffness * Time.deltaTime;

    }
    public void look_at_joe()
    {
        target = joe;
        transf.position = target.position;
        transf.LookAt(joe);
        joe_mode = true;
    }
}
