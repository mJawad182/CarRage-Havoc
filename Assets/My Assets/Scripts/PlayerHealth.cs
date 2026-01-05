using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100;
    private float currentHealth;

    [Header("UI")]
    public Slider healthSlider;  // Assign your UI Slider in inspector

    void Start()
    {
        currentHealth = maxHealth; 
        UpdateHealthUI();
    }

    // Call this to reduce health by 'amount'
   

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0f);
        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }


    // Call this to increase max health and optionally heal fully
    public void IncreaseMaxHealth(int amount, bool healFully = true)
    {
        maxHealth += amount;
        if (healFully)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
        UpdateHealthUI();
    }

    // Call this to heal the player by 'amount'
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // Add your death logic here (disable controls, play animation, etc.)
    }
}
