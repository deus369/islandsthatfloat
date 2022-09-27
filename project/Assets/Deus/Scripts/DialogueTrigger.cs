using UnityEngine;
using UnityEngine.Events;
using StarterAssets;

//! Add this to npcs for interaction when close.
public class DialogueTrigger : MonoBehaviour
{
    public DialogueGraph dialogueGraph;
    public UnityEvent<GameObject> onFirstInteract;
    public UnityEvent<GameObject> onInteract;
    private int interactionCount;
    
    private void OnTriggerEnter(Collider collider)
    {
        GameObject playerObject = collider.gameObject;
        if(playerObject.tag == "Player")
        {
            interactionCount++;
            if (interactionCount == 1)
            {
                if (onFirstInteract != null)
                {
                    onFirstInteract.Invoke(playerObject);
                }
            }
            if (onInteract != null)
            {
                onInteract.Invoke(playerObject);
            }
        }
    }

    public void EnableDialogue(GameObject playerObject)
    {
        NodeUpdater.instance.Begin(playerObject, gameObject, dialogueGraph);
    }
}
