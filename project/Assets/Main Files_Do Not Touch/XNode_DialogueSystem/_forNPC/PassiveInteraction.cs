using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PassiveInteraction : MonoBehaviour
{
    private UnityEvent p_Interaction;

    public void DoPassiveInteraction(){
        p_Interaction.Invoke();
    }
    public InteractionInstigator col_Num;
    public int colliderNumber; 
    private void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            col_Num.c = colliderNumber;
        }
    }


    
  



}
