using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform playerTransform;
    private float attackRadius = 3f;

    private Vector2 _targetPos;
    private Vector2 _direction;

    private Bounds boundaryBounds;

    private float startTime;
    private float waitTime = 1.5f;

    private float distanceFromPlayer = 1f;


    public EnemyChaseState(Enemy e, EnemyStateMachine eState) : base(e, eState)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        boundaryBounds = enemy.boundaryCollider.bounds;
        _targetPos = GetRandomPointAroundPlayer();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.anim.SetBool("move", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();


        if (enemy.IsWithinStrikingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
        else
        {
            var enemyPos = (Vector2)enemy.transform.position;
            _direction = (_targetPos - enemyPos).normalized;
            enemy.MoveEnemy(_direction * enemy.MoveSpeed);
            enemy.anim.SetBool("move", true);

            if ((enemyPos - _targetPos).sqrMagnitude < 0.01f)
            {
                _targetPos = GetRandomPointAroundPlayer();
            }

        }
        //Debug.Log($"Target pos: {_targetPos}\n Enemy Pos: {enemyPos}\n Player Pos: {_playerTransform.transform.position}");


        //float dist = Vector2.Distance(_playerTransform.position, enemy.transform.position);

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public Vector2 GetRandomPointAroundPlayer()
    {
        Vector2 clampedPosition = enemy.gameObject.transform.position;

        if (enemy.player != null)
        {
            playerTransform = enemy.player.transform;

            var randomPos = (Vector2)playerTransform.position + Random.insideUnitCircle * enemy.RandomMovementRange;

            clampedPosition = new Vector2(
                Mathf.Clamp(randomPos.x, boundaryBounds.min.x + enemy.colliderOffset.x, Mathf.Abs(boundaryBounds.max.x) - enemy.colliderOffset.x),
                Mathf.Clamp(randomPos.y, boundaryBounds.min.y, Mathf.Abs(boundaryBounds.max.y) - enemy.colliderOffset.y)
            );
        }

        return clampedPosition;
    }
}