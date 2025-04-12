using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private Transform shootPoint;

    private BasicMovement playerMovement;
    public void ArrowInstantiateAnimationMethod()
    {
        Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        if(playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
    }

    public void Attack(BasicMovement movement)
    {
        print("attack");
        playerMovement = movement;
        playerMovement.PlayAnimation("ArrowAttack");
        playerMovement.SetCanMove(false);

    }
}
