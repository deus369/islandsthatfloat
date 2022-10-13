using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_OpenClosePanel : MonoBehaviour
{
    [Tooltip("The menu containing the button. e.g. Main menu or Pause menu")]
    public GameObject previousMenu;
    [Tooltip("The menu to open when button is pressed")]
    public GameObject panelToOpenClose;
    public bool isOpen;

    private Button button;
    private Pause Paused;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OpenClosePanel);
        Paused = previousMenu.GetComponent<Pause>();
    }

    void Update()
    {
        if (panelToOpenClose != null)
        { 
            if (isOpen)
            {
                if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
                {
                    OpenClosePanel();
                }
            }
        }
    }

    void OpenClosePanel()
    {
        if (panelToOpenClose != null)
        {
            if (previousMenu != null)
            {
                if (!isOpen)
                {
                    panelToOpenClose.GetComponent<CanvasGroup>().alpha = 1;
                    panelToOpenClose.GetComponent<CanvasGroup>().interactable = true;
                    panelToOpenClose.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    
                    previousMenu.GetComponent<CanvasGroup>().alpha = 0;
                    previousMenu.GetComponent<CanvasGroup>().interactable = false;
                    previousMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
                    
                    isOpen = true;
                    return;
                }
                if (isOpen)
                {

                    panelToOpenClose.GetComponent<CanvasGroup>().alpha = 0;
                    panelToOpenClose.GetComponent<CanvasGroup>().interactable = false;
                    panelToOpenClose.GetComponent<CanvasGroup>().blocksRaycasts = false;

                    if(Paused != null)
                    {
                        if (Paused.isPaused == true)
                        {
                            previousMenu.GetComponent<CanvasGroup>().alpha = 1;
                            previousMenu.GetComponent<CanvasGroup>().interactable = true;
                            previousMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
                        }
                    }
                    else
                    {
                        previousMenu.GetComponent<CanvasGroup>().alpha = 1;
                        previousMenu.GetComponent<CanvasGroup>().interactable = true;
                        previousMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    }
                    isOpen = false;
                }
            }
            else { Debug.LogError(gameObject.name + " Script ''Button_OpenClosePanel''  :  previousMenu NOT found."); }
        }
        else { Debug.LogError(gameObject.name + " Script ''Button_OpenClosePanel''  :  panelToOpenClose NOT found."); }
    }
}
