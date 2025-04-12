using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private Transform shootPoint;

    private bool attackingBuffer = true;
    private BasicMovement playerMovement;

    public void ArrowInstantiateAnimationMethod()
    {
        Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
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
            playerMovement = movement;
            playerMovement.PlayAnimation("ArrowAttack");
            playerMovement.SetCanMove(false);
            attackingBuffer = false;
        }



    }
}
