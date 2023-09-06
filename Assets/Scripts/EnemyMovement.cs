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


    private void Awake(){
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        // StartCoroutine(FollowPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    void Patrol()
    {   if (!walkPointSet) LookForDestination();
        if (walkPointSet) enemyNavMeshAgent.SetDestination(destination);
        if (Vector3.Distance (transform.position, destination) < 10) walkPointSet = false;
    }

    void LookForDestination(){
        float zDest = Random.Range(-range, range);
        float xDest = Random.Range(-range, range);
        destination = new Vector3(transform.position.x + xDest, transform.position.y, transform.position.z + zDest);

        if (Physics.Raycast(destination, Vector3.down, 4,  groundLayer)){
            walkPointSet = true;
        }
        
    }

    private IEnumerator FollowPlayer(){

        WaitForSeconds Wait = new WaitForSeconds(updateSpeed);
        while (enabled){
            // if enemy enabled follow player
            enemyNavMeshAgent.SetDestination(targetTransform.transform.position);
            yield return Wait;
        }
    }
}
