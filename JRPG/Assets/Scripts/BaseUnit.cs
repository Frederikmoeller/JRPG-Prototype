using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BaseUnit //Base stats all Units need
{   
    public string name;
    public Sprite sprite;
    public bool isEnemy = true;
    public bool isDead;
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
public class PlayerUnit //Player stats only players need
{
    public int maxMana;
    public int mana;
    public int expNeeded;
    public int currentExp;
}
[System.Serializable]
public class EnemyUnit //stats only enemies need
{
    public int expDrop;
    //public List<Items> ItemDrop;
}
[System.Serializable]
public class Weapon //Weapon class to make a character wield a weapon
{
    public string name;
    public int strength;
    public int magicStrength;
}
