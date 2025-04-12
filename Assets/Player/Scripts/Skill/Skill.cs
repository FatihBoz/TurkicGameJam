using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected Archer archer;

    public virtual void Initialize(Archer archer)
    {
        this.archer = archer;
    }

    public abstract void UseSkill();
}
