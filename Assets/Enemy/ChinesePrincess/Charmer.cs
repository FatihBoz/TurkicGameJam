using UnityEngine;

public class Charmer : MonoBehaviour
{
    public float radius = 5f;
    public float pullForce = 5f;
    public LayerMask playerLayer;

    void FixedUpdate()
    {
        // Belirli bir yarýçapta collider'larý bul
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, playerLayer);

        foreach (Collider col in colliders)
        {
            print("Collider: " + col.name);
            Rigidbody rb = col.attachedRigidbody;

            if (rb != null)
            {
                print("Rigidbody: " + rb.name);
                // Bu scriptin baðlý olduðu objeye doðru yön
                Vector3 direction = (transform.position - rb.position).normalized;

                // Hafif bir çekim kuvveti uygula
                rb.AddForce(direction * pullForce, ForceMode.Force); // daha güçlüdür

                Debug.Log("Applied force magnitude: " + (direction*pullForce).magnitude);

            }
        }
    }

    // Geliþtirici kolaylýðý için sahnede alaný göster
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
