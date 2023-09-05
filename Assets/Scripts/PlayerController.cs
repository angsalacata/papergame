using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed; //control speed of character
    private float currentVelocity;
    [SerializeField] private float smoothTime = .05f;
    private Vector2 move; //store vector values from input
    private Vector3 direction;

 private CharacterController characterController;

// boolean to set walking animation condition
private static readonly int WALK_PROPERTY = Animator.StringToHash("Walking");



 [Header("Relations")]
    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private SpriteRenderer spriteRenderer = null;
private float gravity = -9.81f;
[SerializeField] private float gravityMultiplier = 3.0f;
private float velocity = -1.0f;
[SerializeField] private float jumpForce;
    public void OnMove(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
        direction = new Vector3(move.x, 0.0f, move.y);

        // moving right 
        if (move.x > 0){
            spriteRenderer.flipX = false;
        }

        // moving left
        else if (move.x < 0){
            spriteRenderer.flipX = true;
        }

    }
    // Start is called before the first frame update

    private void Awake(){
        characterController = GetComponent<CharacterController>();
        
    }

    void Start(){
        velocity = -1.0f;
    }


    // Update is called once per frame
    void Update()
    {
        // ApplyGravity();
        movePlayer();
    }

    public void movePlayer(){
        
        if (move.sqrMagnitude == 0) {
            animator.SetBool(WALK_PROPERTY, false);
            return;
        }
        var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
        // transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        characterController.Move(direction *speed * Time.deltaTime);
        animator.SetBool(WALK_PROPERTY, true);
    }

    public void onJump(InputAction.CallbackContext context){
        if (!context.started){
            return;
        }
        if (!IsGrounded()){
            return;
        }

        velocity += jumpForce;

    }

    private void ApplyGravity(){
        if (IsGrounded() && velocity < 0.0f){
            velocity = -1.0f;
        } 

        else {
            velocity += gravity *gravityMultiplier * Time.deltaTime;
        }

            direction.y = velocity;
            characterController.Move(direction *speed * Time.deltaTime);
        
    }

    private bool IsGrounded() => characterController.isGrounded;
}
