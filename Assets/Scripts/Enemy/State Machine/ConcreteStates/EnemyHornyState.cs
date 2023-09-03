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
        enemy.fuckMeter = 20f;
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.anim.SetBool("isMatingPress", false);
        enemy.anim.speed = 1f;
        enemy.CurrentLust = 0;
        enemy.fuckMeter = 0;
        enemy.sexMeter.transform.SetParent(enemy.gameObject.transform);
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

        if (enemy.fuckMeter >= 100f)
        {
            var playerScript = enemy.player.GetComponent<CharacterMovement>();
            playerScript.TakeDamage(enemy.attackDamage);
            playerScript.ReleaseGrabbed();
            enemy.StateMachine.ChangeState(enemy.ChaseState);
        }
        else if (enemy.fuckMeter <= 0f)
        {
            var playerScript = enemy.player.GetComponent<CharacterMovement>();
            playerScript.ReleaseGrabbed();
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
        if (triggerType == Enemy.AnimationTriggerType.ENEMYFUCKING)
        {
            enemy.fuckMeter += enemy.fuckRate;
        }
    }
}