using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speedX;
    public float speedY;
    public Rigidbody2D rb;
    public bool canMove;

    bool facingRight = true;

    [Range(0, 1.0f)]
    public float dampMovement = 0.5f;
    Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Move(float moveX, float moveY, bool jump)
    {
        if (canMove)
        {
            Vector3 targetVelocity = new Vector2(moveX * speedX, moveY * speedY);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, dampMovement);

            Debug.Log("Player should be moving tbh");

            if (moveX > 0 && !facingRight)
            {
                flip();
            }
            else if (moveX < 0 && facingRight)
            {
                flip();
            }

        }
    }

    private void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
}
