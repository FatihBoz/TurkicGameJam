using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class SkillUnlocker : MonoBehaviour
{
    public int slayedMonsterCount;
    public int totalMonsterCount;
    public static SkillUnlocker Instance;
    public Skill[] archerSkills;
    public Skill[] warriorSkills;
     public float unlockedSkill;
    void Awake()
    {
        Instance=this;
        slayedMonsterCount++;
    }
      void OnEnable()
    {
        PlayerChange.OnPlayerChange +=OnCharacterChange;
    }
    void OnDisable()
    {
        PlayerChange.OnPlayerChange -=OnCharacterChange;
    }
    public void OnCharacterChange()
    {
         UnlockSkills();
    }

    public void IncreaseSlayedMonsterCount()
    {
        slayedMonsterCount++;

        unlockedSkill=(float)slayedMonsterCount/totalMonsterCount;
         UnlockSkills();
    }

public void UnlockSkills()
    {
        for (int i = 0; i < Mathf.CeilToInt(unlockedSkill*archerSkills.Length); i++)
        {
            archerSkills[i].UnlockSkill();
            if (i<archerSkills.Length)
            {
                archerSkills[i].UnlockSkill();
            }
        }
        for (int i = 0; i < Mathf.CeilToInt(unlockedSkill*warriorSkills.Length); i++)
        {
            warriorSkills[i].UnlockSkill();
            if (i<warriorSkills.Length)
            {
                warriorSkills[i].UnlockSkill();
            }
        }
    }
}
