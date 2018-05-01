﻿using System.Collections;
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

    public GameObject damageNumber;

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
                    currentHealth -= 1;
                    var clone = (GameObject) Instantiate(damageNumber, transform.position, Quaternion.Euler(Vector3.zero));
                    clone.GetComponent<FloatingNumbers>().damageNumber = 1;
                    timeBetweenAttackCounter = timeBetweenAttack;
                }
            }
            else
            {
                if (timeBetweenDamageCounter > 0f)
                    timeBetweenDamageCounter -= Time.deltaTime;
                else
                {
                    collision.gameObject.GetComponent<Player>().currentHealth -= 10;
                    var clone = (GameObject)Instantiate(damageNumber, collision.gameObject.transform.position, Quaternion.Euler(Vector3.zero));
                    clone.GetComponent<FloatingNumbers>().damageNumber = 10;
                    timeBetweenDamageCounter = timeBetweenDamage;
                }
            }
        }
    }
}
