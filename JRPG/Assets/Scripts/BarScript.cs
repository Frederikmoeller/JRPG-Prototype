using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarScript : MonoBehaviour
{
    public Slider healthBar;
    public PlayerStats playerStats;
    private void Start()
    {
        if (gameObject.name.Contains("Health"))
        {
            healthBar = GetComponent<Slider>();
            healthBar.maxValue = playerStats.MaxHP;
            healthBar.value = playerStats.HP; 
        }
        else if (gameObject.name.Contains("Mana"))
        {
            healthBar = GetComponent<Slider>();
            healthBar.maxValue = playerStats.MaxMana;
            healthBar.value = playerStats.Mana; 
        }

    }
}
