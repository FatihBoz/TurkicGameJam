using UnityEngine;

public class Archer : MonoBehaviour
{
    public static Archer Instance { get; private set; }

    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask groundLayer;

    private bool attackingBuffer = true;
    private BasicMovement playerMovement;
    private BouncingArrow bouncingArrowPrefab;
    public bool isCasting = false;


    public void Initialize(BasicMovement movement)
    {
        Instance = this;
        playerMovement = movement;
    }



    public void ArrowInstantiateAnimationMethod()
    {
        Instantiate(arrowPrefab, shootPoint.position, transform.rotation);
        RestrainPlayer(false);
    }

    public void BouncingArrowInstantiateAnimationMethod()
    {
        Instantiate(bouncingArrowPrefab, shootPoint.position, shootPoint.rotation);
        RestrainPlayer(false);
    }

    public void InstantiateBouncingArrow(BouncingArrow arrowPrefab)
    {
        bouncingArrowPrefab = arrowPrefab;
        playerMovement.PlayAnimation("BouncingArrowAttack");
        RestrainPlayer(true);
    }

    public void Attack(BasicMovement movement)
    {
        if (attackingBuffer && !isCasting)
        {
            playerMovement = movement;
            movement.PlayAnimation("isMoving", false);
            playerMovement.PlayAnimation("ArrowAttack");

            RestrainPlayer(true);
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

    void RestrainPlayer(bool restrain)
    {
        if (playerMovement != null)
        {
            playerMovement.SetCanMove(!restrain);
        }
        attackingBuffer = !restrain;
        isCasting = restrain;
    }

}
