using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleSystem : MonoBehaviour
{
    public enum BattleState
    {
        Start,
        PlayerTurn,
        Enemyturn,
        Lost,
        Won,
        Escape,
    }

    public BattleState state;
    public List<GameObject> turnOrder = new();
    public List<GameObject> charactersToSpawn, enemiesToSpawn;
    [SerializeField] private Transform _canvas;
    private GameObject enemy, character, characterUI;
    private Vector2 UIPos;
    private int turnIndex, enemiesDead, _instanceNumber;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCharacters();
        turnOrder.Sort(CompareSpeed);
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        turnIndex = 0;
        yield return new WaitForSeconds(2f);
        Turn();
    }

    void Turn()
    {
        print(turnOrder[turnIndex]);
        if (turnOrder[turnIndex].GetComponent<Unit>().baseSetup.isEnemy)
        {
            print("Enemy Attack!");
            state = BattleState.Enemyturn;
            var element = charactersToSpawn[Random.Range(0, charactersToSpawn.Count)];
            DamageCalculation(turnOrder[turnIndex], element);
            turnIndex++;
        }
        else
        {
            state = BattleState.PlayerTurn;
            PlayerTurn(turnOrder[turnIndex]);
            turnIndex++;
        }        
        if (turnIndex > turnOrder.Count - 1)
        {
            turnIndex = 0;
        }
    }

    private GameObject ChooseEnemy()
    {
        GameObject chosenEnemy = GameObject.Find("Slime");
        
        return chosenEnemy;
    }

    public IEnumerator PlayerAttack()
    {
        DamageCalculation(turnOrder[turnIndex], ChooseEnemy());
        
        yield return new WaitForSeconds(2f);
        if (ChooseEnemy().GetComponent<Unit>().baseSetup.HP <= 0)
        {
            enemiesDead++;
        }
        
        if (enemiesDead >= enemiesToSpawn.Count)
        {
            state = BattleState.Won;
        }
        // Change state based on what happened
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
            enemy = Instantiate(enemiesToSpawn[i], enemiesToSpawn[i].GetComponent<Unit>().baseSetup.position,
                quaternion.identity);

            enemy.name = enemiesToSpawn[i].GetComponent<Unit>().baseSetup.name;
            turnOrder.Add(enemy);
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

    private void DamageCalculation(GameObject attacker, GameObject victim)
    {
        int damage = attacker.GetComponent<Unit>().baseSetup.Attack / victim.GetComponent<Unit>().baseSetup.Defense;
        victim.GetComponent<Unit>().baseSetup.HP -= damage;
    }

    private void PlayerTurn(GameObject selectedCharacter)
    {
        selectedCharacter.transform.position = Vector3.Lerp(selectedCharacter.transform.position, new Vector3(1f, 0f), 10f);
        var UIposition = GameObject.Find(selectedCharacter.name + "UI").transform.position;
        UIposition = new Vector3(UIposition.x - 120f, UIposition.y);
        GameObject.Find(selectedCharacter.name + "UI").transform.position = UIposition;
        selectedCharacter.transform.GetChild(0).gameObject.SetActive(true);
        
    }
}
