using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected KeyCode keyCode;
    [SerializeField] protected SkillBar skillBar;


    public SkillBar GetSkillBar()
    {
        return skillBar;
    }

    public void SetSkillBar(SkillBar skillBar)
    {
        this.skillBar = skillBar;
    }

    public KeyCode GetKeyCode()
    {
        return keyCode;
    }

    public void SetKeyCode(KeyCode key)
    {
        this.keyCode = key;
    }
}
