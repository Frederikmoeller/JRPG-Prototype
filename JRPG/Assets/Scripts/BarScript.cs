using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public PlayerStats playerHealth;
    private void Start()
    {
        if (gameObject.name.Contains("Health"))
        {
            healthBar = GetComponent<Slider>();
            healthBar.maxValue = playerHealth.MaxHP;
            healthBar.value = playerHealth.HP; 
        }
        else if (gameObject.name.Contains("Mana"))
        {
            healthBar = GetComponent<Slider>();
            healthBar.maxValue = playerHealth.MaxMana;
            healthBar.value = playerHealth.Mana; 
        }

    }
}
