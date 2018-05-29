using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OldMan : MonoBehaviour
{

    public float moveSpeed;
    private bool moving;
    private bool attacking;
    private bool wanderMode = true;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private Vector2 lastMove;
    private Vector3 moveDirection;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;

    public float timeBetweenDamage;
    private float timeBetweenDamageCounter;
    public float timeToDamage;
    private float timeToDamageCounter;
    private float timeBetweenAttack;
    private float timeBetweenAttackCounter = 0f;

    public int maxHealth;
    public int currentHealth;

    private float worldCenter;

    public Cat cat;
    public Item bird;
    public GameObject birdSpawn;
    private bool birdSpawned;

    public int monsterDamage;

    public GameObject damageNumber;
    public GameObject healthBar;
    private GameObject monsterBar;

    private bool alive = true;
    public GameObject roomDoor;

    private DialogueManager dialogueManager;
    private string characterName;
    private string[] dialogueLines;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        timeBetweenDamageCounter = Random.Range(0f, timeBetweenDamage * 0.5f);
        currentHealth = maxHealth;
        dialogueManager = FindObjectOfType<DialogueManager>();
        cat.catFree = false;
        birdSpawned = false;
        monsterBar = (GameObject)Instantiate(healthBar, transform.position, Quaternion.Euler(Vector3.zero));
        monsterBar.GetComponentInChildren<Slider>().maxValue = maxHealth;
        monsterBar.GetComponentInChildren<Slider>().value = currentHealth;
        monsterBar.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (alive)
        {
            if (wanderMode)
            {
                if (moving)
                {
                    timeToMoveCounter -= Time.deltaTime;
                    //transform.Translate(moveDirection * Time.deltaTime);
                    rigidBody.velocity = moveDirection;
                    lastMove = new Vector2(moveDirection.x, moveDirection.y);
                    if (timeToMoveCounter < 0f)
                    {
                        moving = false;
                        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
                    }
                }
                else
                {
                    timeBetweenMoveCounter -= Time.deltaTime;
                    rigidBody.velocity = Vector2.zero;

                    if (timeBetweenMoveCounter < 0f)
                    {
                        moving = true;
                        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
                        moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);

                    }
                }
            }
            else
            {
                if (moving)
                {
                    rigidBody.velocity = moveDirection;
                    lastMove = new Vector2(moveDirection.x, moveDirection.y);
                }
                else
                {
                    rigidBody.velocity = Vector2.zero;
                }

            }
        }

        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        animator.SetBool("Moving", moving);
        animator.SetBool("Attacking", attacking);

        if (alive && monsterBar.activeInHierarchy)
        {
            monsterBar.transform.position = new Vector3(transform.position.x, transform.position.y - 0.7f, transform.position.z);
            monsterBar.GetComponentInChildren<Slider>().maxValue = maxHealth;
            monsterBar.GetComponentInChildren<Slider>().value = currentHealth;
        }

        if (currentHealth <= 0 && alive)
        {
            cat.FreeCat();
            roomDoor.SetActive(false);
            //Destroy(gameObject);
            Destroy(monsterBar);
            var offset = transform.position.x + worldCenter;
            transform.position = new Vector3(worldCenter + offset, transform.position.y, transform.position.z);
            //GetComponent<SpriteRenderer>().color = new Color(112, 213, 255, 182);
            GetComponent<SpriteRenderer>().color = Color.cyan;
            alive = false;
            moving = false;
            attacking = false;
            characterName = "Marianus";
            dialogueLines = new string[2];
            dialogueLines[0] = "Huh? What happened?";
            dialogueLines[1] = "Where's my bird?";
            Debug.Log("T4 - Boss defeated: " + Time.time);
        }

        if (birdSpawned && !alive && !moving)
        {
            lastMove = new Vector2(-1f, 0f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && alive)
        {
            moving = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && alive)
        {
            moving = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerLongRange" && alive)
        {
            wanderMode = false;
            moving = true;
            worldCenter = collision.gameObject.GetComponentInParent<Player>().worldCenter;
            roomDoor.SetActive(true);
            collision.gameObject.GetComponentInParent<Player>().inCombat = true;
        }
        else if (collision.gameObject.name == "PlayerLongRange" && !alive)
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            if (player.inventory.ItemInInventory(bird))
            {
                dialogueLines = new string[1];
                dialogueLines[0] = "You found my bird!";
            }
        }
        if (collision.gameObject.name == "PlayerMeleeRange" && alive)
        {
            monsterBar.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerLongRange" && alive)
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            moveDirection = player.transform.position - transform.position;
            lastMove = new Vector2(moveDirection.x, moveDirection.y);
        }
        if (collision.gameObject.name == "PlayerMeleeRange" && alive)
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            timeBetweenAttack = player.attackTime;
            if (player.attacking && (Vector2.Angle(player.Facing(), -lastMove) < 50f))
            {
                if (timeBetweenAttackCounter > 0f)
                {
                    timeBetweenAttackCounter -= Time.deltaTime;
                }
                else
                {
                    currentHealth -= player.swordDamage;
                    var clone = (GameObject)Instantiate(damageNumber, transform.position, Quaternion.Euler(Vector3.zero));
                    clone.GetComponent<FloatingNumbers>().damageNumber = player.swordDamage;
                    timeBetweenAttackCounter = timeBetweenAttack;
                }
            }
            if (attacking)
            {
                timeToDamageCounter -= Time.deltaTime;
                if (timeToDamageCounter < 0f)
                {
                    attacking = false;
                    timeBetweenDamageCounter = timeBetweenDamage;
                }
            }
            else
            {
                timeBetweenDamageCounter -= Time.deltaTime;
                if (timeBetweenDamageCounter < 0f)
                {
                    attacking = true;
                    player.currentHealth -= monsterDamage;
                    var clone = (GameObject)Instantiate(damageNumber, player.transform.position, Quaternion.Euler(Vector3.zero));
                    clone.GetComponent<FloatingNumbers>().damageNumber = monsterDamage;
                    timeToDamageCounter = timeToDamage;
                }
            }
        }
        else if (collision.gameObject.name == "PlayerMeleeRange" && !alive)
        {
            if (!dialogueManager.dialogueActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                Player player = collision.gameObject.GetComponentInParent<Player>();
                if (!birdSpawned)
                {
                    moveDirection = player.transform.position - transform.position;
                    lastMove = new Vector2(moveDirection.x, moveDirection.y);
                }
                dialogueManager.ShowDialogue(characterName, dialogueLines);
                if (player.inventory.ItemInInventory(bird))
                {
                    birdSpawned = true;
                    player.inventory.RemoveItem(bird);
                    Instantiate(birdSpawn, new Vector3(transform.position.x - 3, transform.position.y, transform.position.z), Quaternion.Euler(Vector3.zero));
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerLongRange" && alive)
        {
            wanderMode = true;
            collision.gameObject.GetComponentInParent<Player>().inCombat = false;
        }
        else if (collision.gameObject.name == "PlayerLongRange" && !alive)
        {
            collision.gameObject.GetComponentInParent<Player>().inCombat = false;
        }
        if (collision.gameObject.name == "PlayerMeleeRange" && alive)
        {
            attacking = false;
            monsterBar.SetActive(false);
        }
        else if (collision.gameObject.name == "PlayerMeleeRange" && !alive)
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
}
