using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class MenuCircle : MonoBehaviour
{
    public List<Transform> buttonTransforms = new List<Transform>();
    public float radius = 2f;
    public int currentButton = 0;

    public Player1Inputs input = null;
    public InputAction action;
    // Start is called before the first frame update

    public float rotDuration = 1f;
    public float elapsedTime = 0f;

    float rotateProgress;

    public Quaternion currentRot;
    public Quaternion startRotation;
    public Quaternion targetRotation;

    public GameObject currentObjectSelected;


    private void Awake()
    {
        input = new Player1Inputs();
    }

    void Start()
    {
        startRotation = transform.rotation;
    }

    private void OnEnable()
    {
        input.Enable();
        input.UI.Navigate.performed += Rotate;

    }

    private void OnDisable()
    {
        input.Disable();
        input.UI.Navigate.performed -= Rotate;
    }

    // Update is called once per frame
    void Update()
    {
        EventCheck();
        currentRot = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, 240f), 1.5f * Time.deltaTime);
        transform.rotation = currentRot;
    }

    void EventCheck()
    {
        EventSystem e = EventSystem.current;
        currentObjectSelected = e.currentSelectedGameObject;

        e.firstSelectedGameObject = transform.GetChild(0).gameObject;
        Debug.Log("Current " + currentObjectSelected);

    }

    IEnumerator UpdatingRotate()
    {
        rotateProgress = 0;

        while (elapsedTime < rotDuration)
        {
            elapsedTime += Time.deltaTime;
            rotateProgress = Mathf.Clamp01(elapsedTime / rotDuration);
            yield return null;
        }
        rotateProgress = 1f;
    }

    void Rotate(InputAction.CallbackContext context)
    {
        if (context.control.path == "/Keyboard/s")
        {
            targetRotation = Quaternion.Euler(0f, 0f, -120f) * startRotation;
            StartCoroutine(UpdatingRotate());
        }

        if (context.control.path == "/Keyboard/w")
        {
            targetRotation = Quaternion.Euler(0f, 0f, 120f) * startRotation;
            StartCoroutine(UpdatingRotate());
        }

        Debug.Log("ROTATE " + context.control.path);

    }

    [ContextMenu("Rotate Buttons")]
    void AdjustDirection()
    {
        buttonTransforms.Clear();

        foreach (var button in transform.GetComponentsInChildren<Button>())
        {
            buttonTransforms.Add(button.transform);
        }

        foreach (var item in buttonTransforms)
        {
            var dir = transform.position - item.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            item.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    [ContextMenu("Move Buttons")]
    void AdjustPosition()
    {
        buttonTransforms.Clear();

        Debug.Log("Run in editor");
        int numOfButtons = transform.childCount;
        float angleStep = 360f / numOfButtons;
        float currentAngle = 0;

        foreach (var button in transform.GetComponentsInChildren<Button>())
        {
            buttonTransforms.Add(button.transform);
        }

        foreach (var item in buttonTransforms)
        {
            float xPos = transform.position.x + Mathf.Cos(Mathf.Deg2Rad * currentAngle) * -radius;
            float yPos = transform.position.y + Mathf.Sin(Mathf.Deg2Rad * currentAngle) * -radius;

            item.position = new Vector2(xPos, yPos);
            currentAngle += angleStep;
        }
    }
}
