using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask groundLayer;

    private bool attackingBuffer = true;
    private BasicMovement playerMovement;
    private ChargedBowAttack chargedBowAttack;
    private bool isCasting = false;


    public void Initialize(BasicMovement movement)
    {
        playerMovement = movement;

        chargedBowAttack = GetComponent<ChargedBowAttack>();
        if (chargedBowAttack != null)
        {
            chargedBowAttack.Initialize(this);
        }
    }


    private void FixedUpdate()
    {
        Rotate(playerMovement.GetHorizontalVelocity().normalized);

        if (isCasting)
        {
            Vector2 mousePos = playerMovement.GetInputActions().UI.Point.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y));

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                Vector3 lookDirection = hit.point - transform.position;
                lookDirection.y = 0f;
                if (lookDirection != Vector3.zero)
                {
                    Rotate(lookDirection.normalized);
                }
            }

        }
    }


    public void Rotate(Vector3 dir)
    {
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        playerMovement.GetRigidbody().MoveRotation(targetRotation);
    }


    public void ArrowInstantiateAnimationMethod()
    {
        Instantiate(arrowPrefab, shootPoint.position, transform.rotation);
        if(playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
        attackingBuffer = true;
        isCasting = false;
    }

    public void Attack(BasicMovement movement)
    {
        if (attackingBuffer && !isCasting)
        {
            isCasting = true;
            movement.SetCanMove(false);

            Vector2 mousePos = movement.GetInputActions().UI.Point.ReadValue<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y));


            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                Vector3 lookDirection = hit.point - transform.position;
                lookDirection.y = 0f;
                print(lookDirection);

                if (lookDirection != Vector3.zero)
                {
                    Rotate(lookDirection.normalized);
                }
            }
            movement.PlayAnimation("isMoving", false);
            playerMovement = movement;
            playerMovement.PlayAnimation("ArrowAttack");
            
            attackingBuffer = false;
        }
    }

    public void SetAttackingBuffer(bool buffer)
    {
        attackingBuffer = buffer;
        playerMovement.SetCanMove(buffer);
    }

    public bool IsCasting()
    {
        return isCasting;
    }

    public void SetCasting(bool casting)
    {
        isCasting = casting;
    }


}
