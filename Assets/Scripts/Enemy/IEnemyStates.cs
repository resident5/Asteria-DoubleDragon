using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyStates
{
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
    public abstract void ChangeState(IEnemyStates state);
}
