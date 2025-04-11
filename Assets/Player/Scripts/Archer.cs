using UnityEngine;

public class Archer : MonoBehaviour
{
    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private Transform shootPoint;
    public void ArrowInstantiateAnimationMethod()
    {
        Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
    }
}
