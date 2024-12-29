using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    Slider slider;
    public string loadFloatDataName;

    void Start()
    {
        slider = GetComponent<Slider>();
        LoadFloatData();
    }

    public void LoadFloatData()
    {
        slider.value = PlayerPrefs.GetFloat(loadFloatDataName, 1);
    }
}