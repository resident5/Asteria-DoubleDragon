using UnityEngine;
using Cinemachine;

public class AmbushTracker : MonoBehaviour
{
    public PolygonCollider2D confiner;
    public BoxCollider2D wall;
    public Vector2[] baseConfinerPoints;
    public Vector2 baseWallOffset;
    public Vector2 baseWallSize;
    
    public AmbushSpot[] ambushSpot;

    void Start()
    {
        confiner = GameObject.FindWithTag("Boundary").GetComponent<PolygonCollider2D>();
        wall = confiner.GetComponent<BoxCollider2D>();
        baseConfinerPoints = confiner.points;
        baseWallOffset = wall.offset;
        baseWallSize = wall.size;
    }

    public void BeginEnemyAmbush(int index)
    {
        confiner.points = ambushSpot[index].confinerBounds;
        wall.size = ambushSpot[index].wallSize;
        wall.offset = ambushSpot[index].wallOffset;
        Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject
            .GetComponent<CinemachineConfiner2D>().InvalidateCache();
    }

    public void EndEnemyAmbush()
    {
        confiner.points = baseConfinerPoints;
        wall.size = baseWallSize;
        wall.offset = baseWallOffset;
        Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject
            .GetComponent<CinemachineConfiner2D>().InvalidateCache();
    }
    
}
