using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CharMove : MonoBehaviour
{
    public bool isSideScroll;
    public Image healtBar;
    public bool isDead;

    public float life = 100f;
    public float speed = 2;
    public int attackDamage = 20;

    public float jumppower = 5;
    public float gravity;
    public float smoothTime = 1f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public Vector3 direction;

    private CharacterController characterController;
    private float currentGravity = 0f;
    private Animator animator;
    private float currentVelocity;

    private float currentLife;
    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;
    private bool attacked = false;

    // Start is called before the first frame update
    void Start()
    {
       animator = GetComponent<Animator>();
       characterController = GetComponent<CharacterController>();
        isDead= false;
        currentLife = life;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        attacked = context.action.triggered; 
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 finalMovement = Movement() + ApplyGravity();
        characterController.Move(finalMovement * Time.deltaTime);

        //Attack
        if(attacked) {
            Attack();
        }
        //Jump
        if (jumped && characterController.isGrounded)
        {
            currentGravity -= jumppower;
        }
    }

    Vector3 ApplyGravity()
    {
        Vector3 gravityMovement = new Vector3(0, -currentGravity,0);
        currentGravity += gravity * Time.deltaTime;
        
        if (characterController.isGrounded) 
        {
            
            if (currentGravity > 1f)
            {
                currentGravity = 1f;
            }
            
        }

        return gravityMovement;
    }
    Vector3 Movement()
    {
        //Movement
        Vector3 moveVector = Vector3.zero;
        Vector3 moveReturn = Vector3.zero;

        if (isDead == true)
        {
            return moveVector;
        }
        if (isSideScroll == false)
        {
            moveVector += Vector3.forward * movementInput.y;
        }
        
        moveVector += Vector3.right * movementInput.x;
        moveVector = Vector3.ClampMagnitude(moveVector, 1f);
        moveVector *= speed;
        
        direction = moveVector.normalized;
        

        direction *= speed;
        bool isMoving = false;
        if (moveVector.magnitude > 0)
        {
            isMoving = true;
        }
  
        //Rotation
        
        if(direction.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,ref currentVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle,0); 
        }

        //animations
        if (isMoving && characterController.isGrounded)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        
        return moveVector;
    }
    void Attack()
    {
        if (isDead == true)
        {
            return;
        }

        //animation
        animator.SetTrigger("isAttacking2");

        //trigger
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage, transform.forward);
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void GetDamage(int damage)
    {
        currentLife -= damage;
        healtBar.fillAmount = currentLife / life;
        if (currentLife <= 0)
        {
            isDead = true;
        }
        if (isDead ==true)
        {
            animator.SetBool("isDead", true);
            return;
        }
        animator.SetTrigger("getHit");
    }

    
}
