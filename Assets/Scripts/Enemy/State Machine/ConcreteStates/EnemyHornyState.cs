using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class EnemyHornyState : EnemyState
{
    public EnemyHornyState(Enemy e, EnemyStateMachine eState) : base(e, eState)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.anim.SetBool("move", false);
        enemy.anim.SetBool("isMatingPress", true);
        enemy.anim.speed = 1f;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.anim.SetBool("isMatingPress", false);
        enemy.anim.speed = 1f;
        enemy.fuckMeter = 0;
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        //Debug.Log($"Fuckmeter = {fuckMeter}");
        enemy.fuckMeter += enemy.fuckRate * Time.deltaTime;
        if (enemy.fuckMeter > 40f)
        {
            enemy.anim.speed = 3f;
            //Debug.Log("enemy.fuck normal");
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
        if(triggerType == Enemy.AnimationTriggerType.ENEMYFUCKING)
        {
            enemy.fuckMeter += enemy.fuckRate;
        }
    }
}