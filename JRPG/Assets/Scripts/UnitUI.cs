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
     [SerializeField] private Unit unit;
     
     // Start is called before the first frame update
    void Start()
    {
        unit = gameObject.GetComponent<Unit>();
        nameText.text = unit.baseSetup.name; 
        HPText.text = unit.baseSetup.HP + "/" + unit.baseSetup.MaxHP;
        ManaText.text = unit.playerSetup.mana + "/" + unit.playerSetup.maxMana;
    }
}
