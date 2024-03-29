using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement Instance;

    //public AnimationEventReceiver animationEventReceiver;

    [Header("Base")]

    #region Base

    [SerializeField]
    private float speedX;

    [SerializeField] private float speedY;

    [SerializeField] private float runX;
    [SerializeField] private float runY;

    [SerializeField] private Rigidbody2D baseRB;
    public bool canMove;

    [SerializeField] private bool facingRight = true;

    [Range(0, 1.0f)] [SerializeField] private float dampMovement = 0.5f;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private Vector3 charDefaultRelPos, baseDefPos;

    #endregion

    [Header("Jump")]

    #region Jump

    [SerializeField]
    private Rigidbody2D charRB;

    [SerializeField] private bool onBase = false;
    [SerializeField] private float jumpStrength = 10f;
    [SerializeField] private int jumpMax = 1;
    [SerializeField] private int jumpCurrent = 0;
    [SerializeField] private Transform groundDetector;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float fallingMultiplier;
    public bool wasAirborne;
    
    #endregion

    [Header("Animator")]

    #region Animator

    public bool jump;

    public bool running;

    public Animator animator;

    public enum AnimationTriggerType
    {
        PLAYERDEAD,
        PLAYERRESET
    }

    #endregion

    [Header("Boundary Collider")]

    #region Collider

    public Collider2D shadowCollider;

    public Collider2D boundaryCollider;
    public float playerHeightOffset = 0.575f;

    #endregion

    [Header("Stats")]

    #region Stats

    [SerializeField]
    private int currentHealth;

    [SerializeField] private int maxHealth;
    [SerializeField] private int attackDamage;
    [SerializeField] private Slider playerHealthBar;
    
    public Enemy targetEnemy;

    public float struggleRate = 1.5f;
    #endregion

    [Header("Audio")]
    public List<AudioClip> attackAudioClips;

    public enum PlayerState
    {
        NEUTRAL,
        AIRBORNE,
        DAMAGED,
        ATTACKING,
        GRABBED,
        DEAD
    }

    public PlayerState playerState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //animationEventReceiver = GetComponentInChildren<AnimationEventReceiver>();
        //animationEventReceiver.AnimationEnded += HandlePlayerReset;
        //animationEventReceiver.DeathAnimationStarted += Die;
        //animationEventReceiver.DeathAnimationEnded += Death;
        charDefaultRelPos = charRB.transform.localPosition;
        boundaryCollider = GameObject.FindGameObjectWithTag("Boundary").GetComponent<BoxCollider2D>();
        playerHealthBar = GameObject.Find("Player Plate").GetComponent<Slider>();
        currentHealth = maxHealth;
        playerHealthBar.maxValue = maxHealth;
        playerHealthBar.value = maxHealth;

        playerState = PlayerState.NEUTRAL;
    }

    private void OnDisable()
    {
        //animationEventReceiver.AnimationEnded -= HandlePlayerReset;
        //animationEventReceiver.DeathAnimationEnded -= Death;
        //animationEventReceiver.DeathAnimationStarted -= Die;
    }

    void Update()
    {
        DebugController.Instance.baseVelocityText.text = "x: " + baseRB.velocity.x + " y " + baseRB.velocity.y;
        DebugController.Instance.charVelocityText.text = "x: " + charRB.velocity.x + " y " + charRB.velocity.y;
    }

    private void FixedUpdate()
    {
    }

    public void Move(float moveX, float moveY, bool run)
    {
        //Debug.Log($"MoveX = {moveX}, MoveY = {moveY}");
        //Make detect base only detext the players base and not the enemy
        DetectBase();

        if (playerState == PlayerState.NEUTRAL || playerState == PlayerState.AIRBORNE)
        {
            Vector3 targetVelocity;

            // if (run)
            // {
            //     targetVelocity = new Vector2(moveX * runX, moveY * runY);
            //     //Debug.Log("RUN FASTER GIRL");
            // }
            // else
            // {
            targetVelocity = new Vector2(moveX * speedX, moveY * speedY);
            // }

            Vector2 velocity2D = Vector3.SmoothDamp(baseRB.velocity, targetVelocity, ref velocity, dampMovement);
            baseRB.velocity = velocity2D;
            animator.SetBool("move", MathF.Abs(moveX) > 0 || Mathf.Abs(moveY) > 0);
            
            transform.position = CalculateBounds();
            

            if (onBase)
            {
                //charRB.velocity = velocity2D;
                charRB.velocity = Vector2.zero;
                if (charRB.transform.localPosition != charDefaultRelPos)
                {
                    var charTransform = charRB.transform;
                    charTransform.localPosition = new Vector2(charTransform.localPosition.x, charDefaultRelPos.y);
                }
            }
            else
            {
                if (charRB.velocity.y < 0)
                {
                    charRB.gravityScale = fallingMultiplier;
                }

                charRB.velocity = new Vector2(velocity2D.x, charRB.velocity.y);
            }

            if (jump && jumpCurrent < jumpMax)
            {
                //????
                playerState = PlayerState.AIRBORNE;
                charRB.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
                charRB.gravityScale = jumpMultiplier;
                jump = false;
                jumpCurrent++;
                onBase = false;
                wasAirborne = true;
            }

            if (charRB.transform.localPosition != charDefaultRelPos)
            {
                //print("pos diff- local: " + charRB.transform.localPosition + "  --default: " + charDefaultRelPos);
                var charTransform = charRB.transform;
                charTransform.localPosition = new Vector2(charDefaultRelPos.x, charTransform.localPosition.y);
            }


            if (moveX > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveX < 0 && facingRight)
            {
                Flip();
            }
        }
        else
        {
            baseRB.velocity = Vector2.zero;
            charRB.velocity = Vector2.zero;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void DetectBase()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundDetector.position, Vector2.down, groundDistance, groundLayerMask);
        if (hit.collider != null)
        {
            //Debug.Log("We are hitting the base also known as " + hit.collider.name);
            onBase = true;
            charRB.gravityScale = 0;
            jumpCurrent = 0;
            if (wasAirborne)
            {
                wasAirborne = false;
                animator.SetBool("isJumping", false);
                playerState = PlayerState.NEUTRAL;
                Debug.Log("IS THIS ON?");
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private Vector2 CalculateBounds()
    {
        Bounds shadowBounds = shadowCollider.bounds;
        Bounds boundaryBounds = boundaryCollider.bounds;

        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(transform.position.x, boundaryBounds.min.x + shadowBounds.extents.x,
                boundaryBounds.max.x - shadowBounds.extents.x),
            Mathf.Clamp(transform.position.y, boundaryBounds.min.y + (shadowBounds.extents.y),
                boundaryBounds.max.y - shadowBounds.extents.y)
        );

        return clampedPosition;
    }

    public void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;
        playerHealthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            playerState = PlayerState.DEAD;
            animator.SetBool("isDead", true);
            //Death;
        }else
        {
            playerState = PlayerState.DAMAGED;
            animator.SetTrigger("hit");

        }
        // canMove = false;


    }

    //public void Die(AnimationEvent animationEvent)
    //{
    //    playerState = PlayerState.DEAD;
    //}

    public void Death()
    {
        GameController.Instance.playerLastCoordinates = gameObject.transform.position;
        GameController.Instance.isPlayerDead = true;
        Destroy(gameObject);
    }

    public void GetGrabbed()
    {
        playerState = PlayerState.GRABBED;
        charRB.gameObject.SetActive(false);
        shadowCollider.gameObject.SetActive(false);
        // canMove = false;
    }

    public void ReleaseGrabbed()
    {
        playerState = PlayerState.NEUTRAL;
        charRB.gameObject.SetActive(true);
        shadowCollider.gameObject.SetActive(true);
        // canMove = false;
    }

    public void OnGrabStruggle()
    {
        
    }

    public void HandlePlayerReset(AnimationEvent animationEvent)
    {
        // canMove = true;
    }

    public void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        if (triggerType == AnimationTriggerType.PLAYERRESET)
        {
            playerState = PlayerState.NEUTRAL;
            Debug.Log("IS THIS ON?3");
        }

        if (triggerType == AnimationTriggerType.PLAYERDEAD)
        {
            playerState = PlayerState.DEAD;
            Death();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(groundDetector.transform.position, Vector2.down * groundDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector3(boundaryCollider.bounds.min.x + shadowCollider.bounds.extents.x, transform.position.y));
        
        Gizmos.DrawLine(transform.position, new Vector3(boundaryCollider.bounds.max.x - shadowCollider.bounds.extents.x, transform.position.y));

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, boundaryCollider.bounds.min.y + (shadowCollider.bounds.extents.y)));

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, boundaryCollider.bounds.max.y - shadowCollider.bounds.extents.y));



    }
}