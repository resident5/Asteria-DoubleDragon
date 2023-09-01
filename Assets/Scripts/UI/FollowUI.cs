using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUI : MonoBehaviour
{

    private Camera cam;
    public Transform target;
    private Enemy enemy;
    public Vector3 offset;
    public RectTransform canvasRect;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        //transform.SetParent(enemy.worldCanvas);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = cam.WorldToViewportPoint(target.position);
        Vector2 canvasPos = new Vector2(
            (screenPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (screenPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)
            );
        transform.position = canvasPos;

        Debug.Log(screenPos);
    }
}
