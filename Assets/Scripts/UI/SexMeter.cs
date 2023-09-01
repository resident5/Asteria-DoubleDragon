using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SexMeter : MonoBehaviour
{
    public Enemy enemy;
    public Slider sexSlider;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        sexSlider.GetComponentInChildren<Slider>();
        enemy.fuckMeter = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        sexSlider.value = enemy.fuckMeter;
        
    }
}
