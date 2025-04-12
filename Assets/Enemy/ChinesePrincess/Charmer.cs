using UnityEngine;

public class Charmer : MonoBehaviour
{
    public Transform player;

    public float radius = 5f;
    public float pullForce = 5f;
    public LayerMask playerLayer;

    Rigidbody rb;

    private void Awake()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(5f > Vector3.Distance(player.transform.position, transform.position))
        {

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
