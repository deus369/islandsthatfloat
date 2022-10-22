using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Used to rotate floating books.
/**
*   ADD TO INTERACTABLE
*/
public class NPCRotateToPlayer : MonoBehaviour
{
    public float leaveDistance = 3f;
    private Quaternion rotateForce2;
    private Transform target;
    private Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.rotation;
    }

    public void TargetPlayer(GameObject player)
    {
        target = player.transform;
    }

    public void UnTargetPlayer()
    {
        transform.rotation = originalRotation;
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            transform.LookAt(target);
            // transform.rotation = -transform.rotation;
            var currentEulerAngles = transform.eulerAngles;
            currentEulerAngles.x = 0;
            currentEulerAngles.y += 180;
            currentEulerAngles.z = 0;
            transform.eulerAngles = currentEulerAngles;
            var distanceTo = Vector3.Distance(transform.position, target.position);
            if (distanceTo >= leaveDistance)
            {
                UnTargetPlayer();
            }
        }
    }
}
