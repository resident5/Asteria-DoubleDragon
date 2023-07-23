using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public CharacterMovement characterMovement;
    public CharacterAttack characterAttack;
    public Player1Inputs input = null;
    private Vector2 moveVector = Vector2.zero;
    public Animator animator;

    public bool running = false;

    public float doubleTapTimeDelay = 0.8f;
    public float timeSinceLastTap;
    public bool doubleTapped;

    float currentTime = 0;

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterAttack = GetComponent<CharacterAttack>();
        input = new Player1Inputs();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCancelled;
        input.Player.PAttack.performed += OnPAttackPerformed;
        input.Player.PAttack.canceled += OnPAttackPerformed;
        //input.Player.DoubleTap.performed += OnDoubleTapPerformed;
        //input.Player.DoubleTap.canceled += OnDoubleTapCancelled;
        input.Player.HoldTap.performed += OnHoldPerformed;
        input.Player.HoldTap.canceled += OnHoldCancelled;

    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCancelled;
        input.Player.PAttack.performed -= OnPAttackPerformed;
        input.Player.PAttack.canceled -= OnPAttackPerformed;
        //input.Player.DoubleTap.performed -= OnDoubleTapPerformed;
        //input.Player.DoubleTap.canceled -= OnDoubleTapCancelled;
        input.Player.HoldTap.performed -= OnHoldPerformed;
        input.Player.HoldTap.canceled -= OnHoldCancelled;


    }

    private void FixedUpdate()
    {
        characterMovement.Move(moveVector.x, moveVector.y, running);
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (input != null)
        {
            characterMovement.jump = true;
            animator.SetBool("jump",true);
        }
    }

    private void OnJumpCancelled(InputAction.CallbackContext value)
    {
        characterMovement.jump = false;
        animator.SetBool("jump", false);
    }

    void OnDoubleTapPerformed(InputAction.CallbackContext context)
    {
        if (input != null)
        {
            Debug.Log("Tapped");
        }
    }

    void OnDoubleTapCancelled(InputAction.CallbackContext context)
    {
        Debug.Log("Untapped");


    }

    void OnHoldPerformed(InputAction.CallbackContext context)
    {
        if (input != null)
        {
            currentTime = Time.time;
            if (currentTime - timeSinceLastTap <= doubleTapTimeDelay)
            {
                doubleTapped = true;
                running = true;
                animator.SetBool("run", true);
                animator.SetBool("move", false);
            }

            timeSinceLastTap = currentTime;

            //if (doubleTapped)
            //{
            //    Debug.Log("Holding");

            //    running = true;
            //}
        }
    }

    void OnHoldCancelled(InputAction.CallbackContext context)
    {
        running = false;
        doubleTapped = false;
        animator.SetBool("run", false);

    }

    public void OnMovementPerformed(InputAction.CallbackContext value)
    {
        if (input != null)
        {
            moveVector = value.ReadValue<Vector2>();
            animator.SetBool("move", true);
        }
    }

    void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
        animator.SetBool("move", false);
    }

    void OnPAttackPerformed(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            characterAttack.Attack();
        }
    }

    void OnPAttackCancelled(InputAction.CallbackContext value)
    {

    }


}
