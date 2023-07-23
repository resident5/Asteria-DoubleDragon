using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMovable
{
    Rigidbody2D RB { get; set; }
    
    bool FacingRight { get; set; }
    
    void MoveEnemy(Vector2 velocity);
    
    void CheckFacingDirection(Vector2 velocity);
}
