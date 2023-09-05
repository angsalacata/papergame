using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
  
    [SerializeField] private int enemyMaxHealth;
   [SerializeField] private Animator animator = null;

   
    private int currentHealth;
    void Start()
    {
        // start with full health
        currentHealth = enemyMaxHealth;
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        
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
