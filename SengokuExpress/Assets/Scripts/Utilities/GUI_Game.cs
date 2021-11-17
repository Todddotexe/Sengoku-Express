using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUI_Game : MonoBehaviour {
    public Slider bark_meter = null;
    public TMP_Text health_text = null;
    public Image hp_board_image = null;
    [HideInInspector] public Material hp_board_material = null;
    void Awake() {
        Debug.Assert(bark_meter  != null);
        Debug.Assert(health_text != null);
        Debug.Assert(hp_board_image != null);
        hp_board_material = hp_board_image.material;
    }
}
