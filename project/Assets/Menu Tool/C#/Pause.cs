using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [Tooltip("The HUD or UI parent that needs to be hidden when game is paused")]
    public GameObject HUD;
    public bool isTimePausable; 
    public bool isPaused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausePlay();
        }
    }

    public void PausePlay()
    {
        isPaused = !isPaused;
        Debug.Log("Game Paused = " + isPaused);

        if(HUD == null)
        {
            Debug.LogWarning(gameObject.name + " Script ''Pause''  : HUD NOT found. The HUD or UI parent that is hidden when game is paused");
        }

        if(isPaused == true)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
            gameObject.GetComponent<CanvasGroup>().interactable = true;
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

            if (HUD != null)
            {
                HUD.GetComponent<CanvasGroup>().alpha = 0;
                HUD.GetComponent<CanvasGroup>().interactable = false;
                HUD.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0;
            gameObject.GetComponent<CanvasGroup>().interactable = false;
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;

            if (HUD != null)
            {
                HUD.GetComponent<CanvasGroup>().alpha = 1;
                HUD.GetComponent<CanvasGroup>().interactable = true;
                HUD.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }

        if (isTimePausable == true)
        {
            if(isPaused == true)
            {
                Time.timeScale = 0;
            }
            else
            { Time.timeScale = 1; }
            Debug.Log("Time Scale = " + Time.timeScale);
        }
    }
}
