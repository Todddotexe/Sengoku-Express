using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Proto_Combat_Arena_Controller : MonoBehaviour {

    BoxCollider trigger = null;
    public Transform player = null;
    public Transform arena_to_enable = null;
    [HideInInspector] public List<Enemy_Controller> enemies = new List<Enemy_Controller>();

    void Start() {
        trigger = GetComponent<BoxCollider>();
        if (arena_to_enable != null) {
            enemies.AddRange(arena_to_enable.GetComponentsInChildren<Enemy_Controller>());
            enemies.ForEach(enemy => enemy.arena = this);
            arena_to_enable.gameObject.SetActive(false); // disable the area we want to appear
        }
    }
    
    void OnTriggerEnter(Collider collider) {
        if (player == null) return;
        if (arena_to_enable == null) return;

        if (collider.gameObject == player.gameObject) {
            // Instantiate enemies
            arena_to_enable.gameObject.SetActive(true);
            trigger.enabled = false;    
        }
    }

    public void update_status() {
        if (arena_to_enable != null) {
            foreach (var enemy in enemies) {
                if (enemy.is_alive) return;
            }
            // we have looped through all enemies and non of them were alive. So disable this arena
            Destroy(arena_to_enable.gameObject); // no need for this object anymore
            Destroy(gameObject); // no need for this object anymore
        }
    }
}
