using UnityEngine;

public class Charmer : MonoBehaviour
{
    public Transform player;
    public float radius = 5f;
    public float pullForce = 5f;
    public LayerMask playerLayer;

    // Yeni: D�nme h�z� (saniyede derece cinsinden)
    public float rotationSpeed = 90f;

    // Yeni: Hasar verme mesafesi ve hasar ayarlar�
    public float damageDistance = 2f;
    public float damagePerSecond = 2f;
    private float lastDamageTime;

    Rigidbody rb;

    private void Awake()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Yeni: Karakterin kendi etraf�nda d�nmesi (Y ekseninde)
        transform.Rotate(0, rotationSpeed * Time.fixedDeltaTime, 0);

        // Oyuncu ile mesafe kontrol�
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer < radius)
        {
            if (rb != null)
            {
                // �ekim kuvveti uygulama
                Vector3 direction = new Vector3(transform.position.x - rb.position.x, 0, transform.position.z - rb.position.z).normalized;
                rb.AddForce(direction * pullForce, ForceMode.Force);
            }

            // Yeni: Hasar mesafesi kontrol� ve hasar verme
            if (distanceToPlayer < damageDistance)
            {
                // Oyuncunun sa�l�k bile�enine eri�im (varsay�lan bir PlayerHealth s�n�f� oldu�unu varsay�yorum)
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null && Time.time - lastDamageTime >= 1f)
                {
                    playerHealth.TakeDamage(damagePerSecond);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // �ekim alan� g�rselle�tirme
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);

        // Yeni: Hasar mesafesi g�rselle�tirme
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageDistance);
    }
}