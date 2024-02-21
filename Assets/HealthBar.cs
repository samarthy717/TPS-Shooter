using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fillImage;
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public Color fullHealthColor = Color.green;
    public Color zeroHealthColor = Color.red;

    void Start()
    {
        slider.maxValue = ThirdPersonController.HitPoints;
        slider.value = currentHealth;
        fillImage.color = fullHealthColor;
    }

    void Update()
    {
            TakeDamage();     
    }

    public void TakeDamage()
    {
        slider.value = ThirdPersonController.HitPoints;
        // Change color based on health
        float healthPercent = currentHealth / maxHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, healthPercent);

        // Ensure health doesn't go below 0
        currentHealth = Mathf.Max(currentHealth, 0f);

        // If health reaches 0, perform any necessary actions (e.g., player dies)
        if (currentHealth <= 0f)
        {
            // Perform actions when health reaches 0
            Debug.Log("Player has died.");
        }
    }
}
