using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Particle_Player : MonoBehaviour {
    public ParticleSystem particle = null;
    
    void OnTriggerEnter(Collider collider) {
        if (particle != null && collider.tag == Global.tag_player) {
            particle.Play();
        }
    }
}
