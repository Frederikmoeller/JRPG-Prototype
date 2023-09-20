using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy", menuName = "Enemy")]
public class EnemyStats : ScriptableObject
{
    public string enemyName;
    public Sprite enemySprite;

    public int Attack;
    public int Defense;
    public int HP;
    public int MaxHP;
    public int Intellect;
    public int Speed;
    public int CritChance;
    public int CritDamage;
    public GameObject charGO;
}
