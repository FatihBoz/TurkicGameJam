using UnityEngine;

public class RageSkill : Skill
{
    [SerializeField] private ComboSystem comboSystem;
    [SerializeField] private KeyCode rageSkillKey = KeyCode.Q;
    
    [Header("Visual Effects")]
    [SerializeField] private GameObject rageActivationEffect;
    private float elapsedTime = 0f;
    private bool isRageActive = false;

    void Start()
    {
        if (comboSystem == null)
        {
            comboSystem = GetComponentInParent<ComboSystem>();
        }
    }

    void Update()
    {
        // Check for keypress and activate rage if not on cooldown
        if (Input.GetKeyDown(rageSkillKey) && !isOnCooldown && !isRageActive)
        {
            skillBar.Cooldown(cooldownTime);
            isOnCooldown = true;
            isRageActive = true;
            
            // Call RageActivate on the combo system
            comboSystem.RageActivate();
            
            // Spawn rage activation effect
            if (rageActivationEffect != null)
            {
                Instantiate(rageActivationEffect, transform.position, Quaternion.identity, transform);
            }
        }
        
        // Handle cooldown timer and UI display
        if (isOnCooldown)
        {
            elapsedTime += Time.deltaTime;


            // Check if cooldown is complete
            if (elapsedTime >= cooldownTime)
            {
                elapsedTime = 0f;
                isOnCooldown = false;
                isRageActive = false;
                
            }
        }
    }
    
    // Public method to check if rage is active
    public bool IsRageActive()
    {
        return isRageActive;
    }
}
