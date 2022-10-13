using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    public AudioMixer audioMixer;

    [HideInInspector]public Resolution[] resolutions;

    public void GetVolume(string tp_paramaterName, Slider tp_slider)
    {
        float value;

        if (audioMixer != null)
        {
            audioMixer.GetFloat(tp_paramaterName, out value);
            tp_slider.value = value;
        }
        else
        {
            Debug.LogError(gameObject.name + " Script ''SettingsController''  : audioMixer NOT found. audioMixer is required for volume settings to work");
        }
    }

    public void SetVolume(string tp_paramaterName, float tp_volume)
    {
        if(audioMixer != null)
        {
            audioMixer.SetFloat(tp_paramaterName, tp_volume);
        }
        else
        {
            Debug.LogError(gameObject.name + " Script ''SettingsController''  : audioMixer NOT found. audioMixer is required for volume settings to work");
        }
    }

    public void GetResolution(TMP_Dropdown tp_dropdown)
    {
        resolutions = Screen.resolutions;

        List<string> options = new List<string>();
        int currentResIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }
        tp_dropdown.AddOptions(options);
        tp_dropdown.value = currentResIndex;
        tp_dropdown.RefreshShownValue();
    }

    public void SetResolution(int tp_resIndex)
    {
        Resolution resolution = resolutions[tp_resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void GetWindowMode(Toggle tp_toggle)
    {
        tp_toggle.isOn = Screen.fullScreen;
    }

    public void SetWindowMode(bool tp_isFullscreen)
    {
        Screen.fullScreen = tp_isFullscreen;
        Debug.Log("Fullscreen = " + Screen.fullScreen);
    }
}

//Settings to add in the future
//Video --- Brightness, Feild of view, Draw distance
//Graphics --- VSync, Max FPS, Anti-Aliasing, Supersampling, Texture Quality
//     Shadow Quality, Reflection Quality, Lighting, Volumetric Lighting/fog, 
//     PostProcessing, Tessellation, Anisotropic, HDR
