using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected GameObject hitEffect;
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


    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Object"))
        {
            collision.gameObject.GetComponent<ObjectDestroy>().DestroyObject();
        }

        if (collision.gameObject.TryGetComponent(out IDamageReceiver damageReceiver))
        {
            ScreenShake.Instance.Shake(1.2f, 0.3f);
            Destroy(Instantiate(hitEffect, transform.position, Quaternion.identity), 1f);
            damageReceiver.TakeDamage(10f);
            Destroy(gameObject);
        }
    }
}
