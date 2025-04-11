using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector3 moveInput;
    private Rigidbody rb;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private bool canDash = true;
    private Vector3 dashDirection;

    [Header("References")]
    public DashTrail dashTrailPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isDashing)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            moveInput = new Vector3(h, 0f, v).normalized;

            if (moveInput.magnitude > 0.1f)
                transform.forward = moveInput;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y, moveInput.z * moveSpeed);
        }
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        canDash = false;

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);


        dashDirection = moveInput == Vector3.zero ? transform.forward : moveInput;
        Vector3 start = transform.position;

        float timer = 0f;

        while (timer < dashDuration)
        {
            rb.linearVelocity = dashDirection * dashForce;
            timer += Time.deltaTime;
            yield return null;
        }

        Vector3 end = transform.position;
        rb.linearVelocity = Vector3.zero;

        CreateDashTrail(start, end);

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void CreateDashTrail(Vector3 start, Vector3 end)
    {
        DashTrail trail = Instantiate(dashTrailPrefab, Vector3.zero, Quaternion.identity);
        trail.Initialize(start, end);
    }
}
