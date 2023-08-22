using System;
using UnityEngine;

[Serializable]
public class AmbushSpot
{
    public Vector2[] confinerBounds = new Vector2[4];
    public Vector2 wallOffset;
    public Vector2 wallSize;
}