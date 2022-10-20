using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

//! Will keep a list of interactables that are nearby
/**
*   ADD TO PLAYER
*/
public class _PlayerTriggerer : MonoBehaviour
{
    public List<GameObject> nearbyInteractables = new List<GameObject>();
    private bool hasTriggered;  // wait until player leaves before they can re trigger
    public UnityEvent<Color> onCanInteract;
    public UnityEvent onInteractableLeave;
    //public _DialogueTrigger dialogueTrigger;

    public void AddInteractable(GameObject interactable, in _DialogueTrigger dialogueTrigger)
    {
        if (!nearbyInteractables.Contains(interactable))
        {
            UnityEngine.Debug.Log("Interactable Added: " + interactable.name);
            nearbyInteractables.Add(interactable);
            if (onCanInteract != null)
            {
                onCanInteract.Invoke(dialogueTrigger.interactColor);
            }
        }
    }

    public void RemoveInteractable(GameObject interactable)
    {
        if (nearbyInteractables.Contains(interactable))
        {
            UnityEngine.Debug.Log("Interactable Removed: " + interactable.name);
            hasTriggered = false;
            nearbyInteractables.Remove(interactable);
            if (onInteractableLeave != null)
            {
                onInteractableLeave.Invoke();
            }
        }
    }
    
    void Update()
    {
        if (_NodeUpdater.instance.IsBusy() || hasTriggered)
        {
            return;
        }
        if (nearbyInteractables.Count > 0 && Input.GetMouseButtonDown(0)) //wait for left click 
        {
            //_DialogueTrigger.mouseUI.SetActive(false);
            var interactTarget = nearbyInteractables[0];
            if (interactTarget && interactTarget.GetComponent<_DialogueTrigger>())
            {
                hasTriggered = true;
                interactTarget.GetComponent<_DialogueTrigger>().TriggerFromTriggerer(gameObject);
                interactTarget.GetComponent<_DialogueTrigger>().mouseUI.SetActive(false);
            }
        }
    }
}
