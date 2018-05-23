using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    public float moveSpeed;
    private bool moving;
    private Vector2 lastMove;

    private Rigidbody2D rigidBody;
    private Animator animator;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;
    private Vector3 moveDirection;

    public float timeBetweenDamage;
    private float timeBetweenDamageCounter;
    public float timeBetweenAttack;
    private float timeBetweenAttackCounter;

    public int currentHealth;
    public int maxHealth;

    public int monsterDamage;
    public int swordDamage;

    public GameObject damageNumber;

    public GameObject player;
    public int distanceThreshold;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //timeBetweenMoveCounter = timeBetweenMove;
        //timeToMoveCounter = timeToMove;
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        timeBetweenDamageCounter = Random.Range(0f, timeBetweenDamage * 0.5f);
        timeBetweenAttackCounter = 0f;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
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
        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        animator.SetBool("Moving", moving);
        if (currentHealth <= 0)
            Destroy(gameObject);

        /*if (Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y)) < distanceThreshold)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            print(Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y)));
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }*/
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (collision.gameObject.GetComponent<Player>().attacking)
            {
                if (timeBetweenAttackCounter > 0f)
                    timeBetweenAttackCounter -= Time.deltaTime;
                else
                {
                    currentHealth -= swordDamage;
                    var clone = (GameObject) Instantiate(damageNumber, transform.position, Quaternion.Euler(Vector3.zero));
                    clone.GetComponent<FloatingNumbers>().damageNumber = swordDamage;
                    timeBetweenAttackCounter = timeBetweenAttack;
                }
            }
            else
            {
                if (timeBetweenDamageCounter > 0f)
                    timeBetweenDamageCounter -= Time.deltaTime;
                else
                {
                    collision.gameObject.GetComponent<Player>().currentHealth -= monsterDamage;
                    var clone = (GameObject)Instantiate(damageNumber, collision.gameObject.transform.position, Quaternion.Euler(Vector3.zero));
                    clone.GetComponent<FloatingNumbers>().damageNumber = monsterDamage;
                    timeBetweenDamageCounter = timeBetweenDamage;
                }
            }
            //print("Distance at Collision: " + Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y)));
        }
    }

}
