using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    private DialogueManager dialogueManager;
    public string characterName;
    public string[] dialogueLines;
    

    public Cat cat;
    public GameObject catSpawn;
    private bool catSpawned;

	// Use this for initialization
	void Start () {
        dialogueManager = FindObjectOfType<DialogueManager>();
        catSpawned = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerLongRange")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            if (characterName == "Peeves" && player.inventory.ItemInInventory(cat))
            {
                dialogueLines = new string[3];
                dialogueLines[0] = "You found my cat!\nThank you so much!!";
                dialogueLines[1] = "Welcome home, Mr. Fuzzywuzzy!";
                dialogueLines[2] = "The game is over now, thank you for playing!";
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerMeleeRange")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            if (!dialogueManager.dialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                dialogueManager.ShowDialogue(characterName, dialogueLines);
                if (characterName == "Peeves" && player.inventory.ItemInInventory(cat) && !catSpawned)
                {
                    catSpawned = true;
                    player.inventory.RemoveItem(cat);
                    Instantiate(catSpawn, new Vector3(transform.position.x + 3, transform.position.y, transform.position.z), Quaternion.Euler(Vector3.zero));
                }
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
