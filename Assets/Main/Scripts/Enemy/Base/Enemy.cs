using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMoveable
{
    public float MaxHealth { get; set; }
    public float currentHealth { get; set; }
    public Rigidbody rb { get; set; }

    private void Start()
    {
        currentHealth = MaxHealth;
    }
    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Move(float velocity)
    {
        
    }
}
