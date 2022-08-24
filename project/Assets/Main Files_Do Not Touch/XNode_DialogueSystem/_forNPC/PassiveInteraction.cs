using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PassiveInteraction : MonoBehaviour
{
    private UnityEvent p_Interaction;

    public void DoPassiveInteraction()
    {
        p_Interaction.Invoke();
    }
    public InteractionInstigator col_Num;
    public int colliderNumber; 
    public Animator animFadeOut;
    public bool ExitTriggerCollider; 
    private void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            col_Num.c = colliderNumber;
            ExitTriggerCollider = false; 
            //animFadeOut.SetBool("FadeOut", false);
        }
    }
    private void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "Player"){
            ExitTriggerCollider = true; 
            //animFadeOut.SetBool("FadeOut", true);
        }
    }

    
    /* void Update(){
        if (bool_trigger.colTrig == true){
            this.GetComponent<BoxCollider>().enabled = true; 
        }
        if (bool_trigger.colTrig == false){
            this.GetComponent<BoxCollider>().enabled = false; 
        }
    } */



}
