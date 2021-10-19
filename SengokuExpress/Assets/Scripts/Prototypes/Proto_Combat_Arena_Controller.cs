using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proto_Combat_Arena_Controller : MonoBehaviour {

    BoxCollider trigger = null;
    public Transform player = null;
    public Transform area_to_enable = null;

    void Start() {
        trigger = GetComponent<BoxCollider>();
        if (area_to_enable != null) {
            area_to_enable.gameObject.SetActive(false); // desable the area we want to appear
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (player == null) return;
        if (area_to_enable == null) return;

        if (collider.gameObject == player.gameObject) {
            // Instantiate enemies
            area_to_enable.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
