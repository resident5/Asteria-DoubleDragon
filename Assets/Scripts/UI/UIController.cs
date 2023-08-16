using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController instance;

    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIController>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }
    public EnemyHealthBarHandler enemyUIHandlder;

    private void Start()
    {
        enemyUIHandlder.gameObject.SetActive(false);
    }
}