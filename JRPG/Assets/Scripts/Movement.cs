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
        RB2D = GetComponent<Rigidbody2D>(); //Gets rigidbody
        _myAnimator = GetComponent<Animator>(); //Gets the animator
        _spriteRenderer = GetComponent<SpriteRenderer>(); //gets the sprite renderer
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        RB2D.velocity = _xDirection*moveSpeed; //Moves the character based on the direction given times the moveSpeed
        //Checks if the x or y velocity is not 0
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
        //Checks which way the character is moving and flips the character appropriately
        if (RB2D.velocity.x > 0)
        {
            _spriteRenderer.flipX = true;
        }
        else if (RB2D.velocity.x < 0)
        {
            _spriteRenderer.flipX = false;
        }
        _myAnimator.SetBool("DoRun", isMoving); //Sets the DoRun attribute in the animator according to the ismoving variable
    }
    //New input system input function.  
    void OnMove(InputValue iv)
    {
        print("pressed");
        _xDirection = iv.Get<Vector2>().normalized; //Gets the direction of the input and normalizes it so the character always moves at the same speed
    }
}
