using UnityEngine;
using UnityEngine.UIElements;

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

        Destroy(gameObject, trailDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger entered: " + other.gameObject.name);
        print("Trigger layer: " + other.gameObject.layer);
        print("Enemy layer mask: " + enemyLayer.value);

        if ((enemyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            print("Hit enemy: " + other.gameObject.name);
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                print("Damaging enemy: " + other.gameObject.name);
                enemyHealth.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }


}
