using UnityEngine;

public class DashTrail : MonoBehaviour
{
    public float trailDuration = 0.1f;
    public float damageAmount = 20f;
    public LayerMask enemyLayer;

    public void Initialize(Vector3 start, Vector3 end)
    {
        Vector3 center = (start + end) / 2f;
        transform.position = center;

        Vector3 direction = end - start;
        float length = direction.magnitude;
        transform.rotation = Quaternion.LookRotation(direction);
        transform.localScale = new Vector3(1f, 1f, length);

        // BoxCast yerine OverlapBox da kullanýlabilir
        Collider[] hits = Physics.OverlapBox(center, new Vector3(0.5f, 1f, length / 2), transform.rotation, enemyLayer);
        foreach (var hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
        }

        Destroy(gameObject, trailDuration);
    }
}
