using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class protoTeleport : MonoBehaviour
{
    [SerializeField] Transform telepoint;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.position = telepoint.position;
        }
    }
}
