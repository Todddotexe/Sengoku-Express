using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Game : MonoBehaviour {
    public Slider bark_meter = null;
    void Start() {
        // bark_meter = GetComponent<Slider>();
        Debug.Assert(bark_meter != null);
    }
}
