using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class protoTriggerShow : MonoBehaviour
{
    public GameObject intro_gui;
    void OnTriggerEnter(Collider collider) {
        if (intro_gui != null && collider.tag == Global.tag_player) {
            intro_gui.SetActive(true);
        }
    }
    
    void OnTriggerExit(Collider collider) {
        if (intro_gui != null && collider.tag == Global.tag_player) {
            intro_gui.SetActive(false);
        }
    }
}
