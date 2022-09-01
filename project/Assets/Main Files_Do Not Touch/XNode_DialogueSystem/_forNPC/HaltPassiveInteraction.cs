using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using StarterAssets;
public class HaltPassiveInteraction : MonoBehaviour
{
    public GameObject Player;
    private UnityEvent haltPassive_Interaction;

    public void PassiveHalt_Interaction(){
        haltPassive_Interaction.Invoke();
    }

    public InteractionInstigator col_Num;
    public int colliderNumber; 
    private void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            col_Num.c = colliderNumber;
            Player.GetComponent<FirstPersonController>().enabled = false;

        }
    }
    private void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Player"){
            //Player.GetComponent<FirstPersonController>().enabled = true;

        }
    }
}
