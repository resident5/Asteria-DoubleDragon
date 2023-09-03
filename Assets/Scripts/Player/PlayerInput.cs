using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour
{
    [FormerlySerializedAs("characterMovement")]
    public CharacterMovement player;

    public CharacterAttack characterAttack;
    public Player1Inputs input = null;
    private Vector2 moveVector = Vector2.zero;

    public bool running = false;

    public float doubleTapTimeDelay = 0.8f;
    public float timeSinceLastTap;
    public bool doubleTapped;

    float currentTime = 0;

    private void Awake()
    {
        player = GetComponent<CharacterMovement>();
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

        input.Player.Struggle.performed += OnStrugglePerformed;
        input.Player.Struggle.canceled += OnStruggleCancelled;

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
        input.Player.Struggle.performed -= OnStrugglePerformed;
        input.Player.Struggle.canceled -= OnStruggleCancelled;

    }

    private void FixedUpdate()
    {
        player.Move(moveVector.x, moveVector.y, running);
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (input != null)
        {
            player.jump = true;
            player.animator.SetBool("isJumping", true);
        }
    }

    private void OnJumpCancelled(InputAction.CallbackContext value)
    {
        //player.jump = false;
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
                //characterMovement.animator.SetBool("run", true);
                //characterMovement.animator.SetBool("move", false);
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
        player.animator.SetBool("run", false);
    }

    public void OnMovementPerformed(InputAction.CallbackContext value)
    {
        if (input != null)
        {
            moveVector = value.ReadValue<Vector2>();
        }
    }

    void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
        player.animator.SetBool("move", false);
        player.animator.SetFloat("speedX", 0);
        player.animator.SetFloat("speedY", 0);
    }

    void OnPAttackPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            characterAttack.Attack();
        }
    }

    void OnPAttackCancelled(InputAction.CallbackContext value)
    {
    }

    void OnStrugglePerformed(InputAction.CallbackContext value)
    {
        Debug.Log("Why do you struggl");
        if (player.playerState == CharacterMovement.PlayerState.GRABBED)
        {
            player.targetEnemy.fuckMeter -= player.struggleRate;

            if (player.targetEnemy.fuckMeter <= 0)
            {
                player.ReleaseGrabbed();
                player.targetEnemy.StateMachine.ChangeState(player.targetEnemy.IdleState);
            }
        }
    }

    void OnStruggleCancelled(InputAction.CallbackContext value)
    {

    }
}