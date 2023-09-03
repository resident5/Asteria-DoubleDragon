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
    public EnemyIdleState IdleState { get; set; }
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

    #region Stats

    [field: SerializeField] public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }
    [field: SerializeField] public float CurrentLust{ get; set; }
    [field: SerializeField] public float MaxLust { get; set; }
    public float lustRate;
    
    public bool isHorny;
    public bool isDead;

    #endregion

    public Transform worldCanvas;
    public SexMeter sexMeter;
    public float fuckMeter;
    public float maxFuckMeter = 100;
    public float fuckRate = 2f;

    public GameObject emoteObject;

    public EnemyHealthBarHandler enemyHealthBarHandler;
    
    public AnimationEventReceiver animationEventReceiver;

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
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
        StateMachine.Initialize(IdleState);


        boundaryCollider = GameObject.FindWithTag("Boundary").GetComponent<PolygonCollider2D>();
        RB = GetComponentInChildren<Rigidbody2D>();
        onEnemyHit += GameController.Instance.ComboCount;
        visCheck = GetComponentInChildren<VisibilityCheck>();
        emoteObject.SetActive(false);
        //animationEventReceiver = GetComponentInChildren<AnimationEventReceiver>();
        //animationEventReceiver.AnimationStart += HandlePlayer;
        //animationEventReceiver.AnimationEnded += HandleSexPlayer;
        player = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
        enemyHealthBarHandler = UIController.Instance.enemyUIHandlder;

    }

    private void OnDisable()
    {
        //animationEventReceiver.AnimationStart -= HandlePlayer;
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
                emoteObject.gameObject.SetActive(true);
            }
        }

        if(GameController.Instance.isPlayerDead)
        {
            StateMachine.ChangeState(IdleState);
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
            sexMeter.sexSlider.direction = Slider.Direction.RightToLeft;
            Turn();
        }
        else if (velocity.x > 0 && !FacingRight)
        {
            sexMeter.sexSlider.direction = Slider.Direction.LeftToRight;
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

    public void TakeDamage(float damageAmount)
    {
        if (!isDead)
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
    }

    public void Die()
    {
        if (enemyHealthBarHandler.targetEnemy == this)
        {
            enemyHealthBarHandler.gameObject.SetActive(false);
        }
        //Check if in ambushMode
        //Reduce enemyWave
        StateMachine.ChangeState(DeathState);
    }

    #endregion

    #region Animation Triggers

    public void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
        if(triggerType == AnimationTriggerType.ENEMYDEAD)
        {
            Destroy(gameObject);
        }
    }

    public enum AnimationTriggerType
    {
        ENEMYDEAD,
        ENEMYATTACKING,
        ENEMYGRABBING,
        ENEMYFUCKING
    }

    // Start is called before the first frame update

    #endregion
    
    //void HandlePlayer(AnimationEvent animationEvent)
    //{
    //    var playerScript = player.GetComponent<CharacterMovement>();
    //    if (IsWithinStrikingDistance)
    //    {
    //        if (!isHorny)
    //        {
    //            playerScript.TakeDamage(attackDamage);
    //        }
    //        else
    //        {
    //            StateMachine.ChangeState(HornyState);
    //            transform.position = playerScript.transform.position;
    //            playerScript.targetEnemy = this;
    //            playerScript.GetGrabbed();
    //            sexMeter.gameObject.transform.SetParent(worldCanvas);
    //        }
    //    }
    //}

    //void HandleSexPlayer(AnimationEvent animationEvent)
    //{
    //}

    public bool SetStrikingDistance(bool isWithinStrikingDistance)
    {
        return IsWithinStrikingDistance = isWithinStrikingDistance;
    }
}