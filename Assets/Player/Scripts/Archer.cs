using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask groundLayer;

    private bool attackingBuffer = true;
    private BasicMovement playerMovement;
    private ChargedBowAttack chargedBowAttack;
    ConeRaycaster coneRaycaster;
    private bool isCasting = false;


    public void Initialize(BasicMovement movement)
    {
        playerMovement = movement;

        chargedBowAttack = GetComponent<ChargedBowAttack>();
        if (chargedBowAttack != null)
        {
            chargedBowAttack.Initialize(this);
        }

        coneRaycaster = GetComponent<ConeRaycaster>();
        if (coneRaycaster != null)
        {
            coneRaycaster.Initialize(this);
        }
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

    public Transform GetShootPoint()
    {
        return shootPoint;
    }

}
