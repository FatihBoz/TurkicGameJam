using UnityEngine;

public class CameraMovementBasic : MonoBehaviour
{
    public Transform target; // Takip edilecek karakter
    public float smoothSpeed = 0.125f; // Kamera hareketinin yumu�akl���
    public Vector3 offset; // Kamera ile hedef aras�ndaki mesafe

    void LateUpdate()
    {
        // Hedefin pozisyonu ile offseti kullanarak kamera konumunu belirle
        Vector3 desiredPosition = target.position + offset;

        // Kameray� hedef pozisyona do�ru yumu�ak�a hareket ettir
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Kameray� yeni pozisyona yerle�tir
        transform.position = smoothedPosition;

        // Kameray� hedefe odakla
        transform.LookAt(target);
    }
}
