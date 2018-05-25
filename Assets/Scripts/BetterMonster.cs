using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterMonster : MonoBehaviour {

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

    public int currentHealth;
    public int maxHealth;

    public int monsterDamage;

    public GameObject damageNumber;

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
	void Update () {
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

        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        animator.SetBool("Moving", moving);
        animator.SetBool("Attacking", attacking);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerTrigger")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            wanderMode = false;
            moving = false;
            moveDirection = player.transform.position - transform.position;
            lastMove = new Vector2(moveDirection.x, moveDirection.y);
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
            //else
            //{
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

            //}
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerTrigger")
        {
            wanderMode = true;
            attacking = false;
        }
    }
}
