using UnityEngine;

public class CameraMovementBasic : MonoBehaviour
{
    public Transform target; // Takip edilecek karakter
    public float smoothSpeed = 0.125f; // Kamera hareketinin yumuþaklýðý
    public Vector3 offset; // Kamera ile hedef arasýndaki mesafe

    void LateUpdate()
    {
        // Hedefin pozisyonu ile offseti kullanarak kamera konumunu belirle
        Vector3 desiredPosition = target.position + offset;

        // Kamerayý hedef pozisyona doðru yumuþakça hareket ettir
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Kamerayý yeni pozisyona yerleþtir
        transform.position = smoothedPosition;

        // Kamerayý hedefe odakla
        transform.LookAt(target);
    }
}
