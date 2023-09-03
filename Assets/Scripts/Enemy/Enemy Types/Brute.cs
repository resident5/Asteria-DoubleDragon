using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brute : Enemy
{
    public float MyRandomRange => RandomMovementRange;
    public int MyDamage => attackDamage;
    //public float strikingDistance => strik;
    public Vector3 Offset => colliderOffset;
    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Offset, MyRandomRange);
            Gizmos.DrawLine(transform.position + Offset, player.transform.position);
        }
    }


    // Start is called before the first frame update
}
