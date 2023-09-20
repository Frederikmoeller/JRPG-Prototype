using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerStats playerSetup;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI HP;
    public TextMeshProUGUI Mana;
    public SpriteRenderer _spriteRenderer;
    public GameObject UI;
    

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer.sprite = playerSetup.sprite;
        nameText.text = playerSetup.CharacterName; 
        HP.text = playerSetup.HP.ToString() + "/" + playerSetup.MaxHP.ToString();
        Mana.text = playerSetup.Mana + "/" + playerSetup.MaxMana;
    }
}
