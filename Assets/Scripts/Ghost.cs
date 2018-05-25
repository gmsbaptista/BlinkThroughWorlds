using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    private DialogueManager dialogueManager;
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
        if (collision.gameObject.name == "PlayerTrigger")
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                //dialogueManager.ShowBox(dialogue);
                if (!dialogueManager.dialogueActive)
                {
                    dialogueManager.ShowDialogue(dialogueLines);
                }
            }
        }
    }
}
