﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject dialogueBox;
    public Text dialogueText;

    public bool dialogueActive;

    public string[] dialogueLines;
    public int currentLine;

	// Use this for initialization
	void Start () {
        dialogueActive = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (dialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            //dialogueActive = false;
            //dialogueBox.SetActive(false);
            currentLine++;
        }
        if (currentLine >= dialogueLines.Length)
        {
            dialogueActive = false;
            dialogueBox.SetActive(false);
            currentLine = 0;
        }
        if (dialogueActive)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
	}

    public void ShowBox (string dialogue)
    {
        dialogueActive = true;
        dialogueBox.SetActive(true);
        dialogueText.text = dialogue;
    }

    public void ShowDialogue (string[] dialogue)
    {
        dialogueActive = true;
        dialogueBox.SetActive(true);
        dialogueLines = dialogue;
        currentLine = 0;
    }
}
