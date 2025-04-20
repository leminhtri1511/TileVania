using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider2D;

    [SerializeField] float runSpeed = 2f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float climbSpeed = 5f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        Run();
        ClimbLadder();
        FlipSprite();
    }


    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void ClimbLadder()
    {
        var isTouchingLadder = myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (!isTouchingLadder)
            return;

        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
    }

    // void ClimbLadder()
    // {
    //     var isTouchingLadder = myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));

    //     if (isTouchingLadder)
    //     {
    //         if (Input.GetKey(KeyCode.W))
    //         {
    //             var changeYVelocity = new Vector2(0f, climbSpeed);
    //             myRigidbody.velocity = changeYVelocity;
    //             Debug.Log(changeYVelocity);

    //         }
    //         else if (Input.GetKey(KeyCode.S))
    //         {
    //             var changeYVelocity = new Vector2(0f, -climbSpeed);
    //             myRigidbody.velocity = changeYVelocity;
    //             Debug.Log(changeYVelocity);
    //         }
    //         else if (!Input.anyKey)
    //         {
    //             var changeYVelocity = new Vector2(0, 0);
    //             myRigidbody.velocity = changeYVelocity;
    //             myRigidbody.gravityScale = 0;


    //             Debug.Log($"changeYVelocity: {changeYVelocity}");
    //         }
    //     }
    //     else myRigidbody.gravityScale = 8;
    // }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void OnMove(InputValue inputValue)
    {
        moveInput = inputValue.Get<Vector2>();
        // Debug.Log($"X: {myRigidbody.velocity.x}");
    }

    void OnJump(InputValue inputValue)
    {
        var isTouchingGround = myCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (!isTouchingGround)
        {
            return;
        }
        else if (inputValue.isPressed && isTouchingGround)
        {
            myRigidbody.velocity = new Vector2(0f, jumpHeight);
        }
    }

}
