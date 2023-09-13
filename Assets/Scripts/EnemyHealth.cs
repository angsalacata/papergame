using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class EnemyHealth : MonoBehaviour
{
  
    [SerializeField] private int enemyMaxHealth;
   [SerializeField] private Animator animator = null;
   private Collider parentCollider;

   // boolean to set walking animation condition
private static readonly int WALK_PROPERTY = Animator.StringToHash("Walking");
    private int currentHealth;
    void Start()
    {
        // start with full health
        currentHealth = enemyMaxHealth;
        parentCollider = GetComponentInParent<Collider>();
        Debug.Log(parentCollider);
    }


    void Update(){
        
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        animator.SetTrigger("Hurt");
        
        if (currentHealth <= 0){
            // enemy dies
            StartCoroutine(Die());
        }
    }

    private IEnumerator  Die(){
        //die animation
        animator.SetBool("IsDead", true);
        parentCollider.enabled = false;
        yield return new WaitForSeconds(.65f); // currently frog dying animation
        //disable enemy
        Debug.Log("enemy died");
        Destroy(gameObject);
    }
}
