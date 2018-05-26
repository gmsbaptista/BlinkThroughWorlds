using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public Item key;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (collision.gameObject.GetComponent<Player>().inventory.ItemInInventory(key))
            {
                collision.gameObject.GetComponent<Player>().inventory.RemoveItem(key);
                Destroy(gameObject);
            }
        }
    }
}
