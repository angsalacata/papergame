using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
  
    [SerializeField] private int enemyMaxHealth;

    private int currentHealth;
    void Start()
    {
        // start with full health
        currentHealth = enemyMaxHealth;
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;

        if (currentHealth <= 0){
            // enemy dies
            Die();
        }
    }

    void Die(){
        //die animation

        //disable enemy
        Debug.Log("enemy died");
        Destroy(gameObject);
    }
}
