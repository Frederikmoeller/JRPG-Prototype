using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BaseUnit
{   
    public string name;
    public Sprite sprite;
    public bool isEnemy = true;
    public int Attack;
    public int Defense;
    public int HP;
    public int MaxHP;
    public int Intellect;
    public int Speed;
    public float CritChance;
    public float CritDamage;
    public GameObject charGO;
    public Vector2 position;
    public int level;
}

[System.Serializable]
public class PlayerUnit
{
    public int maxMana;
    public int mana;
    public int expNeeded;
    public int currentExp;
}
[System.Serializable]
public class EnemyUnit
{
    public int expDrop;
    //public List<Items> ItemDrop;
}
