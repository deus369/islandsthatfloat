using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings_Volume : MonoBehaviour
{
    [Tooltip("Name of paramater in AudioMixer")]public string paramaterName;
    public SettingsController settingsController;

    private AudioMixer audioMixer;
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { SetVolume(); });
        audioMixer = settingsController.audioMixer;
        settingsController.GetVolume(paramaterName, slider);

        //Variables must be set before AddListener
        //slider.onValueChanged.AddListener(delegate { SetVolume(); });
    }

    void SetVolume()
    {
        settingsController.SetVolume(paramaterName, slider.value);
    }
}
