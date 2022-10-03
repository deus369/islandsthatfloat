using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursorAtStart : MonoBehaviour
{
    void Start()
    {
        // lock cursor at start
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("Application is focussed");
        }
        else
        {
            Debug.Log("Application lost focus");
        }
    }
		
    /*private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }*/
}
