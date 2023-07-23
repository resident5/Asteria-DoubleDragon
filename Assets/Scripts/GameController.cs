using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public int comboCounter = 0;
    public float comboDelay = 1.2f;
    public float startTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (comboCounter > 0)
        {
            if(Time.time - startTime >= comboDelay)
            {
                comboCounter = 0;
            }
        }
    }

    public void ComboCount()
    {
        comboCounter += 1;
        startTime = Time.time;
    }
}
