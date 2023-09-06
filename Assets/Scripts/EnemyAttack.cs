using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour

{


    public Transform rayCast;
    public LayerMask raycastMask;
    public float raycastLength;
    public float attackDistance;
    public float timer; //cool down for attacks


    private RaycastHit hit;
    private GameObject target;
    private Animator anim;

    private float distanceEnemyPlayer; //store distance between enemy and player

    private bool attackMode;
    private bool inRange;
    private bool isOnCooldown;

    private float initialTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
