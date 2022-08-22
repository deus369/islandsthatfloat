using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson; //need this to access the RigidbodyFirstPersonController
using StarterAssets;
using TMPro;

public class InteractionInstigator : MonoBehaviour{

    public GameObject DialogueBox;
    public GameObject Player;
    public GameObject[] ColliderTrigger; 
    public int c; 
    public Animator animFadeOut;
 
    

    
    //public bool colTrig = true; 
    private List<Interactable> m_NearbyInteractables = new List<Interactable>();
    private List<PassiveInteraction> passive_NearbyInteraction = new List<PassiveInteraction>();

    public bool HasNearbyInteractables(){
        return m_NearbyInteractables.Count != 0;
    }
    public bool passive_Interaction(){
        return passive_NearbyInteraction.Count != 0;
    }
    private void Start(){
        DialogueBox.SetActive(false);
    }

    private void Update(){
        if (HasNearbyInteractables() && Input.GetButtonDown("Submit")){
            DialogueBox.SetActive(true);
            Player.GetComponent<InteractionInstigator>().enabled = false;
            Player.GetComponent<FirstPersonController>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Player.GetComponent<NodeParser>().NextNode("exit");  //makes sure that StartNode is not activated automatically    
        }
        if (passive_Interaction()){
            DialogueBox.SetActive(true);
            Player.GetComponent<InteractionInstigator>().enabled = false;
            Player.GetComponent<NodeParser>().NextNode("exit");
        }
    }

    public void OnTriggerEnter(Collider other){
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null){
            m_NearbyInteractables.Add(interactable);    
        }

        PassiveInteraction p_interactable = other.GetComponent<PassiveInteraction>();
        if (p_interactable != null){
            passive_NearbyInteraction.Add(p_interactable); 
            //animFadeOut.SetBool("FadeOut", false);
            ColliderTrigger[c].GetComponent<BoxCollider>().enabled = true;  
            print("Collider Number: " + c);
            //colTrig = true;          
        }
    }

    public void OnTriggerExit(Collider other){
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null){
            m_NearbyInteractables.Remove(interactable);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        PassiveInteraction p_interactable = other.GetComponent<PassiveInteraction>();
        if (p_interactable != null){
            //StartCoroutine(wait());
            //DialogueBox.SetActive(false);

            passive_NearbyInteraction.Remove(p_interactable);  
            ColliderTrigger[c].GetComponent<BoxCollider>().enabled = false;  
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; 
            //StartCoroutine(wait());
            //Player.GetComponent<NodeParser>().NextNode_Fade("exit");
            //colTrig = false;      
        }
    }
    IEnumerator wait(){
        //DialogueBox.SetActive(true);
        //animFadeOut.SetBool("FadeOut", true);
        yield return new WaitForSeconds(5f);
    }
}