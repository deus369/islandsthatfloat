using UnityEngine;
using UnityEngine.Events;

//! Use for raycasting interactables.
/**
*   ADD TO PLAYER
*/
public class _PlayerRaycaster : MonoBehaviour
{
    private bool isRaycastingThing = false;
    public float maxRaycast = 3f;
    public UnityEvent<Color> onRaycastThing;
    public UnityEvent onRaycastNothing;
    public UnityEvent onTriggeredThing;
    private GameObject targetInteractable;
    public GameObject mouseUI;
    public GameObjectIndex[] gameObjectIndex;
    public _NodeUpdater nodeUpdater;
    
    

    void Update()
    {
        if (_NodeUpdater.instance.IsBusy())
        {
            return;
        }
        if (targetInteractable)
        {
            var mouseButtonDown = Input.GetMouseButtonDown(0);
            if (mouseButtonDown)
            {
                mouseUI.SetActive(false);
                var dialogueTrigger = targetInteractable.GetComponent<_DialogueTrigger>();
                if (dialogueTrigger)
                {
                    dialogueTrigger.Trigger(gameObject, _InteractType.Raycast);
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
        if (_NodeUpdater.instance.IsBusy())
        {
            return;
        }
        var didRaycastThing = false;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width, Screen.height) / 2f); // Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRaycast))
        {
            /* for(int i = 0; i < gameObjectIndex.Length; i++){
                gameObjectIndex[i].propNumber = nodeUpdater.p; 
                gameObjectIndex[i].particleNumber = nodeUpdater.f; 
                gameObjectIndex[i].npcNumber = nodeUpdater.n;
            } */
            
            
            
            mouseUI.SetActive(true);
            var hitTransform = hit.transform;
            if (hitTransform)
            {
                var dialogueTrigger = hitTransform.gameObject.GetComponent<_DialogueTrigger>();
                if (dialogueTrigger)
                {
                    var canTrigger = dialogueTrigger.CanTrigger(_InteractType.Raycast);
                    if (canTrigger)
                    {
                        for(int i = 0; i < gameObjectIndex.Length; i++){
                            /* hitTransform.GetComponent<GameObjectIndex>().propNumber = nodeUpdater.p;
                            hitTransform.GetComponent<GameObjectIndex>().particleNumber = nodeUpdater.f; 
                            hitTransform.GetComponent<GameObjectIndex>().npcNumber = nodeUpdater.n;  */
                            /* gameObjectIndex[i].propNumber = nodeUpdater.p; 
                            gameObjectIndex[i].particleNumber = nodeUpdater.f; 
                            gameObjectIndex[i].npcNumber = nodeUpdater.n; */
                           
                            print("for loop");
                        }
                        didRaycastThing = true;
                        if (!isRaycastingThing)
                        {
                            isRaycastingThing = true;
                            targetInteractable = hitTransform.gameObject;
                            if (onRaycastThing != null)
                            {
                                Debug.DrawRay(ray.origin, hit.point, Color.red, 2);
                                UnityEngine.Debug.Log("Raycasted Interactable: " + hitTransform.gameObject.name);
                                //onRaycastThing.Invoke(dialogueTrigger.interactColor);
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
        mouseUI.SetActive(false);
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
