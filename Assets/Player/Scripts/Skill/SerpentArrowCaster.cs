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

        for (int i = 0; i < rayCount; i++)
        {

            float t = (float)i / (rayCount - 1);
            float angle = Mathf.Lerp(-halfAngle, halfAngle, t);

            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            Vector3 tempDir = new(transform.position.x, transform.position.y + 1f, transform.position.z);//rayler çok aþaðýda kalýyor

            if (Physics.Raycast(tempDir, direction, out RaycastHit hit, rayDistance, enemyLayer))
            {
                float offSetAngle = 20f * currentProjectiles;
                if(currentProjectiles % 2 == 0)
                {
                    offSetAngle *= -1;   // Bir saða bir sola mermi atmak için
                }

                Vector3 dir = Quaternion.Euler(0,offSetAngle,0) * transform.forward;

                SerpentArrow tempProjectile = Instantiate(projectilePrefab, Archer.Instance.GetShootPoint().position, Archer.Instance.GetShootPoint().rotation);
                tempProjectile.SetTarget(hit.transform);
                tempProjectile.transform.rotation = Quaternion.LookRotation(dir);

                ++currentProjectiles;
                if(currentProjectiles >= maxProjectiles)
                {
                    break;
                }
            }

        }
    }

}

