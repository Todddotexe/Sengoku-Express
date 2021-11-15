using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI_Game : MonoBehaviour {
    public Slider bark_meter = null;
    public TMP_Text health_text = null;
    void Start() {
        // bark_meter = GetComponent<Slider>();
        Debug.Assert(bark_meter  != null);
        Debug.Assert(health_text != null);
    }
}
