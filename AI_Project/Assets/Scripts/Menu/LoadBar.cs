using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBar : MonoBehaviour
{
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value += Time.deltaTime;
        if (slider.value >= slider.maxValue)
            slider.value = 0;
    }
}
