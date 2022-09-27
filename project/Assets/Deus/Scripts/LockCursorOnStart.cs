using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursorOnStart : MonoBehaviour
{
    void Start()
    {
        // lock cursor at start
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
