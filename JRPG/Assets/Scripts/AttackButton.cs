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
        _battleSystem = GameObject.Find("Battlesystem").GetComponent<BattleSystem>(); //Get the battle system script to use methods from it
    }

    public void OnAttackButton()
    {
        print("Attack!");
        _battleSystem.PlayerAttack(); //When button is pressed do the player attack method
    }
    
    public void OnFleeButton() //This method checks if the player can flee or not
    {
        speedDifference = _battleSystem.SpeedDifferenceCalculation();
        int fleeValue = Random.Range(0, 100 + speedDifference); // Gets a random value between 0 and 100 + the speed difference of the fastest enemy and the player
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
