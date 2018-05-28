using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : Item {

    private Animator animator;
    public bool catFree;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        //catFree = false;
	}
	
	// Update is called once per frame
	void Update () {
        animator.SetBool("CatFree", catFree);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && catFree)
        {
            Inventory inventory = collision.gameObject.GetComponent<Player>().inventory;
            if (!inventory.IsFull())
            {
                inventory.AddItem(this);
                Destroy(gameObject);
            }
        }
    }

    public void FreeCat()
    {
        catFree = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
