using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyHitState : EnemyState
{
    public float currentDelay = 0;
    public EnemyHitState(Enemy e, EnemyStateMachine eState) : base(e, eState)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        currentDelay = 0;
        
        enemy.anim.SetTrigger("damaged");
        enemy.enemyHealthBarHandler.targetEnemy = enemy;
        enemy.enemyHealthBarHandler.gameObject.SetActive(true);
        if (enemy.enemyHealthBarHandler.targetEnemy == enemy)
        {
            enemy.enemyHealthBarHandler.slider.maxValue = enemy.MaxHealth;
            //enemy.enemyHealthBarHandler.ModifySlier(-damageAmount);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        currentDelay += Time.deltaTime;
        if (currentDelay >= enemy.recoveryDelay)
        {
            enemy.StateMachine.ChangeState(enemy.StateMachine.PreviousEnemyState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }
}