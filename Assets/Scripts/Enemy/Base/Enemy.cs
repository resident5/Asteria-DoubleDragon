using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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


    public Collider2D boundaryCollider;
    public Vector3 colliderOffset;

    public VisibilityCheck visCheck;

    #region States Variables

    [field: SerializeField] public EnemyStateMachine StateMachine { get; set; }
    public EnemyPatrolState PatrolState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }

    public bool IsWithinStrikingDistance { get; set; }

    public float attackDelay;

    #endregion

    #region Idle Variables

    public float RandomMovementRange = 5f;
    public float MoveSpeed = 1f;

    #endregion

    #region DeathChecks

    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }

    #endregion

    public AnimationEventReceiver animationEventReceiver;

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        PatrolState = new EnemyPatrolState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
    }

    void Start()
    {
        CurrentHealth = MaxHealth;
        StateMachine.Initialize(PatrolState);

        RB = GetComponentInChildren<Rigidbody2D>();
        onEnemyHit += GameController.Instance.ComboCount;
        visCheck = GetComponentInChildren<VisibilityCheck>();
        animationEventReceiver = GetComponentInChildren<AnimationEventReceiver>();
        animationEventReceiver.hitAnimationStart += HandlePlayerHit;
        player = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
        CheckDead();
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

    public void StopEnemy()
    {
        if (anim.GetBool("move") == true)
        {
            anim.SetBool("move", false);
        }

        RB.velocity = Vector2.zero;
    }

    #endregion

    #region Health/Death State Checks

    public void CheckDead()
    {
        if (!isDead)
        {
            if (CurrentHealth > 0)
            {
                isDead = true;
            }

            if (recoveryTime > 0f)
            {
                recoveryTime -= Time.deltaTime;
            }
            else
            {
                recoveryTime = 0;
                anim.SetBool("hit", false);
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        anim.SetBool("hit", true);
        recoveryTime = 0.5f;

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
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
    
    void HandlePlayerHit(AnimationEvent animationEvent)
    {
        var playerScript = player.GetComponent<CharacterMovement>();
        Debug.Log("Player should take damage");
        if (IsWithinStrikingDistance)
        {
            playerScript.TakeDamage(50);
        }
    }

    public bool SetStrikingDistance(bool isWithinStrikingDistance)
    {
        return IsWithinStrikingDistance = isWithinStrikingDistance;
    }
}