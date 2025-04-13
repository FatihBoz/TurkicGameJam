using System.Threading.Tasks;
using UnityEngine;

public class SerpentArrowCaster : Skill
{
    [Header("Projectile")]
    [SerializeField] private SerpentArrow projectilePrefab;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private SkillVisual skillEffect;
    [SerializeField] private int maxProjectiles = 3;

    [Header("Rays")]
    [SerializeField] int rayCount = 7;
    [SerializeField] float coneAngle = 60f;
    [SerializeField] float rayDistance = 10f;

    float elapsedTime = 0f;
    private void Update()
    {
        if (locked)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.E) && !Archer.Instance.isCasting && !isOnCooldown)
        {
            Archer.Instance.SetCasting(true);
            Archer.Instance.SetAttackingBuffer(false);
            Archer.Instance.isCasting = true;
            CastSkill();
            isOnCooldown = true;
            skillBar.Cooldown(cooldownTime);
        }


        if (isOnCooldown)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= cooldownTime)
            {
                elapsedTime = 0f;
                isOnCooldown = false;
            }
        }
    }

    private async void CastSkill()
    {
        print("skill cast");
        //COROUTINE YAZILACAK
        SkillVisual tempObj = Instantiate(skillEffect, Archer.Instance.GetShootPoint().position, Quaternion.identity);
        tempObj.transform.SetParent(Archer.Instance.GetShootPoint().parent);
        tempObj.Initialize(1f); 
        await Task.Delay(1000);
        Archer.Instance.SetCasting(false);
        Archer.Instance.SetAttackingBuffer(true);
        Archer.Instance.isCasting = false;
        CastConeRays();
        Destroy(tempObj);
    }

    void CastConeRays()
    {
        float halfAngle = coneAngle / 2f;
        int currentProjectiles = 0; 

        bool scrennShake = true;

        for (int i = 0; i < rayCount; i++)
        {

            float t = (float)i / (rayCount - 1);
            float angle = Mathf.Lerp(-halfAngle, halfAngle, t);

            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            Vector3 tempDir = new(transform.position.x, Archer.Instance.GetShootPoint().position.y, transform.position.z);//rayler �ok a�a��da kal�yor;
            Debug.DrawRay(tempDir, direction * rayDistance, Color.red, 1f); 
            if (Physics.Raycast(tempDir, direction, out RaycastHit hit, rayDistance, enemyLayer))
            {
                if(scrennShake)
                {
                    ScreenShake.Instance.Shake(1.5f, 0.4f);
                    scrennShake = false;
                }
                print("Hit: " + hit.transform.name);
                float offSetAngle = 20f * currentProjectiles;
                if (currentProjectiles % 2 == 0)
                {
                    offSetAngle *= -1;// Bir sa�a bir sola mermi atmak i�in
                    
                }

                Vector3 dir = Quaternion.Euler(0, offSetAngle, 0) * Vector3.forward;

                SerpentArrow tempProjectile = Instantiate(projectilePrefab, Archer.Instance.GetShootPoint().position, Archer.Instance.GetShootPoint().rotation  );
                tempProjectile.SetTarget(hit.transform);
                print("DIR:" + dir);
                tempProjectile.transform.rotation = Quaternion.LookRotation(dir);

                currentProjectiles++;
                if(currentProjectiles >= maxProjectiles)
                {
                    break;
                }
            }

        }
    }

}

