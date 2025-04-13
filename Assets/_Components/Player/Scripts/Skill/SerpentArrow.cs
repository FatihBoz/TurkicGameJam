using System.Collections;
using UnityEngine;

public class SerpentArrow : Projectile
{
    [SerializeField] private float waitingTimeUntilFollow = 0.5f;
    private float elapsedTimeAfterInstantiate = 0f;
    private Transform target;
    private bool canFollow = false;

    public void SetTarget(Transform target)
    {
        this.target = target;
        print("Target set to: " + target.name);
        StartCoroutine(FollowTargetCoroutine());
    }

    private IEnumerator FollowTargetCoroutine()
    {
        yield return new WaitForSeconds(waitingTimeUntilFollow);
        canFollow = true;
    }

    protected override void Update()
    {
        
        
        elapsedTimeAfterInstantiate += Time.deltaTime;

        if (elapsedTimeAfterInstantiate >= lifetime)
        {
            Destroy(gameObject);
        }

        if (target != null && canFollow)
        {
            transform.Translate(speed * Time.deltaTime * CalculateDirection());
        }
    }

    Vector3 CalculateDirection()
    {
        return (target.position - transform.position).normalized;
    }
}
