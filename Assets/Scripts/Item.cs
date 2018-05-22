using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public Sprite sprite;
    public GameObject itemObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Inventory inventory = collision.gameObject.GetComponent<Player>().inventory;
            if (!inventory.IsFull())
            {
                inventory.AddItem(this);
                Destroy(gameObject);
            }
        }
    }
}
