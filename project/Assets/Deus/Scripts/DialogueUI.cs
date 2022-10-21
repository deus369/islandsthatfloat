using UnityEngine;
using UnityEngine.UI;
using TMPro;

//! Seperate out UI parts from dialogue logic.
/**
*   ADD TO DIALOGUEUI
*/
public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;

    [Header("UI Scene")]
    public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogue; 
    public GameObject dialogueBox;
    public GameObject buttonContainer;
    public Animator animFadeOut;

    [Header("UI Prefabs")]
    public GameObject nextUIPrefab;
    public GameObject choiceUIPrefab;

    void Awake()
    {
        instance = this;
        Hide();
    }

    public void Show()
    {
        dialogueBox.SetActive(true);
    }

    public void Hide()
    {
        dialogueBox.SetActive(false);
    }

    public void Clear()
    {
        speaker.text = "";
        dialogue.text = "";
        ClearButtons();
    }

    private void ClearButtons()
    {
        foreach (Transform child in buttonContainer.transform)
        {
            // Don't destroy the prefab
            if (child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void SetDialogueText(string speakerName, string speech)
    {
        speaker.text = speakerName;
        dialogue.text = speech;
        if(speaker.text == "")
        {
            Debug.LogError("ERROR: Speaker text for ChoiceDialogueNode is empty");
        }
        if(dialogue.text == "")
        {
            Debug.LogError("ERROR: Dialogue text for ChoiceDialogueNode is empty");
        }
    }

    public void SpawnSingleChoice()
    {
        ClearButtons();
        var button = Instantiate(nextUIPrefab, buttonContainer.transform); //spawns the buttons 
        button.SetActive(true);
        button.GetComponentInChildren<TextMeshProUGUI>().text = ">";
        button.GetComponentInChildren<Button>().onClick.AddListener((() => { NextNode();}));
    }

    public void SpawnChoiceDialogue(ChoiceDialogueNode newSegment)
    {
        ClearButtons();
        int answerIndex = 0;
        foreach (var answer in newSegment.Answers)
        {
            var button = Instantiate(choiceUIPrefab, buttonContainer.transform); //spawns the buttons 
            button.SetActive(true);
            button.GetComponentInChildren<TextMeshProUGUI>().text = answer;
            var index = answerIndex;
            button.GetComponentInChildren<Button>().onClick.AddListener((() => { Choice(index);}));
            answerIndex++;
        }
    }

    public void NextNode()
    {
        NodeUpdater.instance.SetNodePort("exit");
        // player.GetComponent<NodeUpdater>().SetNodePort("exit");
    }

    public void Choice(int index)
    {
        // buttonContainer.SetActive(false);   // do i need this
        NodeUpdater.instance.AnswerClicked(index);
    }
}