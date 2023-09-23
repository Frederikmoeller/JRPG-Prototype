using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public enum BattleState
    {
        Start,
        Combat,
        Lost,
        Won,
        Escape,
    }

    public BattleState state;
    public List<GameObject> turnOrder = new();
    public List<GameObject> charactersToSpawn;
    public List<GameObject> enemiesToSpawn;
    private int _instanceNumber;
    private Transform _canvas;
    private GameObject character;
    private GameObject characterUI;
    private Vector2 UIPos;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCharacters();
        turnOrder.Sort(CompareSpeed);
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        state = BattleState.Combat;
        yield return new WaitForSeconds(2f);
        Turn();
    }

    void Turn()
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

        StartCoroutine(PlayerAttack());
    }

    void SpawnCharacters()
    {
        for (int i = 0; i < charactersToSpawn.Count; i++)
        {
            character = Instantiate(charactersToSpawn[i], charactersToSpawn[i].GetComponent<Unit>().baseSetup.position,
                quaternion.identity);

            character.name = charactersToSpawn[i].GetComponent<Unit>().baseSetup.name;
            
            characterUI = Instantiate(charactersToSpawn[i].GetComponent<UnitUI>().UI,
                charactersToSpawn[i].GetComponent<UnitUI>().UI.transform.position, quaternion.identity);
            
            characterUI.name = charactersToSpawn[i].GetComponent<Unit>().baseSetup.name + "UI";

            characterUI.transform.SetParent(_canvas, false);

            UIPos = characterUI.transform.position;
            
            UIPos = new Vector2(UIPos.x, UIPos.y - (150*i));
            characterUI.transform.position = UIPos;

            character.GetComponent<UnitUI>().nameText = characterUI.GetComponentInChildren<TextMeshProUGUI>();

            character.GetComponent<UnitUI>().HPText = characterUI.transform.Find("HealthBar").Find("HPLeft").GetComponent<TextMeshProUGUI>();

            character.GetComponent<UnitUI>().ManaText = characterUI.transform.Find("ManaBar").Find("Mana").GetComponent<TextMeshProUGUI>();

            characterUI.transform.Find("HealthBar").GetComponent<BarScript>().playerStats = character.GetComponent<Unit>();

            characterUI.transform.Find("ManaBar").GetComponent<BarScript>().playerStats = character.GetComponent<Unit>();
            
            turnOrder.Add(character);
        }

        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            character = Instantiate(enemiesToSpawn[i], enemiesToSpawn[i].GetComponent<Unit>().baseSetup.position,
                quaternion.identity);

            character.name = enemiesToSpawn[i].GetComponent<Unit>().baseSetup.name;
            turnOrder.Add(character);
        }

    }

    private int CompareSpeed(GameObject a, GameObject b)
    {
        int a_speed = a.GetComponent<Unit>().baseSetup.Speed;
        int b_speed = b.GetComponent<Unit>().baseSetup.Speed;

        if (a_speed > b_speed)
        {
            return -1;
        }
        if (a_speed < b_speed)
        {
            return 1;
        }
        
        return 0;
    }
}
