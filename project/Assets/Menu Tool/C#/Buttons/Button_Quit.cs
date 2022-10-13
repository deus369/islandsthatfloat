using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Quit : MonoBehaviour
{
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Quit);
    }

    void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
