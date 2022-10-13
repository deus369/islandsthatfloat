using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class SliderText : MonoBehaviour
{
    private TextMeshProUGUI sliderText;
    public Slider slider;
    string text;

    public enum DisplayType
    {
        Percentage,
        FloatValue,
        IntValue
    }
    public DisplayType displayType;

    // Start is called before the first frame update
    void Start()
    {
        sliderText = gameObject.GetComponent<TextMeshProUGUI>();
        updateText();
    }
    
    public void updateText()
    {
        if (displayType == DisplayType.Percentage)
        {
            float percentage = Mathf.InverseLerp(slider.minValue, slider.maxValue, slider.value);
            text = Mathf.RoundToInt(percentage * 100) + "%";
        }
        if (displayType == DisplayType.IntValue)
        {
            text = Mathf.Round(slider.value).ToString();
        }
        if (displayType == DisplayType.FloatValue)
        {
            text = slider.value.ToString("f1");
        }

        if(sliderText != null)
        {
            sliderText.text = text;
        } 
    }
}
