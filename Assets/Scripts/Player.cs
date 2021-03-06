﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public float moveSpeed;
    private bool moving;
    private Vector2 lastMove;

    public bool attacking;
    public float attackTime;
    private float attackTimeCounter;

    private Animator animator;
    private Rigidbody2D rigidBody;

    private int worldIndex = -1; //-1 is physical, 1 is spirit
    public float worldCenter;

    public int maxHealth;
    public int currentHealth;
    public int maxEnergy;
    public int currentEnergy;

    public int swordDamage;
    public int switchCost;

    public bool inCombat = false;
    public bool switchedOnce = false;
    public int switchCounter = 0;

    public Inventory inventory;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        //currentEnergy = maxEnergy;
        currentEnergy = 8;
    }

    // Update is called once per frame
    void Update()
    {
        moving = false;

        if (!attacking)
        {
            if (Input.GetAxisRaw("Horizontal") != 0f)
            {
                //transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0.0f, 0.0f));
                rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rigidBody.velocity.y);
                moving = true;
                lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            }
            if (Input.GetAxisRaw("Vertical") != 0f)
            {
                //transform.Translate(new Vector3(0.0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0.0f));
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
                moving = true;
                lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
            }
            if (Input.GetAxisRaw("Horizontal") == 0f)
            {
                rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
            }
            if (Input.GetAxisRaw("Vertical") == 0f)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
            }

            if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && worldIndex == -1)
            {
                attackTimeCounter = attackTime;
                attacking = true;
                rigidBody.velocity = Vector2.zero;
            }
        }
        if (attackTimeCounter > 0)
            attackTimeCounter -= Time.deltaTime;
        else
            attacking = false;


        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        animator.SetBool("Moving", moving);
        animator.SetBool("Attacking", attacking);

        if (currentHealth <= 0)
        {
            #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
            //gameObject.SetActive(false);
        }
    }

	public void ChangeWorld()
    {
		currentEnergy -= switchCost;
        var offset = transform.position.x - worldIndex * worldCenter;
        worldIndex /= -1;
        transform.position = new Vector3(worldIndex * worldCenter + offset, transform.position.y, transform.position.z);
	}

    public Vector2 Facing()
    {
        return lastMove;
    }

    public bool CanSwitch()
    {
        //(player.transform.position.x < 39f || player.transform.position.x > 52f)
        //x - 70 ~ 93, y - -30~-40
        //x - -86~-107, y - 16~-40
        Vector3 pos = this.gameObject.transform.position;
        return (pos.x < 70f || pos.x > 93f) && (pos.x > -87f || pos.x < -107f || pos.y > 16f);
        //return true;
    }

}

