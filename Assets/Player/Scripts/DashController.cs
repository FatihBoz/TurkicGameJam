using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DashController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private BasicMovement basicMovement;


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

    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        rb = GetComponent<Rigidbody>();

        basicMovement = GetComponent<BasicMovement>();
    }

    private void OnEnable()
    {
        inputActions.Player.Dash.performed += ctx => StartCoroutine(DashCoroutine());


        inputActions.Player.Enable();

    }

    private void OnDisable()
    {

        inputActions.Player.Disable();
    }



    IEnumerator DashCoroutine()
    {
        print("Dashing");

        basicMovement.SetCanMove(false);

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
        basicMovement.SetCanMove(true);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }

    void CreateDashTrail(Vector3 start, Vector3 end)
    {
        DashTrail trail = Instantiate(dashTrailPrefab, new Vector3(0, start.y+20, 0), Quaternion.identity);
        trail.Initialize(start, end);
    }
}
