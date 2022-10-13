using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingScreen : MonoBehaviour
{
    public GameObject mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0;
            gameObject.GetComponent<CanvasGroup>().interactable = false;
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;

            mainMenu.GetComponent<CanvasGroup>().alpha = 1;
            mainMenu.GetComponent<CanvasGroup>().interactable = true;
            mainMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;

            gameObject.SetActive(false);
        }
    }
}
