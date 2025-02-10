using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public bool isPlayerOne = true;
    public Slider healthBar;  // Assign this in the Inspector
    public float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bot")) // Assuming both bots have the "Bot" tag
        {
            TakeDamage(10f); // Reduce health by 10 on collision
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent negative health
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Destroy bot when health reaches 0
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }
}
