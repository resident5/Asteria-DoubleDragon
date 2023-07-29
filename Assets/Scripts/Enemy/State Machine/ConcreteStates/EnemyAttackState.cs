using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private Transform _playerTransform;
    private Vector3 targetDirection;
    private float xOffset = 1.5f;

    private float timeSinceLastAttack;
    private float attackDelay;
    private bool canAttack = true;


    public EnemyAttackState(Enemy e, EnemyStateMachine eState) : base(e, eState)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        attackDelay = e.attackDelay;
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        enemy.MoveEnemy(Vector2.zero);
        if (!enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
        else
        {
            if (canAttack)
            {
                enemy.anim.SetTrigger("attack");
                enemy.anim.SetBool("move",false);
                timeSinceLastAttack = Time.time;
                canAttack = false;
            }
            else if (!canAttack && Time.time - timeSinceLastAttack >= attackDelay)
            {
                canAttack = true;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        if (triggerType == Enemy.AnimationTriggerType.ENEMYATTACKING)
        {
            Debug.Log("Player needs to be hit");
        }
    }
}