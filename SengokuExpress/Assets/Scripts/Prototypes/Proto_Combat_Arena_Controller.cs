using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proto_Combat_Arena_Controller : MonoBehaviour {

    BoxCollider trigger = null;
    public Transform player = null;
    public Transform area_to_enable = null;
    [HideInInspector] public List<Enemy_Controller> enemies = new List<Enemy_Controller>();

    void Start() {
        trigger = GetComponent<BoxCollider>();
        if (area_to_enable != null) {
            area_to_enable.gameObject.SetActive(false); // disable the area we want to appear
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

    public void update_status() {
        if (area_to_enable != null) {
            foreach (var enemy in enemies) {
                if (enemy.is_alive) return;
            }
            // we have looped through all enemies and non of them were alive. So disable this arena
            area_to_enable.gameObject.SetActive(false);
        }
    }
}
