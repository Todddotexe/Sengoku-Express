using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProtoVolCon : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetMasterVol(float sliderValue)
    {
        mixer.SetFloat("masterVol", Mathf.Log10(sliderValue) * 20);
    }
    public void SetPlayerVol(float sliderValue)
    {
        mixer.SetFloat("playerVol", Mathf.Log10(sliderValue) * 20);
    }
    public void SetEnvVol(float sliderValue)
    {
        mixer.SetFloat("envVol", Mathf.Log10(sliderValue) * 20);
    }
    public void SetUIVol(float sliderValue)
    {
        mixer.SetFloat("uiVol", Mathf.Log10(sliderValue) * 20);
    }
}
