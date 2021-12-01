using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class protoJoeTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == Global.tag_player)
        {
            Global.main_camera.look_at_joe();
            Global.player_controller.won = true;
        }
    }
}
