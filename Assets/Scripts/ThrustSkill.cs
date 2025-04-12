using UnityEngine;
using System.Collections;

public class ThrustSkill : MonoBehaviour
{
    [Header("Thrust Settings")]
    [SerializeField] private float thrustForce = 15f;
    [SerializeField] private float thrustDuration = 0.3f;
    [SerializeField] private float thrustCooldown = 2f;
    [SerializeField] private AnimationCurve thrustCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    
    [Header("Damage Settings")]
    [SerializeField] private float thrustDamage = 40f;
    [SerializeField] private float thrustKnockback = 12f;
    [SerializeField] private float knockbackUpwardForce = 2f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Transform attackPoint;
    
    [Header("Hit Stop Settings")]
    [SerializeField] private bool useHitStop = true;
    [SerializeField] private float hitStopDuration = 0.1f;
    [SerializeField] private float timeScale = 0.1f;
    
    [Header("Visual Effects")]
    [SerializeField] private GameObject thrustEffectPrefab;
    [SerializeField] private float effectDuration = 0.5f;
    
    [Header("References")]
    [SerializeField] private BasicMovement playerMovement;
    [SerializeField] private Animator animator;
    
    // State management
    private bool canThrust = true;
    private bool isThrustActive = false;
    private float cooldownTimer = 0f;
    private InputSystem_Actions inputActions;
    private Rigidbody rb;
    
    private void Awake()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<BasicMovement>();
            
        if (animator == null)
            animator = GetComponent<Animator>();
            
        if (attackPoint == null)
            attackPoint = transform;
            
        rb = GetComponent<Rigidbody>();
        inputActions = new InputSystem_Actions();
    }
    
    private void OnEnable()
    {
        inputActions.Player.Enable();
        // Add thrust input binding (Q key)
        AddThrustInputListener();
    }
    
    private void OnDisable()
    {
        // Clean up
        RemoveThrustInputListener();
        inputActions.Player.Disable();
    }
    
    private void Update()
    {
        // Handle cooldown
        if (!canThrust)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canThrust = true;
                cooldownTimer = 0f;
            }
        }
    }
    
    private void AddThrustInputListener()
    {
        // Legacy input detection in Update since it's not in the InputSystem_Actions
        // We'll implement it in the InputLegacy() method
    }
    
    private void RemoveThrustInputListener()
    {
        // Nothing to remove for legacy input
    }
    
    // Called every frame to check for legacy input
    private void InputLegacy()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateThrust();
        }
    }
    
    private void FixedUpdate()
    {
        // Check for thrust input using legacy input system
        InputLegacy();
    }
    
    private void ActivateThrust()
    {
        if (!canThrust || isThrustActive || !playerMovement.IsGrounded())
            return;
            
        StartCoroutine(PerformThrustRoutine());
    }
    
    private IEnumerator PerformThrustRoutine()
    {
        // Set state
        isThrustActive = true;
        canThrust = false;
        cooldownTimer = thrustCooldown;
        
        // Temporarily disable player movement control
        playerMovement.SetCanMove(false);
        
        // Play animation if available
        if (animator != null)
        {
            animator.SetTrigger("thrust");
        }
        
        // Calculate thrust direction (forward of player)
        Vector3 thrustDirection = transform.forward;
        
        // Spawn effect if available
        if (thrustEffectPrefab != null)
        {
            GameObject effect = Instantiate(thrustEffectPrefab, transform.position, transform.rotation);
            Destroy(effect, effectDuration);
        }
        
        // Apply thrust force over time using animation curve
        float elapsedTime = 0f;
        while (elapsedTime < thrustDuration)
        {
            float normalizedTime = elapsedTime / thrustDuration;
            float currentThrustForce = thrustForce * thrustCurve.Evaluate(normalizedTime);
            
            // Apply force
            rb.AddForce(thrustDirection * currentThrustForce, ForceMode.Acceleration);
            
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        
        // Restore player control
        isThrustActive = false;
    }
    
    // Called from animation event during thrust animation
    public void DealThrustDamage()
    {
        bool hitSomething = DealDamageInArea(thrustDamage, thrustKnockback);
        if (hitSomething && useHitStop)
        {
            DoHitStop();
        }
    }
    
    private bool DealDamageInArea(float damageAmount, float knockbackForce)
    {
        // Detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        
        // Apply damage to enemies
        bool hitAnEnemy = false;
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                ApplyKnockback(enemy.transform, knockbackForce);
                hitAnEnemy = true;
            }
        }
        
        return hitAnEnemy;
    }
    
    private void ApplyKnockback(Transform target, float knockbackForce)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb != null)
        {
            // Calculate direction from player to enemy
            Vector3 direction = (target.position - transform.position).normalized;
            
            // Add upward force for better visual effect
            direction.y += knockbackUpwardForce;
            
            // Apply the knockback force
            targetRb.AddForce(direction * knockbackForce, ForceMode.Impulse);
        }
    }
    
    private void DoHitStop()
    {
        StartCoroutine(HitStopCoroutine());
    }
    
    private IEnumerator HitStopCoroutine()
    {
        // Store original time scale and set to slow motion
        float originalTimeScale = Time.timeScale;
        Time.timeScale = timeScale;
        
        // Store original fixed delta time and adjust it
        float originalFixedDeltaTime = Time.fixedDeltaTime;
        Time.fixedDeltaTime = Time.fixedDeltaTime * timeScale;
        
        // Wait for the hit stop duration
        yield return new WaitForSecondsRealtime(hitStopDuration);
        
        // Restore original time scale and fixed delta time
        Time.timeScale = originalTimeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }
    
    // Utility method to reset cooldown (useful for pickups or other game mechanics)
    public void ResetCooldown()
    {
        canThrust = true;
        cooldownTimer = 0f;
    }
    
    public void ReenablePlayerMove()
    {
        playerMovement.SetCanMove(true);
    }
    
    // Getters for UI or other systems
    public bool IsThrusting() => isThrustActive;
    public float GetCooldownRemaining() => cooldownTimer;
    public float GetTotalCooldown() => thrustCooldown;
    public float GetCooldownNormalized() => cooldownTimer / thrustCooldown;
    
    // Optional: Visualize the attack range in the editor
    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
            
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}