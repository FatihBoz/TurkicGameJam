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
        for (int i = 0; i < Mathf.CeilToInt(unlockedSkill*archerSkills.Length); i++)
        {
            archerSkills[i].UnlockSkill();
        }
        for (int i = 0; i < Mathf.CeilToInt(unlockedSkill*warriorSkills.Length); i++)
        {
            warriorSkills[i].UnlockSkill();
        }
    }
    public void IncreaseSlayedMonsterCount()
    {
        slayedMonsterCount++;

        unlockedSkill=(float)slayedMonsterCount/totalMonsterCount;
        for (int i = 0; i < Mathf.CeilToInt(unlockedSkill*archerSkills.Length); i++)
        {
            archerSkills[i].UnlockSkill();
        }
        for (int i = 0; i < Mathf.CeilToInt(unlockedSkill*warriorSkills.Length); i++)
        {
            warriorSkills[i].UnlockSkill();
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
