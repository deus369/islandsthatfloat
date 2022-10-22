using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");   
        Application.Quit();
    }

    public void SetVolume(float value)
    {  
        float newVolume = 0f;
        if (value != 0)
        {
            newVolume = Mathf.Log10(value) * 20 + 2;
        }
        else
        {
            newVolume = -80f;
        }
        audioMixer.SetFloat("VolumeMusic", newVolume);
        // Debug.Log("Set music volume to: " + newVolume);
    }
}
