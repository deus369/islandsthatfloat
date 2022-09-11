using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTriggerOnExit : MonoBehaviour
{
    public void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Player"){
            this.GetComponent<BoxCollider>().enabled = false; 
        }
    }
}
