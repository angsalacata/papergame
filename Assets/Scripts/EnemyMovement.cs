using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{

    public Transform targetTransform;
    

    public float updateSpeed = .1f;

    private NavMeshAgent enemyNavMeshAgent;
    // Start is called before the first frame update

    private void Awake(){
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        StartCoroutine(FollowPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        
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
