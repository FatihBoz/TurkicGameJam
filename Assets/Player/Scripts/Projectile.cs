using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 1.5f;

    float elapsedTimeAfterInstantiated = 0f;

    private void Update()
    {
        elapsedTimeAfterInstantiated += Time.deltaTime;

        if(elapsedTimeAfterInstantiated >= lifetime)
        {
            Destroy(gameObject);
        }

        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }
}
