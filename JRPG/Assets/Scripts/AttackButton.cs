using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    [SerializeField] private BattleSystem _battleSystem;

    private void Awake()
    {
        print("Awake");
        _battleSystem = GameObject.Find("Battlesystem").GetComponent<BattleSystem>();
    }

    public void OnAttackButton()
    {
        print("Attack!");
        StartCoroutine(_battleSystem.PlayerAttack());
    }
}
