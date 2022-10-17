using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Used to rotate floating books.
/**
*   ADD TO INTERACTABLE
*/
public class SimpleRotation : MonoBehaviour
{
    private Quaternion rotateForce2;
    private Transform target;
    public Vector3 rotateForce = new Vector3(1, 0, 0);
    public float leaveDistance = 3f;

    // Start is called before the first frame update
    void Start()
    {
        rotateForce2 = Quaternion.Euler(rotateForce);   
    }

    public void TargetPlayer(GameObject player)
    {
        target = player.transform;
    }

    public void SpinToWin()
    {
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
                SpinToWin();
            }
        }
        else
        {
            transform.rotation *= rotateForce2;
        }
    }
}
