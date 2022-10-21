using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBringer : MonoBehaviour
{
    public void BringDarkness()
    {
        Debug.Log("Turned all lights off, from: " + gameObject);
        var lights = GameObject.FindObjectsOfType<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].type == LightType.Directional)
            {
                lights[i].gameObject.SetActive(false);
            }
        }
    }

    public void RestoreTheLight()
    {
        var lights = GameObject.FindObjectsOfType<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].type == LightType.Directional)
            {
                lights[i].gameObject.SetActive(true);
            }
        }
    }
}
