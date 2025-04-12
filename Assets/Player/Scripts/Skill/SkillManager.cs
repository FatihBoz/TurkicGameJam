using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> archerSkillPrefabs;

    private Dictionary<GameObject, bool> skillDict;
    private Archer archer;

    private void Awake()
    {
        foreach (GameObject skillPrefab in archerSkillPrefabs)
        {
            skillDict.Add(skillPrefab, false);
        }
    }

    public void EarnSkill(Skill skill)
    {
        //Alýnacak skill kaldý mý diye kontrol et
        int r = Random.Range(0, skillDict.Count);
        while (skillDict.Keys.ElementAt(r) != true)
        {
            r = Random.Range(0, skillDict.Count);   
        }
        

        GameObject randomSkillPrefab = skillDict.Keys.ElementAt(r);
        skillDict[randomSkillPrefab] = true;

        Instantiate(randomSkillPrefab, archer.transform);
        archer.AssignSkill(skill);
    }
}
