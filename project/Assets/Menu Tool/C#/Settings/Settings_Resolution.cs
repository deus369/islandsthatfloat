using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Settings_Resolution : MonoBehaviour
{
    public SettingsController settingsController;
    private TMP_Dropdown dropdown;

    // Start is called before the first frame update
    void Start()
    {
        dropdown = gameObject.GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();
        Resolution();

        dropdown.onValueChanged.AddListener(SetResolution);
    }

    public void Resolution()
    {
        settingsController.GetResolution(dropdown);
    }

    public void SetResolution(int tp_resIndex)
    {
        settingsController.SetResolution(tp_resIndex);
    }

}
