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
                // Bu scriptin baðlý olduðu objeye doðru yön
                Vector3 direction = new Vector3(transform.position.x - rb.position.x, 0, transform.position.z - rb.position.z);

                // Hafif bir çekim kuvveti uygula
                rb.AddForce(direction * pullForce, ForceMode.Force); // daha güçlüdür


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
