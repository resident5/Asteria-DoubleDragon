using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.2f;
    public LayerMask enemyLayers;
    public Animator animator;

    public Collider2D attackCollider;
    public ContactFilter2D contactFilter;
    public List<Collider2D> enemyColliders = new List<Collider2D>();

    public int damage = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        //Play an attack animation
        animator.SetTrigger("Attack");

        //Detect enemies in range of attack
        contactFilter.SetLayerMask(enemyLayers);
        attackCollider.OverlapCollider(contactFilter, enemyColliders);
        if (enemyColliders.Count > 0)
        {
            foreach(var col in enemyColliders)
            {
                //Enemies take damage
                Dummy dummy = col.GetComponent<Dummy>();
                if (dummy != null)
                {
                    dummy.TakeDamage(damage);
                    dummy.onEnemyHit?.Invoke();
                }
                Debug.Log("We hit " + col.transform.name);
            }
        }

        //Damage them
    }
}
