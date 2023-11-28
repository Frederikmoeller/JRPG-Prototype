using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    //Sets up the UI for the characters.
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI ManaText;
    public GameObject UI;
    public Unit unit;

     void Update()
    {
        nameText.text = unit.baseSetup.name; //sets the correct name on the UI
        HPText.text = unit.baseSetup.HP + "/" + unit.baseSetup.MaxHP; //Sets the correct HP on the UI
        ManaText.text = unit.playerSetup.mana + "/" + unit.playerSetup.maxMana; //Sets the correct Mana on the UI
    }
}
