using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

//! Will keep a list of interactables that are nearby
public class PlayerTriggerer : MonoBehaviour
{
    public List<GameObject> nearbyInteractables = new List<GameObject>();
    public UnityEvent<Color> onCanInteract;
    public UnityEvent onInteractableLeave;

    public void AddInteractable(GameObject interactable, in DialogueTrigger dialogueTrigger)
    {
        if (!nearbyInteractables.Contains(gameObject))
        {
            UnityEngine.Debug.Log("Interactable Added: " + gameObject.name);
            nearbyInteractables.Add(gameObject);
            if (onCanInteract != null)
            {
                onCanInteract.Invoke(dialogueTrigger.interactColor);
            }
        }
    }

    public void RemoveInteractable(GameObject interactable)
    {
        if (nearbyInteractables.Contains(gameObject))
        {
            UnityEngine.Debug.Log("Interactable Removed: " + gameObject.name);
            nearbyInteractables.Remove(gameObject);
            if (onInteractableLeave != null)
            {
                onInteractableLeave.Invoke();
            }
        }
    }
    
    void Update()
    {
        if (nearbyInteractables.Count > 0 && Input.GetMouseButtonDown(0))
        {
            var interactTarget = nearbyInteractables[0];
            if (interactTarget && interactTarget.GetComponent<DialogueTrigger>())
            {
                interactTarget.GetComponent<DialogueTrigger>().TriggerFromTriggerer(gameObject);
            }
        }
    }
}
