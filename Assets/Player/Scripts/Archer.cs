using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    private List<Skill> skillList;

    [SerializeField] private Projectile arrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LayerMask groundLayer;

    private int maxSkillCount = 3;
    private bool attackingBuffer = true;
    private BasicMovement playerMovement;
    private ChargedBowAttack chargedBowAttack;
    SerpentArrowCaster coneRaycaster;
    private bool isCasting = false;


    public void Initialize(BasicMovement movement)
    {
        playerMovement = movement;
        skillList = new List<Skill>(maxSkillCount);

        chargedBowAttack = GetComponent<ChargedBowAttack>();
        if (chargedBowAttack != null)
        {
            chargedBowAttack.Initialize(this);
        }

        coneRaycaster = GetComponent<SerpentArrowCaster>();
        if (coneRaycaster != null)
        {
            coneRaycaster.Initialize(this);
        }
    }

    public void AssignSkill(Skill skill)
    {
        if (skillList.Count < maxSkillCount)
        {
            skillList.Add(skill);
            skill.Initialize(this);
        }
    }   



    public void ArrowInstantiateAnimationMethod()
    {
        Instantiate(arrowPrefab, shootPoint.position, transform.rotation);
        if(playerMovement != null)
        {
            playerMovement.SetCanMove(true);
        }
        attackingBuffer = true;
        isCasting = false;
    }

    public void Attack(BasicMovement movement)
    {
        if (attackingBuffer && !isCasting)
        {
            isCasting = true;
            movement.SetCanMove(false);
            movement.PlayAnimation("isMoving", false);
            playerMovement = movement;
            playerMovement.PlayAnimation("ArrowAttack");
            
            attackingBuffer = false;
        }
    }

    public void SetAttackingBuffer(bool buffer)
    {
        attackingBuffer = buffer;
        playerMovement.SetCanMove(buffer);
    }

    public bool IsCasting()
    {
        return isCasting;
    }

    public void SetCasting(bool casting)
    {
        isCasting = casting;
    }

    public Transform GetShootPoint()
    {
        return shootPoint;
    }

}
