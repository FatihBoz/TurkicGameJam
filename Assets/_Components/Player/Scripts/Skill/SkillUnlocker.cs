using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class SkillUnlocker : MonoBehaviour
{
    public int slayedMonsterCount;
    public int totalMonsterCount;
    public static SkillUnlocker Instance;
    public Skill[] archerSkills;
    public Skill[] warriorSkills;
    void Awake()
    {
        Instance=this;
        slayedMonsterCount++;
    }
    public void IncreaseSlayedMonsterCount()
    {
        slayedMonsterCount++;

        int unlockedSkill=Mathf.FloorToInt((float)slayedMonsterCount/totalMonsterCount);
        if (unlockedSkill<archerSkills.Length)
        {
            archerSkills[unlockedSkill].UnlockSkill();
        }
        if (unlockedSkill<warriorSkills.Length)
        {
            warriorSkills[unlockedSkill].UnlockSkill();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
