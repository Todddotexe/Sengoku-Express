using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class proto_enemy_bark : MonoBehaviour {
    
    public bool stunt = false;
    public Transform target = null;
    MeshRenderer mesh_renderer = null;
    [SerializeField]
    float speed;
    float stunt_timer_init = 1; // in seconds
    float stunt_timer; // in seconds
    [SerializeField]
    Material stunt_material = null;
    Material normal_material = null;

    void Awake() {
        mesh_renderer = GetComponent<MeshRenderer>();
        normal_material = mesh_renderer.material;
        stunt_timer = stunt_timer_init;
    }

    void FixedUpdate() {
        if (target == null) return;

        if (!stunt) {

            Vector3 velocity = (target.position - transform.position).normalized * speed;
            velocity.y = 0;
            transform.position += velocity * Time.deltaTime;

        } else {
            mesh_renderer.material = stunt_material;
            if (stunt_timer > 0) {
                stunt_timer -= Time.deltaTime;
            } else {
                stunt_timer = stunt_timer_init;
                stunt = false;
                mesh_renderer.material = normal_material;
            }
        }
    }

}
