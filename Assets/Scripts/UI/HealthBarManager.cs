using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private TMP_Text countHP;
    [SerializeField] private Image lineHP;

    private float maxHP;
    public void SetCurrentHP(float newHP)
    {
        lineHP.fillAmount = lineHP.fillAmount - (int.Parse(countHP.text) - newHP) / maxHP;
        countHP.text = newHP.ToString();
    }

    public void SetMaxHP(float maxHP)
    {
        this.maxHP = maxHP;
        lineHP.fillAmount = 1f;
        countHP.text = maxHP.ToString();
    }
}