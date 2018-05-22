using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Slider healthBar;
    public Text hpText;
    public Slider energyBar;
    public Text energyText;
    public Player player;
    public GameObject inventory;


	// Use this for initialization
	void Start () {
        inventory.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        healthBar.maxValue = player.maxHealth;
        healthBar.value = player.currentHealth;
        hpText.text = "HP: " + player.currentHealth + "/" + player.maxHealth;
        energyBar.maxValue = player.maxEnergy;
        energyBar.value = player.currentEnergy;
        energyText.text = "Energy: " + player.currentEnergy + "/" + player.maxEnergy;
        if(Input.GetKeyDown("i"))
        {
            inventory.SetActive(!inventory.activeSelf);
        }
    }
}
