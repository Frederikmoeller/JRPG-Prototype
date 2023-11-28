using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    //These are setup to make different characters
    public BaseUnit baseSetup;
    public PlayerUnit playerSetup;
    public EnemyUnit enemySetup;
    public SpriteRenderer _spriteRenderer;
    public Weapon weapon;


    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer.sprite = baseSetup.sprite; //sets the sprite based on the baseSetup (not really used when animator is used)
    }
}
