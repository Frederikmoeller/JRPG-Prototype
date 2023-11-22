using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
     public TextMeshProUGUI nameText;
     public TextMeshProUGUI HPText;
     public TextMeshProUGUI ManaText;
     public GameObject UI;
     public Unit unit;

     void Update()
    {
        nameText.text = unit.baseSetup.name;
        HPText.text = unit.baseSetup.HP + "/" + unit.baseSetup.MaxHP;
        ManaText.text = unit.playerSetup.mana + "/" + unit.playerSetup.maxMana;
    }
}
