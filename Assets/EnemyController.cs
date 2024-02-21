using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour, Idamageable
{
    public Transform player;
    public float stoppingDistance = 5f;
    public float moveSpeed = 3f;
    public float attackDelay = 2f;

    private bool isAttacking = false;
    private Vector3 initialPosition;
    private Animator animator;
    // healthbar
    public Slider slider;
    public Image fillImage;
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public Color fullHealthColor = Color.green;
    public Color zeroHealthColor = Color.red;
    public float EnemyDamage = 10f;
    private bool isAlive = true;

    void Start()
    {
        initialPosition = transform.position;
        animator = GetComponent<Animator>();
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        fillImage.color = fullHealthColor;
    }

    void Update()
    {
        if (!isAlive) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            if (distanceToPlayer > stoppingDistance)
            {
                MoveTowardsPlayer();
            }
            else
            {
                StopMoving();
                StartCoroutine(Attack());
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
        newPosition.y = initialPosition.y; // Maintain initial y-position
        transform.position = newPosition;
        transform.LookAt(player);
    }

    void StopMoving()
    {
        // Optional: Stop movement or perform other actions
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        // Attack animation or action
        Debug.Log("Enemy is attacking!");
        animator.SetTrigger("IsAttacking");

        yield return new WaitForSeconds(attackDelay);

        // Damage the player
        ThirdPersonController.HitPoints -= EnemyDamage;

        isAttacking = false;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0f);
        slider.value = currentHealth;

        // Change color based on health
        float healthPercent = currentHealth / maxHealth;
        fillImage.color = Color.Lerp(zeroHealthColor, fullHealthColor, healthPercent);

        // If health reaches 0, perform any necessary actions (e.g., enemy dies)
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy has died.");
        isAlive = false;
        animator.SetTrigger("IsDead");

        Destroy(gameObject, 5f); // Destroy the enemy object after 1 second
    }

    public void DamageAmount(float damageAmount)
    {
        TakeDamage(damageAmount);
    }
}
