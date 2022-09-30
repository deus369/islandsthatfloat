using UnityEngine;
using UnityEngine.Events;
using StarterAssets;

//! Add this to npcs for interaction when close.
public class DialogueTrigger : MonoBehaviour
{
    public InteractType interactType;
    public DialogueGraph dialogueGraph;
    public UnityEvent<GameObject> onFirstInteract;
    public UnityEvent<GameObject> onInteract;
    public Color interactColor;
    private int interactionCount;
    
    private void OnTriggerEnter(Collider collider)
    {
        GameObject playerObject = collider.gameObject;
        if(playerObject.tag == "Player")
        {
            Trigger(playerObject, InteractType.TriggerCollider);
        }
    }

    public bool CanTrigger(InteractType triggerType)
    {
        return interactType == InteractType.Any || interactType == triggerType;
    }

    //! returns true if triggered properly.
    public void Trigger(GameObject triggerer, InteractType triggerType)
    {
        if (!CanTrigger(triggerType))
        {
            return;
        }
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
        NodeUpdater.instance.Begin(playerObject, gameObject, dialogueGraph);
    }
}
