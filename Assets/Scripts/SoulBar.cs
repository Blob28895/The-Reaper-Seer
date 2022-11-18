using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMax(int val)
    {
        slider.maxValue = val;
        slider.value = val;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetCurrent(int val)
    {
        slider.value = val;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
