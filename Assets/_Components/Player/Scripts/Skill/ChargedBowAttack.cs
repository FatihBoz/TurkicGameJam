using System.Collections;
using UnityEngine;

public class ChargedBowAttack : Skill
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private SkillVisual chargeEffectPrefab;
    [SerializeField] private float chargeTime = 2f;
    [SerializeField] private float waitingTimeBetweenArrows = 0.25f;
    [SerializeField] private int maxArrows = 3;
    private float elapsedTime = 0f;


    private void Update()
    {
        if (locked)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q) && !Archer.Instance.isCasting && !isOnCooldown)
        {
            Archer.Instance.SetCasting(true);
            Archer.Instance.SetAttackingBuffer(false);
            Archer.Instance.isCasting = true;
            Charge();
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

    void Charge()
    {
        StartCoroutine(ConsequentArrows());
    }


    private IEnumerator ConsequentArrows()
    {
        //-----------------------------------CHARGE-----------------------------------
        SkillVisual chargeObj = Instantiate(chargeEffectPrefab, Archer.Instance.GetShootPoint().position, Quaternion.identity);
        chargeObj.transform.SetParent(Archer.Instance.GetShootPoint().parent);
        chargeObj.Initialize(chargeTime);
        yield return new WaitForSeconds(chargeTime);
        Destroy(chargeObj);
        
        //--------------------------------SHOOT-----------------------------------  
        int arrowCount = 0;
        while (arrowCount < maxArrows)
        {
            ScreenShake.Instance.Shake(1f, 0.3f);
            Instantiate(projectilePrefab, Archer.Instance.GetShootPoint().position, Archer.Instance.GetShootPoint().rotation);
            arrowCount++;
            yield return new WaitForSeconds(waitingTimeBetweenArrows);
        }

        Archer.Instance.isCasting = false;
        Archer.Instance.SetAttackingBuffer(true);
        Archer.Instance.SetCasting(false);
        yield return null;
    }


}
