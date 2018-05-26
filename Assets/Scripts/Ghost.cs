using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    private DialogueManager dialogueManager;
    public string characterName;
    public string[] dialogueLines;

	// Use this for initialization
	void Start () {
        dialogueManager = FindObjectOfType<DialogueManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerMeleeRange")
        {
            if (!dialogueManager.dialogueActive && Input.GetKeyDown(KeyCode.Space))
            {
                dialogueManager.ShowDialogue(characterName, dialogueLines);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerMeleeRange")
        {
            if (dialogueManager.dialogueActive)
            {
                dialogueManager.CloseDialogue();
            }
        }

    }
}
