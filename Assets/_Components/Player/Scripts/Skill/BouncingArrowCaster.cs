using UnityEngine;

public class BouncingArrowCaster : Skill
{
    [SerializeField] private Animator animator;
    [SerializeField] private BouncingArrow arrowPrefab;


    float elapsedTime = 0f; 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !Archer.Instance.isCasting && !isOnCooldown)
        {
            ScreenShake.Instance.Shake();
            Archer.Instance.InstantiateBouncingArrow(arrowPrefab);
            skillBar.Cooldown(cooldownTime);
            isOnCooldown = true;
        }

        if (isOnCooldown)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= cooldownTime)
            {
                elapsedTime = 0f;
                isOnCooldown = false;
            }
        }
    }

}
