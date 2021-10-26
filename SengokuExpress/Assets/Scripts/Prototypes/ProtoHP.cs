using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtoHP : MonoBehaviour
{
    public int maxHP;
    public int curHP;

    private void Awake()
    {
        curHP = maxHP;
    }

    public void damage(int dmg)
    {
        curHP -= dmg;
    }

}
