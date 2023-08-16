using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class EnemyHornyState : EnemyState
{
    public float fuckMeter;
    public float maxFuckMeter = 100;
    public float fuckRate = 7f;
    public EnemyHornyState(Enemy e, EnemyStateMachine eState) : base(e, eState)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.anim.SetBool("move", false);
        enemy.anim.SetBool("isMatingPress", true);
        fuckMeter = 0;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.anim.speed = 1f;
        fuckMeter = 0;
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Debug.Log($"Fuckmeter = {fuckMeter}");
        fuckMeter += fuckRate * Time.deltaTime;
        if (fuckMeter < 40f)
        {
            fuckRate = 7f;
            Debug.Log("Fuck normal");
        }
        else if(fuckMeter < 70f)
        {
            fuckRate = 14f;
            enemy.anim.speed = 1.5f;
            Debug.Log("Fuck Faster");
        }
        else if(fuckMeter <= 100f)
        {
            fuckMeter = 0;
            Debug.Log("CUMMMING!!!");
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