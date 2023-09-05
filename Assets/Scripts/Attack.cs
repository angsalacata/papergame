using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{

    [Header("Relations")]
    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;

    [SerializeField] 
    private Transform attackPoint;
    [SerializeField] 
    private float attackRange;

    public LayerMask enemyLayers;

       [SerializeField] 
    private int attackDamage;


    public float attackRate = 2f;

    float nextAttackTime = 0;

    void Update()
    {
        
    }


    public void OnMeleeAttack(InputAction.CallbackContext context){

        if (!context.started){
            return;
        }

        if (Time.time >= nextAttackTime){
        animator.SetTrigger("Attack");
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

            nextAttackTime = (Time.time + (1f/attackRate));
            foreach (Collider enemy in hitEnemies){
            Debug.Log(enemy.name);
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
        }
    }


void OnDrawGizmosSelected() {
    if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
