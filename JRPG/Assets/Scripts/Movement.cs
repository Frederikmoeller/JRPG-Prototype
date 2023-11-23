using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    private Vector2 _xDirection;
    public float moveSpeed = 6;
    private PlayerInput _playerInput;
    private Rigidbody2D RB2D;
    [SerializeField] private bool isMoving;
    private Animator _myAnimator;
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        RB2D = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RB2D.velocity = _xDirection*moveSpeed;
        //StartCoroutine(RandomEncounter());
        if (RB2D.velocity.x != 0f)
        {
            isMoving = true;
        }
        else if (RB2D.velocity.y != 0f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        if (RB2D.velocity.x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (RB2D.velocity.x < 0)
        {
            _spriteRenderer.flipX = false;
        }
        _myAnimator.SetBool("DoRun", isMoving);
    }

    void OnMove(InputValue iv)
    {
        print("pressed");
        _xDirection = iv.Get<Vector2>().normalized;
    }

    IEnumerator RandomEncounter()
    {
        yield return new WaitForSeconds(3f);
        if (isMoving == false) yield break;
        int chanceForEncounter = Random.Range(0, 100);
        if (chanceForEncounter < 50)
        {
            SceneManager.LoadScene("Combat");
        }
    }
}
