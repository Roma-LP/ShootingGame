using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScaler : MonoBehaviour
{
    [SerializeField] private RectTransform[] canvases;
    [SerializeField] private float squareResolutionScale = 0.06f;
    [SerializeField] private float rectangularResolutionScale = 0.08f;

    private void Awake()
    {
        float canvasScale;
        if (Mathf.Abs(Camera.main.aspect - 4f / 3f) <= 0.001)
            canvasScale = squareResolutionScale;
        else
            canvasScale = rectangularResolutionScale;
        foreach (var canv in canvases)
            canv.localScale = Vector3.one * canvasScale;
    }
}
