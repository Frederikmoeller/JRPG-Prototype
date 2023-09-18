using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    public enum BattleState
    {
        Start,
        Playerturn,
        Enemyturn,
        Lost,
        Won,
        Escape,
    }

    public BattleState state;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        state = BattleState.Playerturn;
        yield return new WaitForSeconds(2f);
        PlayerTurn();
    }

    void PlayerTurn()
    {
        
    }

    IEnumerator PlayerAttack()
    {
        print("wait for...");
        // Damage the enemy
        yield return new WaitForSeconds(2f);
        print("it!");
        // Check if enemy is dead
        // Change state based on what happened
    }

    public void OnAttackButton()
    {
        if (state != BattleState.Playerturn)
            return;

        StartCoroutine(PlayerAttack());
    }
}
