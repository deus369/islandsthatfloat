using UnityEngine;
using UnityEngine.Events;

public class PlayerRaycaster : MonoBehaviour
{
    private bool isRaycastingThing = false;
    public float maxRaycast = 3f;
    public UnityEvent<Color> onRaycastThing;
    public UnityEvent onRaycastNothing;
    public UnityEvent onTriggeredThing;

    void FixedUpdate()
    {
        var didRaycastThing = false;
        var mouseButtonDown = Input.GetMouseButtonDown(0);
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
                            if (onRaycastThing != null)
                            {
                                Debug.DrawRay(ray.origin, hit.point, Color.red, 2);
                                UnityEngine.Debug.Log("Raycasted Interactable: " + hitTransform.gameObject.name);
                                onRaycastThing.Invoke(dialogueTrigger.interactColor);
                            }
                        }
                        if (mouseButtonDown)
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
        }
        if (!didRaycastThing)
        {
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
