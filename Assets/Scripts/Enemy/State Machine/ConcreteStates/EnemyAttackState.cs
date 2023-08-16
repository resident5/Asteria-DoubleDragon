using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private Transform _playerTransform;
    private Vector3 targetDirection;
    private float xOffset = 1.5f;

    private float attackTimer;
    private bool canAttack = true;


    public EnemyAttackState(Enemy e, EnemyStateMachine eState) : base(e, eState)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
            attackTimer += Time.deltaTime;
            if (attackTimer >= enemy.attackDelay)
            {
                if (!enemy.isHorny)
                {
                    enemy.anim.SetTrigger("attack");
                }
                else
                {
                    enemy.anim.SetTrigger("grab");
                    enemy.StateMachine.ChangeState(enemy.HornyState);
                }
                attackTimer = 0;
            }

            // if (!canAttack && Time.time - timeSinceLastAttack >= enemy.attackDelay)
            // {
            //     canAttack = true;
            // }
            // else if (canAttack)
            // {
            // }
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