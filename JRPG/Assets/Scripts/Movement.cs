using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Vector2 _xDirection;
    public float moveSpeed = 6;
    private PlayerInput _playerInput;
    private Rigidbody2D RB2D;

    // Start is called before the first frame update
    void Start()
    {
        RB2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RB2D.velocity = _xDirection*moveSpeed;
    }

    void OnMove(InputValue iv)
    {
        print("pressed");
        _xDirection = iv.Get<Vector2>().normalized;
    }
}
