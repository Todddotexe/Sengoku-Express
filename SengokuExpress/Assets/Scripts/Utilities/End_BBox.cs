using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End_BBox : MonoBehaviour {

    void OnTriggerEnter(Collider col) {
        if (col.tag == Global.tag_player) {
            Global.set_game_state(Global.STATES.WIN);
            Destroy(gameObject);
        }
    }    
}
