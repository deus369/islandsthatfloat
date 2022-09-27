using UnityEngine;
using UnityEngine.UI;
using XNode; 
using TMPro;
using System;
using StarterAssets;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class NodeUpdater : MonoBehaviour
{
    public static NodeUpdater instance;
    public Animator animFadeOut;
    [Header("Events")]
    public UnityEvent onStartDialogue;  // Show dialogue ui
    public UnityEvent onEndDialogue;  // Hide dialogue ui
    public UnityEvent onBeginNode;  // clear dialogue ui
    public UnityEvent<string, string> onSetNode;  // clear dialogue ui
    public UnityEvent<ChoiceDialogueNode> onSetChoice;
    // dialogue
    private ChoiceDialogueNode activeChoiceDialogueNode;
    private DialogueGraph dialogueGraph;
    private Coroutine dialogueRoutine;
    private GameObject player;
    private GameObject interactable;

    void Awake()
    {
        NodeUpdater.instance = this;
    }

    //! Starts the dialogue system
    public void Begin(GameObject player, GameObject interactable, DialogueGraph dialogueGraph)
    {
        this.player = player;
        this.interactable = interactable;
        this.dialogueGraph = dialogueGraph;
        if (onStartDialogue != null)
        {
            onStartDialogue.Invoke();
        }
        SetNode("Start");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        player.GetComponent<FirstPersonController>().enabled = false;
    }

    //! Ends the dialogue tree
    public void End()
    {
        if (onEndDialogue != null)
        {
            onEndDialogue.Invoke();
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<FirstPersonController>().enabled = true;
    }

    public void SetNode(string nextNodeName)
    {
        Debug.Log("Going to Node [" + nextNodeName + "]");
        //! find start node!
        BaseNode targetNode = null;
        foreach (BaseNode node in dialogueGraph.nodes)
        {  
            // Debug.LogError("    Checking node: " + node.GetString());
            if (node != null && node.GetString() == nextNodeName)
            {
                targetNode = node;
                break;
            }    
        }
        if (dialogueRoutine != null)
        {
            StopCoroutine(dialogueRoutine); 
            dialogueRoutine = null;
        }
        if (targetNode != null)
        {
            dialogueGraph.current = targetNode;
            dialogueRoutine = StartCoroutine(ParseNode());    
        }
        else
        {
            Debug.LogError("Target Node not found for: " + dialogueGraph.name + " :: " + nextNodeName);
        }
    }

    public void SetNodePort(string targetPortName)
    {
        Debug.Log("Going to Node Port [" + targetPortName + "]");
        //! find start node!
        NodePort targetNodePort = null;
        foreach (NodePort nodePort in dialogueGraph.current.Ports)
        {  
            // Debug.LogError("    Checking nodePort: " + nodePort.fieldName);
            if (nodePort != null && nodePort.fieldName == targetPortName)
            {
                targetNodePort = nodePort;
                break;
            }    
        }
        if (dialogueRoutine != null)
        {
            StopCoroutine(dialogueRoutine); 
            dialogueRoutine = null;
        }
        if (targetNodePort != null)
        {
            dialogueGraph.current = targetNodePort.Connection.node as BaseNode;
            dialogueRoutine = StartCoroutine(ParseNode());    
        }
        else
        {
            Debug.LogError("Target Node Port not found for: " + dialogueGraph.name + " :: " + targetPortName);
        }
    }
    
    private void SetChoice(ChoiceDialogueNode newSegment)
    {
        activeChoiceDialogueNode = newSegment;
        if (onSetChoice != null)
        {
            onSetChoice.Invoke(newSegment);
        }
    }
    
    public void AnswerClicked(int clickedIndex)
    {
        //Function when the choices button is pressed 
        BaseNode baseNode = dialogueGraph.current; 
        var port = activeChoiceDialogueNode.GetPort("Answers " + clickedIndex);
        // if node  goes to a new node
        if (port.IsConnected)
        {
            dialogueGraph.current = port.Connection.node as BaseNode; 
            dialogueRoutine = StartCoroutine(ParseNode());    
        }
        else
        {
            Debug.LogError("ERROR: ChoiceDialogue port is not connected");
            End();
        }
    }

    //! Main function for dialogue node routine. todo: Simplify more. Still speghetti hell.
    public IEnumerator ParseNode()
    { 
        // Node logic goes here
        BaseNode baseNode = dialogueGraph.current; 
        string data = baseNode.GetString(); 
        string[] dataParts = data.Split('/'); //array of strings 
        var nodeName = dataParts[0];
        UnityEngine.Debug.Log("Entered Node [" + nodeName + "]");
        // clear ui on begin node
        if (onBeginNode != null)
        {
            onBeginNode.Invoke();
        }
        if (nodeName == "Start")
        {
            SetNodePort("exit");
        }
        else if (nodeName == "ChoiceDialogueNode")
        {
            if (onSetNode != null)
            {
                onSetNode.Invoke(dataParts[1], dataParts[2]);
            }
            SetChoice(baseNode as ChoiceDialogueNode); //Instantiates the buttons 
        }
        else if (nodeName == "DialogueNode")
        {
            if (onSetNode != null)
            {
                onSetNode.Invoke(dataParts[1], dataParts[2]);
            }
        }
        else if (nodeName == "CloseDialogue_ExitNode")
        {
            End();
        }
        else if (nodeName == "CloseDialogue_ExitNode_NoLoop_toStart")
        {
            End();
        }
        else if (nodeName == "CustomNode")
        {
            SetNodePort("exit");
        }
        else if (nodeName == "FadeDialogueNode")
        { 
            if (onSetNode != null)
            {
                onSetNode.Invoke(dataParts[1], dataParts[2]);
            }
            animFadeOut.SetBool("FadeOut", false);
            yield return new WaitForSeconds(4);
            animFadeOut.SetBool("FadeOut", true);
            yield return new WaitForSeconds(1);
            SetNodePort("exit");
        }
        else if (nodeName == "HaltPassiveDialogueNode")
        { 
            if (onSetNode != null)
            {
                onSetNode.Invoke(dataParts[1], dataParts[2]);
            }
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); //waits for left mouse click input then goes to next node
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            SetNodePort("exit");

        }
        else if (nodeName == "AnimationNode")
        {
            SetNodePort("exit");
        }
        else if (nodeName == "testNode")
        {
            print("test node");
            SetNodePort("exit");
        } 
    }
}