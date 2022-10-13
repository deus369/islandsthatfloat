using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings_Fullscreen : MonoBehaviour
{
    public SettingsController settingsController;
    private Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        toggle = gameObject.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(SetWindowMode);
        settingsController.GetWindowMode(toggle);
    }

    public void SetWindowMode(bool tp_isFullscreen)
    {
        settingsController.SetWindowMode(tp_isFullscreen);
    }

}
