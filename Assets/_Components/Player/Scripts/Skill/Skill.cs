using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldownTime = 8f;
    protected bool isOnCooldown = false;
    [SerializeField] protected SkillBar skillBar;


    public bool locked;
    void Awake()
    {
        locked=true;
    }
    public void UnlockSkill()
    {
        locked=false;
        skillBar.Unlock();
    }
    public SkillBar GetSkillBar()
    {
        return skillBar;
    }

    public void SetSkillBar(SkillBar skillBar)
    {
        this.skillBar = skillBar;
    }

}
