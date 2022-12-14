using UnityEngine;
using UnityEngine.Events;

//! Use for raycasting interactables.
/**
*   ADD TO PLAYER
*/
public class PlayerRaycaster : MonoBehaviour
{
    private bool isRaycastingThing = false;
    public float maxRaycast = 3f;
    public UnityEvent<Color> onRaycastThing;
    public UnityEvent onRaycastNothing;
    public UnityEvent onTriggeredThing;
    private GameObject targetInteractable;

    void Update()
    {
        if (NodeUpdater.instance.IsBusy())
        {
            return;
        }
        if (targetInteractable)
        {
            var mouseButtonDown = Input.GetMouseButtonDown(0);
            if (mouseButtonDown)
            {
                var dialogueTrigger = targetInteractable.GetComponent<DialogueTrigger>();
                if (dialogueTrigger)
                {
                    dialogueTrigger.Trigger(gameObject, InteractType.Raycast);
                    if (onTriggeredThing != null)
                    {
                        onTriggeredThing.Invoke();
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (NodeUpdater.instance.IsBusy())
        {
            return;
        }
        var didRaycastThing = false;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width, Screen.height) / 2f); // Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRaycast))
        {
            var hitTransform = hit.transform;
            if (hitTransform)
            {
                var dialogueTrigger = hitTransform.gameObject.GetComponent<DialogueTrigger>();
                if (dialogueTrigger)
                {
                    var canTrigger = dialogueTrigger.CanTrigger(InteractType.Raycast);
                    if (canTrigger)
                    {
                        didRaycastThing = true;
                        if (!isRaycastingThing)
                        {
                            isRaycastingThing = true;
                            targetInteractable = hitTransform.gameObject;
                            if (onRaycastThing != null)
                            {
                                Debug.DrawRay(ray.origin, hit.point, Color.red, 2);
                                UnityEngine.Debug.Log("Raycasted Interactable: " + hitTransform.gameObject.name);
                                onRaycastThing.Invoke(dialogueTrigger.interactColor);
                            }
                        }
                    }
                }
            }
        }
        if (!didRaycastThing)
        {
            targetInteractable = null;
            OnRaycastNothing();
        }
    }

    void OnRaycastNothing()
    {
        if (isRaycastingThing)
        {
            isRaycastingThing = false;
            if (onRaycastNothing != null)
            {
                onRaycastNothing.Invoke();
            }
        }
    }
}
