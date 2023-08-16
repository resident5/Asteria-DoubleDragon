using System;
using UnityEngine;

public class EnemyStrikingDistanceCheck : MonoBehaviour
{
    public GameObject playerTarget { get; set; }
    private Enemy enemy;

    public void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == playerTarget)
        {
            enemy.SetStrikingDistance(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == playerTarget)
        {
            enemy.SetStrikingDistance(false);
        }
    }
}