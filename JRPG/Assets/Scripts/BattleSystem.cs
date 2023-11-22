using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private int charactersDead, enemiesDead, _instanceNumber;
    public int turnIndex;

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
        charactersDead = 0;
        enemiesDead = 0;
        yield return new WaitForSeconds(2f);
        Turn();
    }

    public void Turn()
    {
        if (turnIndex > turnOrder.Count - 1)
        {
            turnIndex = 0;
        }
        
        if (turnOrder[turnIndex].GetComponent<Unit>().baseSetup.isEnemy && turnOrder[turnIndex].GetComponent<Unit>().baseSetup.isDead == false)
        {
            StartCoroutine(EnemyTurn());
        }
        else if (turnOrder[turnIndex].GetComponent<Unit>().baseSetup.isEnemy == false && turnOrder[turnIndex].GetComponent<Unit>().baseSetup.isDead == false)
        {
            state = BattleState.PlayerTurn;
            PlayerTurn(turnOrder[turnIndex]);
        }
        else
        {
            turnIndex++;
            Turn();
        }

    }

    private GameObject ChooseEnemy()
    {
        GameObject chosenEnemy = GameObject.Find("Slime");
        
        return chosenEnemy;
    }

    public void PlayerAttack()
    {
        DamageCalculation(turnOrder[turnIndex], ChooseEnemy());
    }

    public IEnumerator EndOfTurn()
    {
        turnOrder[turnIndex].transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        var UIPos = GameObject.Find(turnOrder[turnIndex].name + "UI").transform.position;
        UIPos = new Vector3(UIPos.x + 120f, UIPos.y);
        GameObject.Find(turnOrder[turnIndex].name + "UI").transform.position = UIPos;
        StartCoroutine(MoveCharacterBack(turnOrder[turnIndex]));
        turnIndex++;
        Turn();
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

            characterUI.GetComponent<UnitUI>().unit = character.GetComponent<Unit>();

            characterUI.GetComponent<UnitUI>().nameText = characterUI.GetComponentInChildren<TextMeshProUGUI>();

            characterUI.GetComponent<UnitUI>().HPText = characterUI.transform.Find("HealthBar").Find("HPLeft").GetComponent<TextMeshProUGUI>();

            characterUI.GetComponent<UnitUI>().ManaText = characterUI.transform.Find("ManaBar").Find("Mana").GetComponent<TextMeshProUGUI>();

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
        print("Attacker is " + attacker + " and victim is " + victim);
        int damage = attacker.GetComponent<Unit>().baseSetup.Attack * attacker.GetComponent<Unit>().weapon.strength / victim.GetComponent<Unit>().baseSetup.Defense;
        damage = Mathf.FloorToInt(damage * Random.Range(0.90f, 1.10f));
        print(damage);
        victim.GetComponent<Unit>().baseSetup.HP -= damage;
        if (victim.GetComponent<Unit>().baseSetup.HP < 0)
        {
            victim.GetComponent<Unit>().baseSetup.HP = 0;
        }
        
        if (victim.GetComponent<Unit>().baseSetup.HP <= 0 && victim.GetComponent<Unit>().baseSetup.isEnemy)
        {
            victim.GetComponent<Unit>().baseSetup.isDead = true;
            enemiesDead++;
            victim.SetActive(false);
        }
        else if (victim.GetComponent<Unit>().baseSetup.HP <= 0 && victim.GetComponent<Unit>().baseSetup.isEnemy == false)
        {
            victim.GetComponent<Unit>().baseSetup.isDead = true;
            charactersDead++;
            victim.SetActive(false);
        }
        
        if (enemiesDead >= enemiesToSpawn.Count)
        {
            state = BattleState.Won;
            WaitFunction(3f);
            SceneManager.LoadScene("Overworld");
        }
        else if (charactersDead >= charactersToSpawn.Count)
        {
            state = BattleState.Lost;
            WaitFunction(3f);
            SceneManager.LoadScene("Overworld");
        }
        else
        {
            StartCoroutine(EndOfTurn());
        }
    }

    private float lerpTime = 0.1f; // Adjust the time as needed

    private IEnumerator MoveCharacter(GameObject selectedCharacter)
    {
        // Store the initial position of the character
        Vector3 initialPosition = selectedCharacter.transform.position;

        // Set the target position
        Vector3 targetPosition = new Vector3(1f, 0f);

        // Interpolate the position over time for forward movement
        float elapsedTime = 0f;
        while (elapsedTime < lerpTime)
        {
            selectedCharacter.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / lerpTime);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the character reaches the exact target position for forward movement
        selectedCharacter.transform.position = targetPosition;
    }
    
    private IEnumerator MoveCharacterBack(GameObject currentCharacter)
    {
        // Store the target position
        Vector3 targetPosition = currentCharacter.GetComponent<Unit>().baseSetup.position;

        // Set the initial position
        Vector3 initialPosition = currentCharacter.transform.position;

        // Interpolate the position over time for backward movement
        float elapsedTime = 0f;
        while (elapsedTime < lerpTime)
        {
            currentCharacter.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / lerpTime);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the character reaches the exact target position for backward movement
        currentCharacter.transform.position = targetPosition;
    }

    private void PlayerTurn(GameObject selectedCharacter)
    {
        StartCoroutine(MoveCharacter(selectedCharacter));
        var UIposition = GameObject.Find(selectedCharacter.name + "UI").transform.position;
        UIposition = new Vector3(UIposition.x - 120f, UIposition.y); 
        GameObject.Find(selectedCharacter.name + "UI").transform.position = UIposition;
        selectedCharacter.transform.GetChild(0).gameObject.SetActive(true);
    }

    private IEnumerator EnemyTurn()
    {
        state = BattleState.Enemyturn;
        var element = turnOrder[Random.Range(0, turnOrder.Count - 1)];
        yield return new WaitForSeconds(2f);
        if (element.GetComponent<Unit>().baseSetup.isEnemy == false && element.GetComponent<Unit>().baseSetup.isDead == false)
        {
            DamageCalculation(turnOrder[turnIndex], element);
            turnIndex++;
            Turn();
        }
        else
        {
            Turn();
        }
    }

    public int SpeedDifferenceCalculation()
    {
        int enemySpeed = 0;

        GameObject enemyChosen = turnOrder[Random.Range(0, turnOrder.Count - 1)];
        if (enemyChosen.GetComponent<Unit>().baseSetup.isEnemy)
        {
            enemySpeed = enemyChosen.GetComponent<Unit>().baseSetup.Speed;
        }
        else
        {
            SpeedDifferenceCalculation();
        }
        
        int speedDifference = turnOrder[turnIndex].GetComponent<Unit>().baseSetup.Speed - enemySpeed;
        return speedDifference;
    }

    public IEnumerator WaitFunction(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
