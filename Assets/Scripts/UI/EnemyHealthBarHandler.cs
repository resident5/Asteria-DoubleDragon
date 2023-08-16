using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarHandler : MonoBehaviour
{
    public Enemy targetEnemy;
    public Slider slider;

    private void Update()
    {
    }

    public void ModifySlier(float value)
    {
        slider.value += value;
    }
}
