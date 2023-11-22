using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BarScript : MonoBehaviour
{
    public Slider Bar;
    public Unit playerStats;

    private void Start()
    {
        Bar = GetComponent<Slider>();
    }

    private void Update()
    {

        if (gameObject.name.Contains("Health"))
        {
            Bar.maxValue = playerStats.baseSetup.MaxHP;
            Bar.value = playerStats.baseSetup.HP; 
        }
        else if (gameObject.name.Contains("Mana"))
        {
            Bar.maxValue = playerStats.playerSetup.maxMana;
            Bar.value = playerStats.playerSetup.mana; 
        }

    }
}
