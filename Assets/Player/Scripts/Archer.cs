using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask groundLayer;


    private bool attackingBuffer = true;
    private BasicMovement playerMovement;


    public void ArrowInstantiateAnimationMethod()
    {
        Instantiate(arrowPrefab, shootPoint.position, transform.rotation);
        if(playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
        attackingBuffer = true;
    }

    public void Attack(BasicMovement movement)
    {
        if (attackingBuffer)
        {

            Vector2 mousePos = movement.GetInputActions().UI.Point.ReadValue<Vector2>();

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y));


            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                Vector3 lookDirection = hit.point - transform.position;
                lookDirection.y = 0f;
                print(lookDirection);

                if (lookDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                    transform.rotation = targetRotation;
                }
            }

            playerMovement = movement;
            playerMovement.PlayAnimation("ArrowAttack");
            playerMovement.SetCanMove(false);
            attackingBuffer = false;
        }
    }

}
