using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 2f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathReact = new Vector2(10f, 20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    SpriteRenderer spriteRenderer;

    bool isAlive = true;
    float startingGravityScale;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startingGravityScale = myRigidbody.gravityScale;
    }

    void Update()
    {

        if (!isAlive)
        {
            return;
        }
        Run();
        ClimbLadder();
        FlipSprite();
        Die();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        bool isPlayerRunning = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        myRigidbody.velocity = playerVelocity;
        myAnimator.SetBool("isRunning", isPlayerRunning);
    }

    void ClimbLadder()
    {
        if (!isAlive)
        {
            return;
        }
        var isTouchingLadder = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        bool isPlayerClimbing = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;

        if (isTouchingLadder)
        {
            myRigidbody.gravityScale = 0;

            Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
            myRigidbody.velocity = climbVelocity;

            myAnimator.SetBool("isClimbing", isPlayerClimbing);
        }
        else
        {
            myAnimator.SetBool("isClimbing", false);
            myRigidbody.gravityScale = startingGravityScale;
            return;
        }
    }

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
        if (!isAlive)
        {
            return;
        }
        if (isAlive)
        {
            moveInput = inputValue.Get<Vector2>();
        }
        else
            return;
    }

    void OnJump(InputValue inputValue)
    {
        if (!isAlive)
        {
            return;
        }
        var isTouchingGround = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (inputValue.isPressed && isTouchingGround)
        {
            myRigidbody.velocity = new Vector2(0f, jumpHeight);
        }
        else
            return;
    }

    void OnFire(InputValue inputValue)
    {
        if (!isAlive)
        {
            return;
        }

        Instantiate(bullet, gun.position, transform.rotation);
    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody.velocity = deathReact;
            spriteRenderer.color = new Color(1, 0, 0, 1);
        }
    }

}
