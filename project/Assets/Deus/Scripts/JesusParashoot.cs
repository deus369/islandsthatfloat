using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusParashoot : MonoBehaviour
{
    public float bottomOfHell = -50;
    Vector3 startPosition;
    Quaternion startRotation;
    
    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
    
    void Update()
    {
        if (transform.position.y < bottomOfHell)
        {
            Debug.Log("Resetting player position.");
            CharacterController characterController = gameObject.GetComponent<CharacterController>();
            characterController.enabled = false;
            transform.position = startPosition;
            transform.rotation = startRotation;
            characterController.enabled = true;
        }
    }
}
