using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoDmg : MonoBehaviour
{
    public int damage;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "enemy")
        other.GetComponent<ProtoHP>().damage(damage);

    }
}
