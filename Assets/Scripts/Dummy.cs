using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dummy : MonoBehaviour
{
    public int health;
    public Animator anim;
    public float recoveryTime;

    public bool isDead = false;
    public delegate void attackedByPlayer();

    public attackedByPlayer onEnemyHit;
    // Start is called before the first frame update
    void Start()
    {
        onEnemyHit += GameController.Instance.ComboCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (health > 0)
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        anim.SetBool("hit", true);
        recoveryTime = 0.5f;
    }
}
