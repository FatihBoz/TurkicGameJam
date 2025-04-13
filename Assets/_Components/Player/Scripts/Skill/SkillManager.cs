using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SkillManager : MonoBehaviour
{
    [SerializeField] private List<Skill> skills;
    private int listIndex = 0;

    private void Start()
    {

    }
}
