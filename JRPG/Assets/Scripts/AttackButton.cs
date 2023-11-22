using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AttackButton : MonoBehaviour
{
    [SerializeField] private BattleSystem _battleSystem;
    private int speedDifference;

    private void Awake()
    {
        print("Awake");
        _battleSystem = GameObject.Find("Battlesystem").GetComponent<BattleSystem>();
    }

    public void OnAttackButton()
    {
        print("Attack!");
        _battleSystem.PlayerAttack();
    }
    
    public void OnFleeButton()
    {
        speedDifference = _battleSystem.SpeedDifferenceCalculation();
        int fleeValue = Random.Range(0, 100 + speedDifference);
        print(fleeValue);
        if (fleeValue < 50)
        {
            print("Can't escape!");
            StartCoroutine(_battleSystem.EndOfTurn());
        }
        else
        {
            print("Got Away!");
            StartCoroutine(_battleSystem.WaitFunction(2f));
            SceneManager.LoadScene("Overworld");
        }
    }
}
