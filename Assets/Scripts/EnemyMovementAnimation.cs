using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class EnemyMovementAnimation : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    private NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    private SpriteRenderer spriteRenderer = null;


    void Start ()
    {
        anim = GetComponent<Animator> ();
        // Don’t update position automatically
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
        float horizontalVel = agent.velocity.x;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot (transform.right, worldDeltaPosition);
        float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2 (dx, dy);
        

        // Debug.Log(dx);
        // Debug.Log(dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime/0.15f);
        smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldWalk = horizontalVel != 0;

        //enemy should face right when moving
        if (horizontalVel > 0){
            spriteRenderer.flipX = false;
        }
        //enemy should face left when moving
        else if (horizontalVel < 0){
            spriteRenderer.flipX = true;
        }
        // Update animation parameters
        anim.SetBool("IsWalking", shouldWalk);
    }


     void OnAnimatorMove ()
    {
        // Update position to agent position
        // transform.position = agent.nextPosition;
    }
}
