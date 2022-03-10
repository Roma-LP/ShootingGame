using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairScatter : MonoBehaviour
{
    [SerializeField] private float normalScatter = 1f;
    [SerializeField] private float shootScatter = 2f;
    [SerializeField, Min(0.01f)] private float stepAnim;
    [SerializeField] private Image crosshair;


    private Vector3 normalCrosshair;
    private Vector3 shootCrosshair;
    private Vector3 stepCrosshair;

    private void Awake()
    {
        normalCrosshair = new Vector3(normalScatter, normalScatter, normalScatter);
        shootCrosshair = new Vector3(shootScatter, shootScatter, shootScatter);
        stepCrosshair = new Vector3(stepAnim, stepAnim, stepAnim);
        crosshair.rectTransform.localScale = normalCrosshair;
    }
    public void ChangeSizeCrosshairOnShoot()
    {
        if (crosshair.rectTransform.localScale.x < shootCrosshair.x)
            crosshair.rectTransform.localScale += stepCrosshair * Time.deltaTime;
        else
            crosshair.rectTransform.localScale = shootCrosshair;
    }
    public void ChangeSizeCrosshairOnNormal()
    {
        if (crosshair.rectTransform.localScale.x > normalCrosshair.x)
            crosshair.rectTransform.localScale -= stepCrosshair * Time.deltaTime;
        else
            crosshair.rectTransform.localScale = normalCrosshair;
    }

}
