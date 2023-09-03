using System;
using UnityEngine;

public class EnemyStrikingDistanceCheck : MonoBehaviour
{
    private Enemy enemy;

    public void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Shadow == Shadow
        if (other.gameObject == enemy.player.GetComponent<CharacterMovement>().shadowCollider.gameObject)
        {
            enemy.SetStrikingDistance(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == enemy.player.GetComponent<CharacterMovement>().shadowCollider.gameObject)
        {
            enemy.SetStrikingDistance(false);
        }
    }
}