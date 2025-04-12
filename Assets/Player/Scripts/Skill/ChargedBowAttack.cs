using System.Collections;
using UnityEngine;

public class ChargedBowAttack : Skill
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private SkillVisual chargeEffectPrefab;
    [SerializeField] private float chargeTime = 2f;
    [SerializeField] private float waitingTimeBetweenArrows = 0.25f;
    [SerializeField] private int maxArrows = 3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !Archer.Instance.isCasting)
        {
            Archer.Instance.SetCasting(true);
            Archer.Instance.SetAttackingBuffer(false);
            Archer.Instance.isCasting = true;
            Charge();
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
