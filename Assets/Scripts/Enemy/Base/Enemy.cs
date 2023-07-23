using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour, IDamagable, IEnemyMovable
{
    public Rigidbody2D RB { get; set; }
    public bool FacingRight { get; set; } = true;

    public float moveSpeed;
    //public bool facingRight = true;

    public Animator anim;
    public bool isDead = false;
    public float recoveryTime;

    public delegate void attackedByPlayer();

    public attackedByPlayer onEnemyHit;
    public CharacterMovement playerTarget;

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }

    #region DeathChecks
    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }

    #endregion
    
    #region Animation Triggers

    public void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }
    public enum AnimationTriggerType
    {
        ENEMYDAMAGED,
        FOOTSTEPSOUND
    }
    // Start is called before the first frame update
    #endregion

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);

    }

    void Start()
    {
        CurrentHealth = MaxHealth;
        RB = GetComponentInChildren<Rigidbody2D>();
        playerTarget = CharacterMovement.Instance;
        onEnemyHit += GameController.Instance.ComboCount;
        
        StateMachine.Initialize(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
        //FindPlayer();
        CheckDead();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Movement Methods
    
    public Vector2 GetRandomCoordinateAroundPlayer(int index)
    {
        Vector2[] locationArray =
        {
            new Vector2(1f, 1f).normalized,
            new Vector2(-1f, 1f).normalized,
            new Vector2(-1f, -1f).normalized,
            new Vector2(1f, -1f).normalized,
        };

        if (playerTarget == null)
        {
            Debug.Log("NULL REF???");
        }

        return locationArray[index] + (Vector2)playerTarget.transform.position;
    }

    public void Turn()
    {
        FacingRight = !FacingRight;
        transform.Rotate(0, 180, 0);
    }
    
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
        else if(velocity.x > 0 && !FacingRight)
        {
            Turn();
        }
        // if (playerTarget.transform.position.x < transform.position.x)
        // {
        //     // Walk left towards them
        //     if (facingRight)
        //     {
        //         Turn();
        //     }
        // }
        //
        // if (playerTarget.transform.position.x > transform.position.x)
        // {
        //     // Walk right towards them
        //     if (!facingRight)
        //     {
        //         Turn();
        //     }
        // }
    }
    
    #endregion
    
    public void FindPlayer()
    {
        if (playerTarget.gameObject.transform.position.y > transform.position.y)
        {
            //MoveUp();
        }
        else if (playerTarget.gameObject.transform.position.y < transform.position.y)
        {
            //MoveDown();
        }
    }

    public void EngageState()
    {
        Vector2 targetPosition = GetRandomCoordinateAroundPlayer(Random.Range(0, 4));

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        //Go to a random spot near the player
        //Check if the player is looking at you
        //If the player is not looking at you move towards the same Y coordinates to prepare to attack
        //If they are looking at you back up while still looking at the player
        //If they are close to you while looking at you wait a while and then attack
    }

    
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

    public void Damage(float damageAmount)
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


}