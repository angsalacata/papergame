using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{

    public Transform targetTransform; // player transform

    [SerializeField] private LayerMask groundLayer, playerLayer;
    public float updateSpeed = .1f;


    private NavMeshAgent enemyNavMeshAgent;
    // Start is called before the first frame update

    // patrolling movement
    [SerializeField] float range; // range of patrolling
    Vector3 destination;  // where patrolling enemy walks to
    bool walkPointSet; // does the enemy know where to walk towards?


    // tracking player and attacking player
    private GameObject player;
    [SerializeField] float playerSightRange, attackRange;
    [SerializeField] bool playerInSight, playerInAttackRange;

    private Transform enemyAttackPoint;

    [SerializeField] float enemyAttackRate = 2f;
    float nextEnemyAttackTime = 0f;

    [SerializeField] int attackDamage = 1;

    private Animator animator = null;


    private void Awake()
    {
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Character"); // find the Player Character in Hierarchy 
        animator = transform.Find("Sprite").GetComponent<Animator>(); // get the animator for the sprite renderer for enemy

    }
    void Start()
    {
        // StartCoroutine(FollowPlayer());
        // enemyAttackPoint = transform.Find("EnemyAttackPoint");
        Debug.Log(transform.Find("EnemyAttackPoint"));
        enemyAttackPoint = transform.Find("EnemyAttackPoint");


    }

    // Update is called once per frame
    void Update()
    {
        float horizontalVel = enemyNavMeshAgent.velocity.x;


        //attack point face right 
        if (horizontalVel > 0)
        {
            enemyAttackPoint.localPosition = new Vector3(1, 0, 0);

        }
        //attack point face left 
        else if (horizontalVel < 0)
        {
            enemyAttackPoint.localPosition = new Vector3(-1, 0, 0);
        }
        playerInSight = Physics.CheckSphere(transform.position, playerSightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSight && !playerInAttackRange) Patrol();
        else if (playerInSight && !playerInAttackRange) Chase();
        else if (playerInSight && playerInAttackRange) AttackPlayer();
    }

    void Patrol()
    {
        if (!walkPointSet) LookForDestination();
        if (walkPointSet) enemyNavMeshAgent.SetDestination(destination);
        if (Vector3.Distance(transform.position, destination) < 10) walkPointSet = false;
    }

    void LookForDestination()
    {
        float zDest = Random.Range(-range, range);
        float xDest = Random.Range(-range, range);
        destination = new Vector3(transform.position.x + xDest, transform.position.y, transform.position.z + zDest);

        if (Physics.Raycast(destination, Vector3.down, 3, groundLayer))
        {
            walkPointSet = true;
        }

    }

    void Chase()
    {
        enemyNavMeshAgent.SetDestination(player.transform.position);
    }

    void AttackPlayer()
    {
        //Set Enemy Animation
        // first check if enemy is already in attack stage, if not trigger animation. if yes do nothing
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack") && Time.time >= nextEnemyAttackTime)
        {
            // set agent destination to current destination to stop movement.
                enemyNavMeshAgent.SetDestination(transform.position);
                animator.SetTrigger("Attack");
                Collider[] hitPlayer = Physics.OverlapSphere(enemyAttackPoint.position, attackRange, playerLayer);
                nextEnemyAttackTime = (Time.time + (1f / enemyAttackRate));
                foreach (Collider player in hitPlayer)
                {
                    Debug.Log("Attacking: " +player.name);
                    player.GetComponent<HealthAndInventory>().TakeDamage(attackDamage);
                }
            
        }
    }

    private IEnumerator FollowPlayer()
    {

        WaitForSeconds Wait = new WaitForSeconds(updateSpeed);
        while (enabled)
        {
            // if enemy enabled follow player
            enemyNavMeshAgent.SetDestination(targetTransform.transform.position);
            yield return Wait;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, playerSightRange);

    }
}
