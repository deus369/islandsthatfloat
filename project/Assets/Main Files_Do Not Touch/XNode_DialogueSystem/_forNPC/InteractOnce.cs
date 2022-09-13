using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StarterAssets;


public class InteractOnce : MonoBehaviour
{
    public GameObject Player;
    private UnityEvent interactingOnce;
    public void interacted_once(){
        interactingOnce.Invoke();
    }
    public InteractionInstigator col_Num;
    public int colliderNumber; 
    private void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            col_Num.c = colliderNumber;     
        }
    }

    
}
