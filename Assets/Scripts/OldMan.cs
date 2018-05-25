using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldMan : MonoBehaviour {

    public int maxHealth;
    public int currentHealth;

    public Cat cat;

    private float timeBetweenAttack;
    private float timeBetweenAttackCounter = 0f;
    public GameObject damageNumber;

    // Use this for initialization
    void Start ()
    {
        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (currentHealth <= 0)
        {
            cat.catFree = true;
            Destroy(gameObject);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerTrigger")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            Vector3 moveDirection = player.transform.position - transform.position;
            Vector2 lastMove = new Vector2(moveDirection.x, moveDirection.y);
            timeBetweenAttack = player.attackTime;
            if (player.attacking && (Vector2.Angle(player.Facing(), -lastMove) < 50f))
            {
                if (timeBetweenAttackCounter > 0f)
                {
                    timeBetweenAttackCounter -= Time.deltaTime;
                }
                else
                {
                    currentHealth -= player.swordDamage;
                    var clone = (GameObject)Instantiate(damageNumber, transform.position, Quaternion.Euler(Vector3.zero));
                    clone.GetComponent<FloatingNumbers>().damageNumber = player.swordDamage;
                    timeBetweenAttackCounter = timeBetweenAttack;
                }
            }
        }
    }
}
