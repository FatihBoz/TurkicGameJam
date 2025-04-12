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
            Rigidbody rb = col.attachedRigidbody;

            if (rb != null)
            {
                // Bu scriptin ba�l� oldu�u objeye do�ru y�n
                Vector3 direction = new Vector3(transform.position.x - rb.position.x, 0, transform.position.z - rb.position.z);

                // Hafif bir �ekim kuvveti uygula
                rb.AddForce(direction * pullForce, ForceMode.Force); // daha g��l�d�r


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
