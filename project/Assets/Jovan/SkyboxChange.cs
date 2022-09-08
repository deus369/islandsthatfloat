using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChange : MonoBehaviour
{
    public Material skyBox;
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")){
            RenderSettings.skybox = skyBox;  
        } 
    }
    
}
