using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public GameObject itemToDrop;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerMeleeRange")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            Vector3 moveDirection = player.transform.position - transform.position;
            Vector2 lastMove = new Vector2(moveDirection.x, moveDirection.y);
            if (player.attacking && (Vector2.Angle(player.Facing(), -lastMove) < 50f))
            {
                Destroy(gameObject);
                Instantiate(itemToDrop, transform.position, Quaternion.Euler(Vector3.zero));
            }
        }
    }
}
