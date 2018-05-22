using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Inventory inventory;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
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

            if (Input.GetMouseButtonDown(0) && worldIndex == -1)
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


        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        animator.SetBool("Moving", moving);
        animator.SetBool("Attacking", attacking);

        if (currentHealth <= 0)
            gameObject.SetActive(false);
    }

	public void changeWorld(){
		currentEnergy -= 5;
        var offset = transform.position.x - worldIndex * worldCenter;
        worldIndex /= -1;
        transform.position = new Vector3(worldIndex * worldCenter + offset, transform.position.y, transform.position.z);
		
	}

}

