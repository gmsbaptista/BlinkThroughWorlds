using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Cat cat;

    public int monsterDamage;

    public GameObject damageNumber;

    //private bool inCombat = false;
    public GameObject roomDoor;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        timeBetweenDamageCounter = Random.Range(0f, timeBetweenDamage * 0.5f);
        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update ()
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

        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        animator.SetBool("Moving", moving);
        animator.SetBool("Attacking", attacking);

        if (currentHealth <= 0)
        {
            cat.catFree = true;
            roomDoor.SetActive(false);
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            moving = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            moving = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerLongRange")
        {
            wanderMode = false;
            moving = true;
        }
        if (collision.gameObject.name == "PlayerMeleeRange")
        {
            roomDoor.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerLongRange")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            moveDirection = player.transform.position - transform.position;
            lastMove = new Vector2(moveDirection.x, moveDirection.y);
        }
        if (collision.gameObject.name == "PlayerMeleeRange")
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerLongRange")
        {
            wanderMode = true;
        }
        if (collision.gameObject.name == "PlayerMeleeRange")
        {
            attacking = false;
        }
    }
}
