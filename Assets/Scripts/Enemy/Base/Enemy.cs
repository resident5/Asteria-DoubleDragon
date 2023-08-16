using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamagable, IEnemyMovable, ITriggerCheckable
{
    public Rigidbody2D RB { get; set; }
    public bool FacingRight { get; set; } = true;

    public Animator anim;
    public bool isDead = false;
    public float recoveryTime;
    public GameObject player;
    public delegate void AttackedByPlayer();
    public AttackedByPlayer onEnemyHit;

    public int attackDamage;
    
    public Collider2D boundaryCollider;
    public Vector3 colliderOffset;

    public VisibilityCheck visCheck;
    
    #region States Variables

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyPatrolState PatrolState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyHitState HitState { get; set; }
    public EnemyDeathState DeathState { get; set; }
    
    public EnemyHornyState HornyState { get; set; }
    public bool IsWithinStrikingDistance { get; set; }

    public float attackDelay;
    public float recoveryDelay;
    #endregion

    #region Idle Variables

    public float RandomMovementRange = 5f;
    public float MoveSpeed = 1f;

    #endregion

    #region DeathChecks

    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float CurrentLust{ get; set; }
    [field: SerializeField] public float MaxLust { get; set; }
    public float lustRate;


    public bool isHorny;

    #endregion

    public EnemyHealthBarHandler enemyHealthBarHandler;
    
    public AnimationEventReceiver animationEventReceiver;

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        PatrolState = new EnemyPatrolState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        HitState = new EnemyHitState(this, StateMachine);
        DeathState = new EnemyDeathState(this, StateMachine);
        HornyState = new EnemyHornyState(this, StateMachine);
    }

    void Start()
    {
        CurrentHealth = MaxHealth;
        CurrentLust = 0;
        StateMachine.Initialize(PatrolState);

        RB = GetComponentInChildren<Rigidbody2D>();
        onEnemyHit += GameController.Instance.ComboCount;
        visCheck = GetComponentInChildren<VisibilityCheck>();
        animationEventReceiver = GetComponentInChildren<AnimationEventReceiver>();
        animationEventReceiver.AnimationStart += HandlePlayer;
        player = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
        enemyHealthBarHandler = UIController.Instance.enemyUIHandlder;

    }

    private void OnDisable()
    {
        animationEventReceiver.AnimationStart -= HandlePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
        if (!isHorny)
        {
            CurrentLust += 1f * lustRate * Time.deltaTime;
            if (CurrentLust >= MaxLust)
            {
                isHorny = true;
                visCheck.gameObject.GetComponent<SpriteRenderer>().material.color = Color.magenta;
            }
        }
        Debug.Log($"Statemachine = {StateMachine.CurrentEnemyState}");
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Movement Methods

    public void MoveEnemy(Vector2 velocity)
    {
        RB.velocity = velocity;
        CheckFacingDirection(velocity);
    }

    public void CheckFacingDirection(Vector2 velocity)
    {
        if (velocity.x < 0 && FacingRight)
        {
            Turn();
        }
        else if (velocity.x > 0 && !FacingRight)
        {
            Turn();
        }
    }

    public void Turn()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0, 180, 0);
    }
    
    #endregion

    #region Health/Death State Checks

    // public void CheckDead()
    // {
    //     if (!isDead)
    //     {
    //         if (CurrentHealth > 0)
    //         {
    //             isDead = true;
    //         }
    //
    //         if (recoveryTime > 0f)
    //         {
    //             recoveryTime -= Time.deltaTime;
    //         }
    //         else
    //         {
    //             recoveryTime = 0;
    //             anim.SetBool("hit", false);
    //         }
    //     }
    // }

    public void TakeDamage(float damageAmount)
    {
        
        CurrentHealth -= damageAmount;

        StateMachine.ChangeState(HitState);

        enemyHealthBarHandler.slider.value = CurrentHealth;
        
        //anim.SetBool("hit", true);
        recoveryTime = 0.5f;

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        if (enemyHealthBarHandler.targetEnemy == this)
        {
            enemyHealthBarHandler.gameObject.SetActive(false);
        }
        StateMachine.ChangeState(DeathState);
    }

    #endregion

    #region Animation Triggers

    public void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    public enum AnimationTriggerType
    {
        ENEMYDAMAGED,
        ENEMYATTACKING
    }

    // Start is called before the first frame update

    #endregion
    
    void HandlePlayer(AnimationEvent animationEvent)
    {
        var playerScript = player.GetComponent<CharacterMovement>();
        if (IsWithinStrikingDistance)
        {
            if (!isHorny)
            {
                playerScript.TakeDamage(attackDamage);
            }
            else
            {
                transform.position = playerScript.transform.position;
                playerScript.GetGrabbed();
            }
        }
    }

    public bool SetStrikingDistance(bool isWithinStrikingDistance)
    {
        return IsWithinStrikingDistance = isWithinStrikingDistance;
    }
}