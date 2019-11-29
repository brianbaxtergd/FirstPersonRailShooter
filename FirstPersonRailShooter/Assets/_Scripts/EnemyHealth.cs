using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Inscribed")]
    public int health;

    void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int _amount)
    {
        health = Mathf.Max(health - _amount, 0);
        if (health == 0)
            Die();
    }
}
