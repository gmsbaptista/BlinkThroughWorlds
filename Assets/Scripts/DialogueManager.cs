using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject dialogueBox;
    public Text dialogueText;
    public Text characterName;

    public bool dialogueActive;

    public string[] dialogueLines;
    public int currentLine;

	// Use this for initialization
	void Start () {
        CloseDialogue();
    }
	
	// Update is called once per frame
	void Update () {
		if (dialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            //dialogueActive = false;
            //dialogueBox.SetActive(false);
            currentLine++;
        }
        if (currentLine >= dialogueLines.Length)
        {
            CloseDialogue();
            //currentLine = 0;
        }
        if (dialogueActive)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
	}

    /*public void ShowBox (string dialogue)
    {
        OpenDialogue();
        dialogueText.text = dialogue;
    }*/

    public void ShowDialogue (string character, string[] dialogue)
    {
        OpenDialogue();
        characterName.text = character;
        dialogueLines = dialogue;
        currentLine = -1;
    }

    public void CloseDialogue()
    {
        dialogueActive = false;
        dialogueBox.SetActive(false);
    }

    public void OpenDialogue()
    {
        dialogueActive = true;
        dialogueBox.SetActive(true);
    }
}
