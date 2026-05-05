using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    [SerializeField] private HealthSystem health;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        health.OnHealthChanged += updateHealth;

    }

    // Update is called once per frame
    void updateHealth(int current, int max)
    {
        float percent = (float)current / max;
        fillImage.fillAmount = Mathf.Lerp(max, percent, 10f);
    }
}
