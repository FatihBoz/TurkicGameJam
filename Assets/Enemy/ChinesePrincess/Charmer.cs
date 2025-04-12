using UnityEngine;

public class Charmer : MonoBehaviour
{
    public float radius = 5f;
    public float pullForce = 5f;
    public LayerMask playerLayer;

    void FixedUpdate()
    {
        // Belirli bir yar��apta collider'lar� bul
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, playerLayer);

        foreach (Collider col in colliders)
        {
            print("Collider: " + col.name);
            Rigidbody rb = col.attachedRigidbody;

            if (rb != null)
            {
                print("Rigidbody: " + rb.name);
                // Bu scriptin ba�l� oldu�u objeye do�ru y�n
                Vector3 direction = (transform.position - rb.position).normalized;

                // Hafif bir �ekim kuvveti uygula
                rb.AddForce(direction * pullForce, ForceMode.Force); // daha g��l�d�r

                Debug.Log("Applied force magnitude: " + (direction*pullForce).magnitude);

            }
        }
    }

    // Geli�tirici kolayl��� i�in sahnede alan� g�ster
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
