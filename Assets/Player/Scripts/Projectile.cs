using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float lifetime = 1.5f;

    float elapsedTimeAfterInstantiated = 0f;


     protected virtual void Update()
    {
        elapsedTimeAfterInstantiated += Time.deltaTime;

        if(elapsedTimeAfterInstantiated >= lifetime)
        {
            Destroy(gameObject);
        }

        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }
}
