using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GUI_Game : MonoBehaviour {
    public Slider bark_meter = null;

    void Start() {
        bark_meter = GetComponent<Slider>();
    }
}
