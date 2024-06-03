using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;

    void Update()
    {
        slider.onValueChanged.AddListener(listener =>
        {
            text.text = "Тривалість сесії: " + listener.ToString();
        });
    }
}
