using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StarterAssets;


public class InteractOnce : MonoBehaviour
{
    private UnityEvent interactingOnce;
    public void interacted_once(){
        interactingOnce.Invoke();
    }
    public InteractionInstigator interactionInstigator;
    public NodeParser nodeParser;
    public int colliderNumber; 
    public int propNumber;
    private void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            interactionInstigator.c = colliderNumber; //disables collider after interaction
            interactionInstigator.p = propNumber;   
            nodeParser.p = propNumber;  
        }
    }

    
}
