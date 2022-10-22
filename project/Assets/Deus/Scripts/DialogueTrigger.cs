using UnityEngine;
using UnityEngine.Events;
using StarterAssets;

//! Add this to npcs for interaction when close.
/**
*   ADD TO INTERACTABLE
*/
public class DialogueTrigger : MonoBehaviour
{
    [Header("Data")]
    public InteractType interactType;
    public Color interactColor;
    public DialogueGraph dialogueGraph;
    public bool playDialogueOnInteract;
    [Header("Events")]
    public UnityEvent<GameObject> onFirstInteract;
    public UnityEvent<GameObject> onInteract;
    public UnityEvent onInteractEnd;
    private int interactionCount;
    [Header("Disable Movement")]
    public bool isDisablePlayerMovement = true;
    [Header("Auto Play")]
    public bool isAutoPlayDialogue;
    public float textNodeSpeed = 1.4f;
    private bool isPlayingDialogue;
    private float lastHitNext;

    void Update()
    {
        if (isPlayingDialogue && Time.time - lastHitNext >= textNodeSpeed)
        {
            lastHitNext = Time.time;
            DialogueUI.instance.NextNode();
        }
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        GameObject playerObject = collider.gameObject;
        if(playerObject.tag == "Player")
        {
            Trigger(playerObject, InteractType.TriggerCollider);
            //! Add this interactable to player nearby interactables.
            if (interactType == InteractType.TriggerEnterPassive)
            {
                TriggerFromTriggerer(playerObject);
            }
            else if (interactType == InteractType.TriggerEnterClick)
            {
                var playerTriggerer = playerObject.GetComponent<PlayerTriggerer>();
                if (playerTriggerer)
                {
                    playerTriggerer.AddInteractable(gameObject, this);
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        GameObject playerObject = collider.gameObject;
        if(playerObject.tag == "Player")
        {
            //! Remove this interactable to player nearby interactables.
            if (interactType == InteractType.TriggerEnterClick)
            {
                var playerTriggerer = playerObject.GetComponent<PlayerTriggerer>();
                if (playerTriggerer)
                {
                    playerTriggerer.RemoveInteractable(gameObject);
                }
            }
        }
    }

    public bool CanTrigger(InteractType triggerType)
    {
        return interactType == InteractType.Any || interactType == triggerType;
    }

    //! returns true if triggered properly.
    public void Trigger(GameObject triggerer, InteractType triggerType)
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
        if (playDialogueOnInteract)
        {
            EnableDialogue(triggerer);
        }
        if (isAutoPlayDialogue)
        {
            isPlayingDialogue = true;
            lastHitNext = Time.time;
        }
    }

    public void EnableDialogue(GameObject playerObject)
    {
        if (isDisablePlayerMovement)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            playerObject.GetComponent<FirstPersonController>().enabled = false;
        }
        NodeUpdater.instance.Begin(playerObject, gameObject, dialogueGraph);
    }

    public void OnDialogueEnded(GameObject playerObject)
    {
        if (isAutoPlayDialogue)
        {
            isPlayingDialogue = false;
        }
        //! If was disabled, reenable player movement
        if (isDisablePlayerMovement)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            playerObject.GetComponent<FirstPersonController>().enabled = true;
        }
        if (onInteractEnd != null)
        {
            onInteractEnd.Invoke();
        }
    }
}
