using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New character", menuName = "Characters")]
public class PlayerStats : ScriptableObject
{
    public string CharacterName;
    public Sprite sprite;
    public enum Class
    {
        Warrior,
        Cleric,
        Mage
    }

    public int Attack;
    public int Defense;
    public int HP;
    public int MaxHP;
    public int Mana;
    public int MaxMana;
    public int Intellect;
    public int Speed;
    public int CritChance;
    public int CritDamage;
}
