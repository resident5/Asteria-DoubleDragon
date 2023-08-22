using System;
using UnityEngine;

public class AmbushTrigger : MonoBehaviour
{
    public int ambushIndex;
    private bool wasTriggered = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log($"GAME OBJECT COLLIDED = {other.gameObject.name}");
        if (other.gameObject.CompareTag("Player"))
        {
            if (wasTriggered == false)
            {
                //Maybe just do this in the GAMECONTROLLER script instead?
                
                // Debug.Log("Triggered Ambush");
                GameController.Instance.ambushTracker.BeginEnemyAmbush(ambushIndex);
                GameController.Instance.BeginAmbush();
                wasTriggered = true;
            }
        }
    }
}