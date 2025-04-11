using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private Animator animator;
    private Rigidbody rb;

    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
        {
            Vector3 moveDirection = new(moveInput.x, 0f, moveInput.y);
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }
}
