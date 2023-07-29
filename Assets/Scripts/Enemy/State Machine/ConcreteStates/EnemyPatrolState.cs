using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class EnemyPatrolState : EnemyState
{
    private Bounds boundaryBounds;
    public EnemyPatrolState(Enemy e, EnemyStateMachine eState) : base(e, eState)
    {
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
        base.FrameUpdate();

        if (enemy.visCheck.isVisible)
        {
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
    }

    private void OnBecameVisible()
    {
        
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