using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public CharacterMovement characterMovement;
    public Player1Inputs input = null;
    private Vector2 moveVector = Vector2.zero;
    public Animator animator;


    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        input = new Player1Inputs();
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCancelled;
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
    }

    private void OnJumpCancelled(InputAction.CallbackContext obj)
    {
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Jump.canceled -= OnJumpCancelled;
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        characterMovement.Move(moveVector.x, moveVector.y, false);
        Debug.Log(moveVector);
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
}
