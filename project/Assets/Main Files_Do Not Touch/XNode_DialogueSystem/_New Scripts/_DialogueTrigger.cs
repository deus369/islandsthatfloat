using UnityEngine;
using UnityEngine.Events;
using StarterAssets;

//! Add this to npcs for interaction when close.
/**
*   ADD TO INTERACTABLE
*/
public class _DialogueTrigger : MonoBehaviour
{
    public _InteractType interactType;
    public DialogueGraph dialogueGraph;
    public UnityEvent<GameObject> onFirstInteract;
    public UnityEvent<GameObject> onInteract;
    // public UnityEvent onInteractEnd;
    //public Color interactColor;
    private int interactionCount;
    public GameObject mouseUI;
    
    private void OnTriggerEnter(Collider collider)
    {
        
        GameObject playerObject = collider.gameObject;
        if(playerObject.tag == "Player")
        {
            Trigger(playerObject, _InteractType.TriggerCollider);
            //! Add this interactable to player nearby interactables.
            if (interactType == _InteractType.TriggerEnterClick)
            {
                mouseUI.SetActive(true);
                var playerTriggerer = playerObject.GetComponent<_PlayerTriggerer>();
                if (playerTriggerer)
                {
                    playerTriggerer.AddInteractable(gameObject, this);
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        mouseUI.SetActive(false);
        GameObject playerObject = collider.gameObject;
        if(playerObject.tag == "Player")
        {
            //! Remove this interactable to player nearby interactables.
            if (interactType == _InteractType.TriggerEnterClick)
            {
                var playerTriggerer = playerObject.GetComponent<_PlayerTriggerer>();
                if (playerTriggerer)
                {
                    playerTriggerer.RemoveInteractable(gameObject);
                }
            }
        }
    }

    public bool CanTrigger(_InteractType triggerType)
    {
        return interactType == _InteractType.Any || interactType == triggerType;
    }

    //! returns true if triggered properly.
    public void Trigger(GameObject triggerer, _InteractType triggerType)
    {
        if (CanTrigger(triggerType))
        {
            //! I need sleep fml.
            GenericTriggerFunction(triggerer);
        }
    }

    public void TriggerFromTriggerer(GameObject triggerer)
    {
        GenericTriggerFunction(triggerer);
    }

    private void GenericTriggerFunction(GameObject triggerer)
    {
        interactionCount++;
        if (interactionCount == 1)
        {
            if (onFirstInteract != null)
            {
                onFirstInteract.Invoke(triggerer);
            }
        }
        if (onInteract != null)
        {
            onInteract.Invoke(triggerer);
        }
    }

    public void EnableDialogue(GameObject playerObject)
    {
        _NodeUpdater.instance.Begin(playerObject, gameObject, dialogueGraph);
    }
}
