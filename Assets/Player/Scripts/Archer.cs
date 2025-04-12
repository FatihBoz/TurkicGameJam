using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private Transform shootPoint;

    private PlayerMovement playerMovement;
    private bool attackingBuffer = true;
    public void ArrowInstantiateAnimationMethod()
    {
        Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        if(playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
        attackingBuffer = true;
    }

    public void Attack(PlayerMovement movement)
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
