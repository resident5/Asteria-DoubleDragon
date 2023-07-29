using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public static CharacterMovement Instance;

    [Header("Base")]
    #region Base
    [SerializeField] private float speedX;
    [SerializeField] private float speedY;

    [SerializeField] private float runX;
    [SerializeField] private float runY;

    [SerializeField] private Rigidbody2D baseRB;
    [SerializeField] private bool canMove;

    [SerializeField] private bool facingRight = true;

    [Range(0, 1.0f)]
    [SerializeField] private float dampMovement = 0.5f;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private Vector3 charDefaultRelPos, baseDefPos;
    #endregion

    [Header("Jump")]
    #region Jump
    [SerializeField] private Rigidbody2D charRB;
    [SerializeField] private bool onBase = false;
    [SerializeField] private float jumpStrength = 10f;
    [SerializeField] private int jumpMax = 1;
    [SerializeField] private int jumpCurrent = 0;
    [SerializeField] private Transform groundDetector;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float fallingMultiplier;
    #endregion

    [Header("Animator")]
    #region Animator

    public bool jump;
    public bool running;

    public Animator animator;
    #endregion

    [Header("Boundary Collider")]
    #region Collider
    
    public Collider2D shadowCollider;
    public Collider2D boundaryCollider;
    public float playerHeightOffset = 0.575f;

    #endregion

    [Header("Stats")]

    #region Stats

    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int attackDamage;
    
    #endregion
    
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
        charDefaultRelPos = charRB.transform.localPosition;
        currentHealth = maxHealth;
    }
    // Update is called once per frame
    void Update()
    {
        DebugController.Instance.baseVelocityText.text = "x: " + baseRB.velocity.x + " y " + baseRB.velocity.y;
        DebugController.Instance.charVelocityText.text = "x: " + charRB.velocity.x + " y " + charRB.velocity.y;

        //if(running)
        //{
        //    Debug.Log("OMG IM RUNNING");
        //}
    }

    private void FixedUpdate()
    {
    }

    public void Move(float moveX, float moveY, bool run)
    {
        //Make detect base only detext the players base and not the enemy
        DetectBase();

        if (canMove)
        {
            Vector3 targetVelocity;

            if (run)
            {
                targetVelocity = new Vector2(moveX * runX, moveY * runY);
                //Debug.Log("RUN FASTER GIRL");
            }
            else
            {
                targetVelocity = new Vector2(moveX * speedX, moveY * speedY);
            }

            Vector2 velocity2D = Vector3.SmoothDamp(baseRB.velocity, targetVelocity, ref velocity, dampMovement);
            //Bounds shadowBounds = shadowCollider.bounds;
            //Bounds boundaryBounds = boundaryCollider.bounds;

            ////Vector2 clampedPosition = boundaryCollider.ClosestPoint(baseRB.position);
            //Vector2 clampedPosition = new Vector2(
            //    Mathf.Clamp(transform.position.x, boundaryBounds.min.x + shadowBounds.extents.x, MathF.Abs(boundaryBounds.max.x) - shadowBounds.extents.x),
            //    Mathf.Clamp(transform.position.y, boundaryBounds.min.y + shadowBounds.extents.y, MathF.Abs(boundaryBounds.max.y) - shadowBounds.extents.y)
            //);

            //Debug.Log("Boundary Bounds " + boundaryBounds.min.y);
            //Debug.Log("Shadow Bounds " + shadowBounds.extents.y);
            baseRB.velocity = velocity2D;

            //transform.position = new Vector2(clampedPosition.x, clampedPosition.y);
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

            if (jump)
            {
                //????
                charRB.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
                charRB.gravityScale = jumpMultiplier;
                jump = false;
                jumpCurrent++;
                onBase = false;

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


        if (velocity.x == 0)
        {
            //The player has stopped moving
            //running = false;
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
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private Vector2 CalculateBounds()
    {
        Bounds shadowBounds = shadowCollider.bounds;
        Bounds boundaryBounds = boundaryCollider.bounds;
        
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(transform.position.x, boundaryBounds.min.x + shadowBounds.extents.x, MathF.Abs(boundaryBounds.max.x) - shadowBounds.extents.x),
            Mathf.Clamp(transform.position.y, boundaryBounds.min.y + (shadowBounds.extents.y), boundaryBounds.max.y - shadowBounds.extents.y)
        );
        
        return clampedPosition;
    }

    public void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;
        animator.SetBool("hit", true);
        if (currentHealth <= damageTaken)
        {
            Debug.Log("Player Died");
            //Death;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(groundDetector.transform.position, Vector2.down * groundDistance);
    }
}
