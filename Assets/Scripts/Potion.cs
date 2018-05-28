using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {

    private int restore;
    private int baseHealthRestore = 5;
    private int baseEnergyRestore = 5;

    // Use this for initialization
    void Start()
    {
        if (gameObject.tag == "HealthPotion")
        {
            /*float preRestore = Random.Range(baseHealthRestore * 0.5f, baseHealthRestore * 1.5f);
            transform.localScale = new Vector3(preRestore / baseHealthRestore, preRestore / baseHealthRestore, 1f);
            restore = Mathf.RoundToInt(preRestore);*/
            restore = baseHealthRestore;
        }
        else if (gameObject.tag == "EnergyPotion")
        {
            /*float preRestore = Random.Range(baseEnergyRestore * 0.5f, baseEnergyRestore * 1.5f);
            transform.localScale = new Vector3(preRestore / baseEnergyRestore, preRestore / baseEnergyRestore, 1f);
            restore = Mathf.RoundToInt(preRestore);*/
            restore = baseEnergyRestore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (gameObject.tag == "HealthPotion")
            {
                if (player.currentHealth < player.maxHealth)
                {
                    player.currentHealth += restore;
                    if (player.currentHealth > player.maxHealth)
                    {
                        player.currentHealth = player.maxHealth;
                    }
                    Destroy(gameObject);
                }
            }
            else if (gameObject.tag == "EnergyPotion")
            {
                if (player.currentEnergy < player.maxEnergy)
                {
                    player.currentEnergy += restore;
                    if (player.currentEnergy > player.maxEnergy)
                    {
                        player.currentEnergy = player.maxEnergy;
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}
