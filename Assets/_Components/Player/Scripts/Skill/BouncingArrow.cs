using UnityEngine;

public class BouncingArrow : Projectile
{
    [SerializeField] private float bounceRadius;
    [SerializeField] private LayerMask bounceLayer;
    [SerializeField] private float maxBounce = 5;
    bool canFollow = true;
    private Transform target;
    private Collider currentEnemy;
    private int bounceCount = 0;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    Collider DetermineNextTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, bounceRadius,bounceLayer);
        Collider closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (Collider enemy in enemies)
        {
            if(enemy == currentEnemy)
                continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        if (closestEnemy != null)
        {
            print(closestEnemy.name);
        }
        return closestEnemy;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.TryGetComponent(out IDamageReceiver damageReceiver))
        {
            currentEnemy = collision;
            ScreenShake.Instance.Shake(1.2f, 0.3f);
            Destroy(Instantiate(hitEffect, transform.position, Quaternion.identity), 1f);
            damageReceiver.TakeDamage(damage);
            canFollow = false;
            if (DetermineNextTarget() != null && bounceCount < maxBounce)
            {
                target = DetermineNextTarget().transform;
                canFollow = true;
                bounceCount++;
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        return;
    }

    private Vector3 DetermineMoveDirection()
    {
        if(target == null)
            return transform.forward;

        return (target.transform.position - transform.position).normalized;

    }

    protected override void Update()
    {
        if (canFollow)
        {
            rb.linearVelocity = speed * DetermineMoveDirection();
        }

        
    }

}
