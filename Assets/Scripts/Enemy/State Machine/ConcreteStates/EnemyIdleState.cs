using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class EnemyIdleState : EnemyState
{
    private Bounds boundaryBounds;
    public EnemyIdleState(Enemy e, EnemyStateMachine eState) : base(e, eState)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.anim.SetBool("move", false);
    }

    public override void ExitState()
    {
        base.ExitState();
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.visCheck.isVisible && enemy.player != null)
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