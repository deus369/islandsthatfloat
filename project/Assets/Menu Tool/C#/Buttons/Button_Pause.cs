using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Pause : MonoBehaviour
{
    private Button button;
    [Tooltip("the Pause Script. button will not work without it")]
    public Pause pauseScript;
    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(ButtonPausePlay);
        }
    }

    public void ButtonPausePlay()
    {
        pauseScript.PausePlay();
    }
}
