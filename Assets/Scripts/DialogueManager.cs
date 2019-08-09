using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public Text dialogueText;
    public Text nameText;
    public GameObject dialogueBox;
    public GameObject nameBox;

    public string[] dialogueLines;

    public int currentLine;

    public static DialogueManager instance;

    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;

    private bool justStarted;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueBox.activeInHierarchy)
        {
            if(Input.GetButtonUp("Fire1"))
            {
                if (!justStarted)
                {
                    currentLine++;

                    if (currentLine >= dialogueLines.Length)
                    {
                        dialogueBox.SetActive(false);
                        
                        GameManager.instance.dialogueActive = false;

                        if(shouldMarkQuest)
                        {
                            shouldMarkQuest = false;
                            if(markQuestComplete)
                            {
                                QuestManager.instance.MarkQuestComplete(questToMark);
                            }
                            else
                            {
                                QuestManager.instance.MarkQuestIncomplete(questToMark);
                            }
                        }
                    }
                    else
                    {
                        nameChecker();
                        dialogueText.text = dialogueLines[currentLine];
                    }
                }
                else
                {
                    justStarted = false;
                }
            }
        }

        if (dialogueBox.activeInHierarchy)
        {
            if (Input.GetButtonUp("Fire2"))
            {
                if (currentLine > 0)
                {
                    currentLine--;
                }

                nameChecker();
                dialogueText.text = dialogueLines[currentLine];
            }
        }
    }

    public void ShowDialogue(string[] newLines, bool isPerson)
    {
        dialogueLines = newLines;

        currentLine = 0;

        nameChecker();

        dialogueText.text = dialogueLines[currentLine];

        dialogueBox.SetActive(true);

        justStarted = true;

        nameBox.SetActive(isPerson);
        
        GameManager.instance.dialogueActive = true;
    }

    public void nameChecker()
    {
        if (dialogueLines[currentLine].StartsWith("n-"))
        {
            nameText.text = dialogueLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }

    public void ShouldActivateQuestAtEnd(string questName, bool shouldComplete)
    {
        questToMark = questName;
        markQuestComplete = shouldComplete;

        shouldMarkQuest = true;
    }
}
