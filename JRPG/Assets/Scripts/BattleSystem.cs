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
    //Different states the combat can be in
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
    public GameObject hitParticle;
    private Animator _characterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _characterAnimator = GetComponent<Animator>(); //Gets the animator
        SpawnCharacters(); //Spawns characters
        turnOrder.Sort(CompareSpeed); //Sorts the turnOrder list based on the speed of the characters
        StartCoroutine(SetupBattle()); //Runs setup battle
    }
    //This function makes sure everything that needs to be reset is reset and starts the first turn
    IEnumerator SetupBattle()
    {
        turnIndex = 0;
        charactersDead = 0;
        enemiesDead = 0;
        yield return new WaitForSeconds(2f);
        Turn();
    }
    //This function handles what happens if it is an enemy turn or a player turn and also makes sure that the turns loop
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
    //chooses an enemy (Never got around to making the player choose themselves)
    private GameObject ChooseEnemy()
    {
        GameObject chosenEnemy = GameObject.Find("Slime");
        
        return chosenEnemy;
    }
    //Basically just runs the damage calculation of the player and the enemy.
    //The method is used by a button.
    public void PlayerAttack()
    {
        DamageCalculation(turnOrder[turnIndex], ChooseEnemy());
    }
    //Handles what happens at the end of the turn. This includes moving all the UI and character positions back
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
    //Instantiates all player characters and their UI and also spawns the enemies and adds them to the turnOrder list.
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
    //This method compares the speed between characters and is used when sorting the turnOrder.
    private int CompareSpeed(GameObject a, GameObject b)
    {
        int a_speed = a.GetComponent<Unit>().baseSetup.Speed;
        int b_speed = b.GetComponent<Unit>().baseSetup.Speed;

        if (a_speed > b_speed)
        {
            return -1; //Returns -1 to show the sort function that the two gameobjects should change places.
        }
        if (a_speed < b_speed)
        {
            return 1;
        }
        return 0; //Failsafe if the speeds are the same.
    }
    //This method does all the damage calculations and also spawns particles.
    //It also checks if the HP of the target is <0 and sets the HP to 0 if it is.
    //Lastly it also checks if players are dead and enemies are dead and adds to a tally.
    //If that tally is the same or more than the count of the list of player characters or enemies then it should close boot you to the overworld.
    private void DamageCalculation(GameObject attacker, GameObject victim)
    {
        print("Attacker is " + attacker + " and victim is " + victim);
        int damage = attacker.GetComponent<Unit>().baseSetup.Attack * attacker.GetComponent<Unit>().weapon.strength / victim.GetComponent<Unit>().baseSetup.Defense;
        damage = Mathf.FloorToInt(damage * Random.Range(0.90f, 1.10f));
        print(damage);
        Instantiate(hitParticle, victim.transform.position, Quaternion.identity);
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

    private float lerpTime = 0.1f; // Adjusts how fast the player moves to the middle
    //Moves characters to the middle of the screen to indicate it's turn
    private IEnumerator MoveCharacter(GameObject selectedCharacter)
    {
        // Store the initial position of the character
        Vector3 initialPosition = selectedCharacter.transform.position;

        // Set the target position
        Vector3 targetPosition = new Vector3(1f, 0f);
        
        selectedCharacter.GetComponent<Animator>().SetBool("DoRun", true); //Plays the running animation

        // Interpolate the position over time for forward movement
        float elapsedTime = 0f;
        while (elapsedTime < lerpTime) //While loop used since this isn't run in update
        {
            //Lerp(Linear interpolation) used to smooth movement from a to b
            selectedCharacter.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / lerpTime); 
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensures the character reaches the exact target position
        selectedCharacter.transform.position = targetPosition;

        if (selectedCharacter.transform.position == targetPosition)
        {
            selectedCharacter.GetComponent<Animator>().SetBool("DoRun", false); //Stops the running animation and plays idle animation
        }
    }
    //Does the same as MoveCharacter but just backwards
    private IEnumerator MoveCharacterBack(GameObject currentCharacter)
    {
        // Store the target position
        Vector3 targetPosition = currentCharacter.GetComponent<Unit>().baseSetup.position;

        // Set the initial position
        Vector3 initialPosition = currentCharacter.transform.position;
        
        currentCharacter.GetComponent<Animator>().SetBool("DoRun", true);

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
        if (currentCharacter.transform.position == targetPosition)
        {
            currentCharacter.GetComponent<Animator>().SetBool("DoRun", false);
        }
        
    }
    //If the turn is a player turn this function will move the character to the center and the UI will be shifted left to indicate who it is
    private void PlayerTurn(GameObject selectedCharacter)
    {
        StartCoroutine(MoveCharacter(selectedCharacter));
        var UIposition = GameObject.Find(selectedCharacter.name + "UI").transform.position;
        UIposition = new Vector3(UIposition.x - 120f, UIposition.y); 
        GameObject.Find(selectedCharacter.name + "UI").transform.position = UIposition;
        selectedCharacter.transform.GetChild(0).gameObject.SetActive(true);
    }
    //If the turn is a enemy turn it will choose a random gameobject and check if it is a player.
    private IEnumerator EnemyTurn()
    {
        state = BattleState.Enemyturn;
        var element = turnOrder[Random.Range(0, turnOrder.Count)];
        if (element.GetComponent<Unit>().baseSetup.isEnemy == false && element.GetComponent<Unit>().baseSetup.isDead == false) //If it was a player run damage calculation turn
        {
            yield return new WaitForSeconds(1f);
            DamageCalculation(turnOrder[turnIndex], element);
            yield return new WaitForSeconds(1f);
            turnIndex++;
            Turn();
        }
        else //If it wasn't a player then run this function again
        {
            StartCoroutine(EnemyTurn());
        }
    }
    //This method gets the speed difference when the player tries to escape
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
    //Waitfunction made to run when function can't be an IEnmuerator but still needs to wait.
    public IEnumerator WaitFunction(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
