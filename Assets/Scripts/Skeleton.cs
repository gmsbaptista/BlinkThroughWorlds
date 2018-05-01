using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour {

    public float moveSpeed;

    private Rigidbody2D rigidBody;

    private bool moving;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;
    private Vector3 moveDirection;

    public float timeBetweenDamage;
    private float timeBetweenDamageCounter;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        //timeBetweenMoveCounter = timeBetweenMove;
        //timeToMoveCounter = timeToMove;
        timeBetweenMoveCounter = Random.Range(timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
        timeToMoveCounter = Random.Range(timeToMove * 0.75f, timeToMove * 1.25f);
        timeBetweenDamageCounter = Random.Range(0f, timeBetweenDamage * 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
            rigidBody.velocity = moveDirection;
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
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (collision.gameObject.GetComponent<Player>().attacking)
                Destroy(gameObject);
            else
            {
                if (timeBetweenDamageCounter > 0f)
                    timeBetweenDamageCounter -= Time.deltaTime;
                else
                {
                    collision.gameObject.GetComponent<Player>().currentHealth -= 10;
                    timeBetweenDamageCounter = timeBetweenDamage;
                }
            }
        }
    }
}
