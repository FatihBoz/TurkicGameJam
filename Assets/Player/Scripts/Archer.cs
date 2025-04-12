using UnityEngine;

public class Archer : MonoBehaviour
{
    public static Archer Instance { get; private set; }

    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask groundLayer;

    private bool attackingBuffer = true;
    private BasicMovement playerMovement;
    public bool isCasting = false;


    public void Initialize(BasicMovement movement)
    {
        Instance = this;
        playerMovement = movement;
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
