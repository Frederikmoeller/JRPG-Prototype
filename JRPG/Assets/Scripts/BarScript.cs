using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarScript : MonoBehaviour
{
    public Slider Bar;
    public Unit playerStats;
    private void Start()
    {
        if (gameObject.name.Contains("Health"))
        {
            Bar = GetComponent<Slider>();
            Bar.maxValue = playerStats.baseSetup.MaxHP;
            Bar.value = playerStats.baseSetup.HP; 
        }
        else if (gameObject.name.Contains("Mana"))
        {
            Bar = GetComponent<Slider>();
            Bar.maxValue = playerStats.playerSetup.maxMana;
            Bar.value = playerStats.playerSetup.mana; 
        }

    }
}
