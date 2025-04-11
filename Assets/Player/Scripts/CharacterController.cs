using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private Archer archer;
    private Animator animator;
    private Rigidbody rb;

    private bool canMove = true;    

    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        archer = GetComponent<Archer>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += ctx => archer.Attack(this);
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }


    public void PlayAnimation(string animName)
    {
        animator.SetTrigger(animName);
    }



    private void FixedUpdate()
    {
        animator.SetBool("isMoving", rb.linearVelocity != Vector3.zero);

        if (!canMove)
        {
            return;
        }

        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        

        if (moveInput != Vector2.zero)
        {
            
            Vector3 moveDirection = new(moveInput.x, 0f, moveInput.y);
            rb.linearVelocity = moveDirection * moveSpeed;
        }

        
    }


    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
