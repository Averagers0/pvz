using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{
    public PlantData plantData;

    public int currentHealth;

    public virtual void Start()
    {
        currentHealth = plantData.health;
    }

    public virtual void OnPlant()
    {
        Debug.Log($"{plantData.plantName} 被种下");
    }

    public abstract void SpecialAbility();

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
