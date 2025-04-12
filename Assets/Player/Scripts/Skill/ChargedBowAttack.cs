using System.Collections;
using UnityEngine;

public class ChargedBowAttack : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private SkillVisual chargeEffectPrefab;
    [SerializeField] private float chargeTime = 2f;
    [SerializeField] private float waitingTimeBetweenArrows = 0.25f;
    [SerializeField] private int maxArrows = 3;
    private Archer archer;
    private bool isCasting = false;

    public void Initialize(Archer archer)
    {
        this.archer = archer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !archer.IsCasting())
        {   
            archer.SetCasting(true);
            archer.SetAttackingBuffer(false);
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
        SkillVisual chargeObj = Instantiate(chargeEffectPrefab, archer.GetShootPoint().position, Quaternion.identity);
        chargeObj.transform.SetParent(archer.GetShootPoint().parent);
        chargeObj.Initialize(chargeTime);
        yield return new WaitForSeconds(chargeTime);
        Destroy(chargeObj);

        //--------------------------------SHOOT-----------------------------------  
        int arrowCount = 0;
        while (arrowCount < maxArrows)
        {
            Instantiate(projectilePrefab, archer.GetShootPoint().position, archer.GetShootPoint().rotation);
            arrowCount++;
            yield return new WaitForSeconds(waitingTimeBetweenArrows);
        }

        isCasting = false;
        archer.SetAttackingBuffer(true);
        archer.SetCasting(false);
        yield return null;
    }


}
