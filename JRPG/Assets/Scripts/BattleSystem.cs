using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private bool currentActor;
    public List<GameObject> turnOrder = new();
    public List<GameObject> charactersToSpawn;
    private int _instanceNumber;
    [SerializeField] private Transform _canvas;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCharacters();

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

    void SpawnCharacters()
    {
        for (int i = 0; i < charactersToSpawn.Count; i++)
        {
            GameObject character = Instantiate(charactersToSpawn[i], charactersToSpawn[i].GetComponent<Player>().playerSetup.spawnPoints[0],
                quaternion.identity);

            character.name = charactersToSpawn[i].GetComponent<Player>().playerSetup.CharacterName;
            
            GameObject characterUI = Instantiate(charactersToSpawn[i].GetComponent<Player>().UI,
                charactersToSpawn[i].GetComponent<Player>().UI.transform.position, quaternion.identity);
            
            characterUI.name = charactersToSpawn[i].GetComponent<Player>().playerSetup.CharacterName + "UI";

            characterUI.transform.SetParent(_canvas, false);

            var position = characterUI.transform.position;
            
            position = new Vector3(position.x, position.y - (100*i));
            characterUI.transform.position = position;

            character.GetComponent<Player>().nameText = characterUI.GetComponentInChildren<TextMeshProUGUI>();

            character.GetComponent<Player>().HP = characterUI.transform.GetChild(2).Find("HPLeft").GetComponent<TextMeshProUGUI>();

            character.GetComponent<Player>().Mana = characterUI.transform.GetChild(3).Find("Mana").GetComponent<TextMeshProUGUI>();
            
            //print(characterUI.GetComponent<Player>().UI.GetComponent<BarScript>().playerStats);

            character.GetComponent<Player>().UI.transform.Find("HealthBar").GetComponent<BarScript>().playerStats = character.GetComponent<Player>().playerSetup;

            character.GetComponent<Player>().UI.transform.Find("ManaBar").GetComponent<BarScript>().playerStats = character.GetComponent<Player>().playerSetup;
        }
    }
}
