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
            if (CharacterMovement.Instance.playerState != CharacterMovement.PlayerState.DEAD)
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
                    }
                    attackTimer = 0;
                }
            }
        }

        if(GameController.Instance.isPlayerDead)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        var playerScript = enemy.player.GetComponent<CharacterMovement>();

        if (triggerType == Enemy.AnimationTriggerType.ENEMYATTACKING)
        {
            playerScript.TakeDamage(enemy.attackDamage);

        }

        if (triggerType == Enemy.AnimationTriggerType.ENEMYGRABBING)
        {
            enemy.StateMachine.ChangeState(enemy.HornyState);
            enemy.transform.position = playerScript.transform.position;
            playerScript.targetEnemy = enemy;
            playerScript.GetGrabbed();
            enemy.sexMeter.gameObject.transform.SetParent(enemy.worldCanvas);
        }
    }
}