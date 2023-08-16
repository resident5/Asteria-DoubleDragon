using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyDeathState : EnemyState
{

    public EnemyDeathState(Enemy e, EnemyStateMachine eState) : base(e, eState)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.RB.velocity = Vector2.zero;
        enemy.anim.SetBool("death", true);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
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