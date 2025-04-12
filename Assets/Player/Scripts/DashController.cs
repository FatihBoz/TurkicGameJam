using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DashController : MonoBehaviour
{
    private InputSystem_Actions inputActions;
    private Rigidbody rb;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Dash")]
    public float dashForce = 20f; // Kuvvet
    public float dashDuration = 0.2f; // Dash s�resi
    public float dashCooldown = 1f; // Dash sonras� bekleme s�resi
    private bool isDashing = false;
    private bool canDash = true;
    private Vector3 dashDirection;

    [Header("References")]
    public DashTrail dashTrailPrefab;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
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
        if (!canDash || isDashing) yield break;

        isDashing = true;
        canDash = false;

        // Dash s�ras�nda �arp��malar� engelle
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        if (moveInput != Vector2.zero)
        {
            dashDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        }
        else
        {
            dashDirection = transform.forward;
        }

        // Dash hareketini uygula
        float timer = 0f;
        Vector3 startPosition = transform.position;
        while (timer < dashDuration)
        {
            // Hareketi do�rudan uygula
            transform.position += dashDirection * dashForce * Time.deltaTime; // Kuvvetle de�il, mesafeyle ilerliyoruz

            timer += Time.deltaTime;
            yield return null;
        }

        Vector3 endPosition = transform.position;

        // Dash izi olu�tur
        CreateDashTrail(startPosition, endPosition);

        // �arp��ma engellemesini kald�r
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

        isDashing = false;

        // Dash sonras� cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void CreateDashTrail(Vector3 start, Vector3 end)
    {
        DashTrail trail = Instantiate(dashTrailPrefab, new Vector3(0, start.y + 20, 0), Quaternion.identity);
        trail.Initialize(start, end);
    }
}
