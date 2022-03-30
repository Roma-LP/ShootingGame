using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ChangeSensitivity : MonoBehaviour
{
    [SerializeField] private TMP_InputField sensitivityValue;
    [SerializeField] private TMP_InputField sensitivityAimValue;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider sensitivityAimSlider;

    private void Awake()
    {
        sensitivitySlider.onValueChanged.AddListener(delegate { ChangeSensitivityValue(); });
        sensitivityAimSlider.onValueChanged.AddListener(delegate { ChangeSensitivityAimValue(); });
        sensitivityValue.onValueChanged.AddListener(delegate { ChangeSensitivitySlider(); });
        sensitivityAimValue.onValueChanged.AddListener(delegate { ChangeSensitivityAimSlider(); });

        if (PlayerPrefs.HasKey("Sensitivity"))
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        else
            PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
         
        if (PlayerPrefs.HasKey("SensitivityAim"))
            sensitivityAimSlider.value = PlayerPrefs.GetFloat("SensitivityAim");
        else
            PlayerPrefs.SetFloat("SensitivityAim", sensitivityAimSlider.value);

        sensitivityValue.text = sensitivitySlider.value.ToString("F1");
        sensitivityAimValue.text = sensitivityAimSlider.value.ToString("F1");
    }
    private void ChangeSensitivityValue()
    {
        sensitivityValue.text = sensitivitySlider.value.ToString("F1");
    }

    private void ChangeSensitivityAimValue()
    {
        sensitivityAimValue.text = sensitivityAimSlider.value.ToString("F1");
    }
    private void ChangeSensitivitySlider()
    {
        sensitivitySlider.value = float.Parse(sensitivityValue.text);
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
    } 
    private void ChangeSensitivityAimSlider()
    {
        sensitivityAimSlider.value = float.Parse(sensitivityAimValue.text);
        PlayerPrefs.SetFloat("SensitivityAim", sensitivityAimSlider.value);
    }
}
