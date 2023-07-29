using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(Enemy e, EnemyStateMachine eState)
    {
        this.enemy = e;
        this.enemyStateMachine = eState;
    }

    public virtual void EnterState(){}
    public virtual void ExitState(){}
    public virtual void FrameUpdate(){}
    public virtual void PhysicsUpdate(){}
    public virtual void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType){}
    
}