using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private TMP_Text countHP;
    [SerializeField] private Image lineHP;

    private float maxHP;
    private bool isFirstSetHP = true;

    private void Awake()
    {
        GameManager.Instance.OnHPChange += SetCurrentHP;
    }

    private void SetCurrentHP(float newHP)
    {
        if (isFirstSetHP)
        {
            maxHP = newHP;
            isFirstSetHP = false;
            lineHP.fillAmount = 1;
        }
        else
        {
            lineHP.fillAmount = lineHP.fillAmount - (int.Parse(countHP.text) - newHP) / maxHP;
        }
        countHP.text = newHP.ToString();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnHPChange -= SetCurrentHP;
    }
}
