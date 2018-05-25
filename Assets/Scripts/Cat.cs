using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {

    private Animator animator;
    public bool catFree;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        catFree = false;
	}
	
	// Update is called once per frame
	void Update () {
        animator.SetBool("CatFree", catFree);
	}
}
