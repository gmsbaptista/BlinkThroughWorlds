﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed;
    private int worldIndex = -1; //-1 is physical, 1 is spirit
    public float worldCenter;
    private Animator animator;
    private bool moving;
    private Vector2 lastMove;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moving = false;
        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0.0f));
            moving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }        
        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        animator.SetBool("Moving", moving);

        if (Input.GetMouseButtonDown(1))
        {
            var offset = transform.position.x - worldIndex * worldCenter;
            worldIndex /= -1;
            transform.position = new Vector3(worldIndex * worldCenter + offset, transform.position.y, transform.position.z);
        }
    }
}
