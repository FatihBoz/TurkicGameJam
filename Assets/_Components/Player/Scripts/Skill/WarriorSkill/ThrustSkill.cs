using UnityEngine;
using UnityEngine.UI;

public class ThrustSkill : Skill
{
    [SerializeField] private ComboSystem comboSystem;
    [SerializeField] private KeyCode thrustSkillKey = KeyCode.Q;
    
    [Header("Visual Effects")]
    [SerializeField] private GameObject rageActivationEffect;
    private float elapsedTime = 0f;
    private bool isThrustActive = false;

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
        if (Input.GetKeyDown(thrustSkillKey) && !isOnCooldown && !isThrustActive)
        {
            isOnCooldown = true;
            isThrustActive = true;
            
            // Call ThrustActivate on the combo system
            comboSystem.ThrustActivate();
            
            // Spawn thrust activation effect
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
                isThrustActive = false;
            }
        }
    }
    
    // Public method to check if thrust is active
    public bool IsThrustActive()
    {
        return isThrustActive;
    }
}
