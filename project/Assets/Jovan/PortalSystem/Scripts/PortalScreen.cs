using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScreen: MonoBehaviour
{
    public Transform playerCamera; //a reference to the camera that this camera wants to mimic;
    public Transform myPortalRelativeTransform, otherPortalRelativeTransform; 
    //references to two empty gameobjects, each a child of one of the two portals involved in this effect; note that both parent objects (portals) must have the same scale (preferably (1,1,1)) in order for this to work;

    void LateUpdate()
    {
        otherPortalRelativeTransform.position = playerCamera.position; 
        //have the other empty gameobject match the position (in world space) of the player camera;
        otherPortalRelativeTransform.rotation = playerCamera.rotation; 
        //and its rotation too;

        myPortalRelativeTransform.localPosition = otherPortalRelativeTransform.localPosition; 
        //then have "my" empty gameobject match the LOCAL position of the other empty;
        myPortalRelativeTransform.localRotation = otherPortalRelativeTransform.localRotation;
        //and its LOCAL rotation;

        transform.position = myPortalRelativeTransform.position;
        //then set this camera's world-space position to match that of the empty;
        transform.rotation = myPortalRelativeTransform.rotation;
        //and same for the rotation;
    }
}