using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private Archer archer;
    private Animator animator;
    private Rigidbody rb;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private int maxJumps = 2;
    private int currentJumps;
    private bool isGrounded;

    private Ray ray;
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
        inputActions.UI.Enable();
        if (archer != null)
        {
            inputActions.Player.Attack.performed += ctx => archer.Attack(this);
        }
        inputActions.Player.Jump.performed += ctx => Jump();
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



    private void FixedUpdate()
    {
        animator.SetBool("isMoving", rb.linearVelocity != Vector3.zero);

        if (!canMove)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();



        if (moveInput != Vector2.zero)
        {

            Vector3 moveDirection = new(moveInput.x, 0f, moveInput.y);
            rb.linearVelocity = moveDirection * moveSpeed;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(targetRotation);
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
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f); // Yere yak�nsa grounded
        if (isGrounded)
        {
            currentJumps = 0; // Zeminle temasta iken z�plama say�s�n� s�f�rla
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
}
