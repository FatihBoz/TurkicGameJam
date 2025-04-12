using System.Threading.Tasks;
using UnityEngine;

public class SerpentArrowCaster : MonoBehaviour
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

    private Archer archer;

    public void Initialize(Archer archer)
    {
        this.archer = archer;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // test i�in
        {
            print("Pye bas�ld�");
            CastSkill();
        }
    }

    private async void CastSkill()
    {
        SkillVisual tempObj = Instantiate(skillEffect, archer.GetShootPoint().position, Quaternion.identity);
        tempObj.transform.SetParent(archer.GetShootPoint().parent);
        tempObj.Initialize(1f); 
        await Task.Delay(1000);
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

            Vector3 tempDir = new(transform.position.x, transform.position.y + 1f, transform.position.z);//rayler �ok a�a��da kal�yor

            if (Physics.Raycast(tempDir, direction, out RaycastHit hit, rayDistance, enemyLayer))
            {
                float offSetAngle = 20f * currentProjectiles;
                if(currentProjectiles % 2 == 0)
                {
                    offSetAngle *= -1;   // Bir sa�a bir sola mermi atmak i�in
                }

                Vector3 dir = Quaternion.Euler(0,offSetAngle,0) * transform.forward;

                SerpentArrow tempProjectile = Instantiate(projectilePrefab, archer.GetShootPoint().position, archer.GetShootPoint().rotation);
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

