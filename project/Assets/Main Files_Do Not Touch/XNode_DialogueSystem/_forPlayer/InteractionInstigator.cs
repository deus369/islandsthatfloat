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
    public GameObject[] prop_model;
    public GameObject[] ColliderTrigger; //disables collider after interaction 
   
    public int c; 
    public int p;
    //public Animator animFadeOut;
    //public bool playerTrigger = false;
    //public NodeParser nodeParser;
    //public int g;
 
    

    
    //public bool colTrig = true; 
    private List<Interactable> m_NearbyInteractables = new List<Interactable>();
    private List<PassiveInteraction> passive_NearbyInteraction = new List<PassiveInteraction>();
    private List<HaltPassiveInteraction> haltPassive_NearbyInteraction = new List<HaltPassiveInteraction>();
    private List<InteractOnce> interactingOnce = new List<InteractOnce>();
    public bool HasNearbyInteractables(){
        return m_NearbyInteractables.Count != 0;
    }
    public bool talk_once(){
        return interactingOnce.Count != 0;
    }
    public bool passive_Interaction(){
        return passive_NearbyInteraction.Count != 0;
    }
    public bool passiveHalt_Interaction(){
        return haltPassive_NearbyInteraction.Count != 0;
    }
    private void Start(){
        DialogueBox.SetActive(false);
    }

    private void Update(){
        if (HasNearbyInteractables() && Input.GetMouseButtonDown(0)){ //Input.GetButtonDown("Submit")
            DialogueBox.SetActive(true);
            Player.GetComponent<InteractionInstigator>().enabled = false;
            Player.GetComponent<FirstPersonController>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Player.GetComponent<NodeParser>().NextNode("exit");  //makes sure that StartNode is not activated automatically    
        }
        if (talk_once() && Input.GetMouseButtonDown(0)){ //Input.GetButtonDown("Submit")
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
            Cursor.visible = false;
            Player.GetComponent<NodeParser>().NextNode("exit");
        }
        if (passiveHalt_Interaction()){ //add something more here
            DialogueBox.SetActive(true);
            Player.GetComponent<InteractionInstigator>().enabled = false;
            //Player.GetComponent<FirstPersonController>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            Player.GetComponent<NodeParser>().NextNode("exit");
            //if(Input.GetMouseButtonDown(0)){Player.GetComponent<NodeParser>().NextNode("exit");}
            

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
            ColliderTrigger[c].GetComponent<BoxCollider>().enabled = true;  
               
        }
        InteractOnce o_interactingOnce = other.GetComponent<InteractOnce>();
        if (o_interactingOnce != null){
            interactingOnce.Add(o_interactingOnce); 
            ColliderTrigger[c].GetComponent<BoxCollider>().enabled = true;      
        }
        HaltPassiveInteraction haltPassive_Interaction = other.GetComponent<HaltPassiveInteraction>();
        if (haltPassive_Interaction != null){
            haltPassive_NearbyInteraction.Add(haltPassive_Interaction); 
            
        }
    }

    public void OnTriggerExit(Collider other){
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null){
            m_NearbyInteractables.Remove(interactable);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        InteractOnce o_interactingOnce = other.GetComponent<InteractOnce>();
        if (o_interactingOnce != null){
            interactingOnce.Remove(o_interactingOnce); 
            if (prop_model[p].GetComponent<MeshRenderer>().enabled == false){ //Disables collider trigger if mesh renderer is disabled. It's in effect after leaving collider trigger
                ColliderTrigger[c].GetComponent<BoxCollider>().enabled = false;
            }
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;    
        }
        PassiveInteraction p_interactable = other.GetComponent<PassiveInteraction>();
        if (p_interactable != null){
            passive_NearbyInteraction.Remove(p_interactable);  
            ColliderTrigger[c].GetComponent<BoxCollider>().enabled = false;  
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;      
        }
        HaltPassiveInteraction haltPassive_Interaction = other.GetComponent<HaltPassiveInteraction>();
        if (haltPassive_Interaction != null){
            haltPassive_NearbyInteraction.Remove(haltPassive_Interaction); 
            ColliderTrigger[c].GetComponent<BoxCollider>().enabled = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;  
            //print("Collider Number: " + c);        
        }
    }

}