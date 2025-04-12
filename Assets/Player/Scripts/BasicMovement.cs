using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private Archer archer;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 horizontalVelocity = Vector3.zero;


    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private int maxJumps = 2;
    private int currentJumps;
    private bool isGrounded;

    private bool canMove = true;

    private bool canGetInput = true;



    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        archer = GetComponent<Archer>();



    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.UI.Enable();
        if (archer != null)
        {
            inputActions.Player.Attack.performed += ctx => archer.Attack(this);
        }
        inputActions.Player.Jump.performed += ctx => Jump();

        if (archer != null)
        {
            archer.Initialize(this);
        }
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.UI.Disable();
    }


    public void PlayAnimation(string animName)
    {
        animator.SetTrigger(animName);
    }

    public void PlayAnimation(string animName, bool value)
    {

        animator.SetBool(animName, value);
    }


    private void FixedUpdate()
    {
        CheckIfGrounded();

        Rotate(horizontalVelocity);

        if (!canMove)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y,0);
            animator.SetBool("isMoving", rb.linearVelocity.x != 0 || rb.linearVelocity.z != 0);
            return;
        }

        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
        {
            
            horizontalVelocity = new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y, moveInput.y * moveSpeed);
            rb.linearVelocity = horizontalVelocity;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
        
        animator.SetBool("isMoving", rb.linearVelocity.x != 0 || rb.linearVelocity.z != 0);

    }

    public void Rotate(Vector3 dir)
    {
        Vector2 mousePos = inputActions.UI.Point.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y));

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f;
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                rb.MoveRotation(targetRotation);
            }
        }
    }
    

    private void Jump()
    {
        if (currentJumps < maxJumps)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            currentJumps++;
        }
    }

    private void CheckIfGrounded()
    {
        isGrounded = Physics.Raycast(transform.position+Vector3.up*1f, Vector3.down, 2f);
        if (isGrounded)
        {
            currentJumps = 0;
        }
    }

    public InputSystem_Actions GetInputActions()
    {
        return inputActions;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void SetCanGetInput(bool value)
    {
        canGetInput = value;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }
}
