using UnityEngine;
using System.Collections;

public class ComboSystem : MonoBehaviour
{
    [Header("Combo Settings")]
    
    // Animation references
    [SerializeField] private Animator animator;
    [SerializeField] private BasicMovement playerMovement;
    
    [Header("Attack Settings")]
    [SerializeField] private float firstAttackDamage = 20f;
    [SerializeField] private float secondAttackDamage = 35f;
    [SerializeField] private float thirdAttackDamage = 50f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Transform attackPoint;
    
    [Header("Knockback Settings")]
    [SerializeField] private float firstAttackKnockback = 5f;
    [SerializeField] private float secondAttackKnockback = 10f;
    [SerializeField] private float thirdAttackKnockback = 15f;
    [SerializeField] private float knockbackUpwardForce = 2f;
    
    [Header("Hit Stop Settings")]
    [SerializeField] private bool useHitStop = true;
    [SerializeField] private float hitStopDuration = 0.1f;
    [SerializeField] private float timeScale = 0.1f;
    
    [Header("Ground Slam Settings")]
    [SerializeField] private float groundSlamDamage = 50f;
    [SerializeField] private float groundSlamRange = 3f;
    [SerializeField] private float groundSlamKnockback = 15f;
    [SerializeField] private float groundSlamForce = 20f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject slamEffectPrefab;
    
    [Header("Rage Skill Settings")]
    [SerializeField] private float rageDuration = 10f;
    [SerializeField] private float rageDamageMultiplier = 1.5f;
    [SerializeField] private float rageAttackSpeedMultiplier = 1.3f;
    [SerializeField] private float rageMoveSpeedMultiplier = 1.2f;
    [SerializeField] private GameObject rageEffectPrefab;
    
    [Header("Slash Effect Settings")]
    [SerializeField] private GameObject slashEffectPrefab;
    [SerializeField] private float slashEffectDuration = 0.5f;
    [SerializeField] private Vector3 slashEffectOffset = new Vector3(0, 0, 1f);
    
    // Combo state
    private bool firstAttack = false;
    private bool secondAttack = false;
    private bool thirdAttack = false;
    private bool isGroundSlamming = false;
    private Rigidbody rb;
    
    // Rage state
    private bool isRageActive = false;
    private float originalFirstAttackDamage;
    private float originalSecondAttackDamage;
    private float originalThirdAttackDamage;
    private float originalGroundSlamDamage;
    private float originalMoveSpeed;
    
    
    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
            
        if (attackPoint == null)
            attackPoint = transform;

        if (playerMovement == null)
            playerMovement = GetComponent<BasicMovement>();
        
        rb = GetComponent<Rigidbody>();
        
        // Store original values for rage state
        originalFirstAttackDamage = firstAttackDamage;
        originalSecondAttackDamage = secondAttackDamage;
        originalThirdAttackDamage = thirdAttackDamage;
        originalGroundSlamDamage = groundSlamDamage;
        originalMoveSpeed = playerMovement.GetMoveSpeed();
    }
    
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0))
        {
            if (CheckForGroundSlam())
                return;
                
            PerformAttack();
        }

        animator.SetBool("firstAttack", firstAttack);
        animator.SetBool("secondAttack", secondAttack);
        animator.SetBool("thirdAttack", thirdAttack);
    }
    
    private bool CheckForGroundSlam()
    {
        // Check if player is in the air (not grounded) and pressed attack
        if (!playerMovement.IsGrounded() && !isGroundSlamming)
        {
            PerformGroundSlam();
            return true;
        }
        return false;
    }
    
    private void PerformGroundSlam()
    {
        isGroundSlamming = true;
        animator.SetBool("groundSlamming",true);
        playerMovement.SetCanMove(false);

        // Apply downward force for quick landing
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(Vector3.down * groundSlamForce, ForceMode.Impulse);
        
        // Start checking for ground collision
        StartCoroutine(CheckGroundSlamImpact());
    }
    
    private IEnumerator CheckGroundSlamImpact()
    {
        // Wait until we hit the ground
        while (!playerMovement.IsGrounded())
        {
            yield return null;
        }
        
        animator.SetBool("groundSlamming",false);
        // We've hit the ground, perform the ground slam attack
        GroundSlamImpact();
        
        // Reset state
        isGroundSlamming = false;
    }
    
    private void GroundSlamImpact()
    {
        // Detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, groundSlamRange, enemyLayers);
        
        // Apply damage to enemies
        bool hitSomething = false;
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(groundSlamDamage);
                ApplyRadialKnockback(enemy.transform, groundSlamKnockback);
                hitSomething = true;
            }
        }
        
        // Apply hit stop if we hit something
        if (hitSomething && useHitStop)
        {
            DoHitStop();
        }
        
        // Spawn visual effect
        if (slamEffectPrefab != null)
        {
            Instantiate(slamEffectPrefab, transform.position, Quaternion.identity);
        }
    }
    
    private void ApplyRadialKnockback(Transform target, float knockbackForce)
    {
        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb != null)
        {
            // Calculate direction from impact point to enemy
            Vector3 direction = (target.position - transform.position).normalized;
            
            // Add upward force for better visual effect
            direction.y += knockbackUpwardForce;
            
            // Apply the knockback force
            targetRb.AddForce(direction * knockbackForce, ForceMode.Impulse);
        }
    }
    
    private void PerformAttack()
    {
        if (!firstAttack && !secondAttack && !thirdAttack)
        {
            firstAttack = true;
            playerMovement.SetCanMove(false);
        }
        else if (firstAttack && !secondAttack && !thirdAttack)
        {
            secondAttack = true;
        }
        else if (firstAttack && secondAttack && !thirdAttack)
        {
            thirdAttack = true;
        }
    }
    
    public void AfterFirstAttack()
    {
        if (!secondAttack)
        {
            firstAttack = false;
            playerMovement.SetCanMove(true);
        }
    }
    
    public void AfterSecondAttack()
    {
        if (!thirdAttack)
        {
            secondAttack = false;
            firstAttack = false;
            playerMovement.SetCanMove(true);
        }
    }
    
    public void AfterThirdAttack()
    {
        thirdAttack = false;
        secondAttack = false;
        firstAttack = false;
        playerMovement.SetCanMove(true);
    }
    
    // Called from animation event during first attack animation
    public void DealFirstAttackDamage()
    {
        bool hitSomething = DealDamageInArea(firstAttackDamage, firstAttackKnockback);
        if (hitSomething && useHitStop)
        {
            DoHitStop();
        }
        
        // Instantiate slash effect
        SpawnSlashEffect();
    }
    
    // Called from animation event during second attack animation
    public void DealSecondAttackDamage()
    {
        bool hitSomething = DealDamageInArea(secondAttackDamage, secondAttackKnockback);
        if (hitSomething && useHitStop)
        {
            DoHitStop();
        }
        
        // Instantiate slash effect
        SpawnSlashEffect();
    }
    
    // Called from animation event during third attack animation
    public void DealThirdAttackDamage()
    {
        bool hitSomething = DealDamageInArea(thirdAttackDamage, thirdAttackKnockback);
        if (hitSomething && useHitStop)
        {
            DoHitStop();
        }
        
        // Instantiate slash effect
        SpawnSlashEffect();
    }
    
    private bool DealDamageInArea(float damageAmount, float knockbackForce)
    {
        // Detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        
        // Apply damage to enemies
        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                if (ScreenShake.Instance != null)
                {
                    ScreenShake.Instance.Shake(4f, 0.3f);
                }
                ApplyKnockback(enemy.transform, knockbackForce);
                return true; // We hit at least one enemy
            }
        }
        
        return false; // We didn't hit any enemies
    }
    
    private void ApplyKnockback(Transform target, float knockbackForce)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Calculate direction from player to enemy
            Vector3 direction = (target.position - transform.position).normalized;
            
            // Add upward force for better visual effect
            direction.y += knockbackUpwardForce;
            
            // Apply the knockback force
            rb.AddForce(direction * knockbackForce, ForceMode.Impulse);
        }
    }
    public void EnableSetCanMove()
    {
        playerMovement.SetCanMove(true);
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
    
    // Optional: Visualize the attack range in the editor
    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
            
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        
        // Draw ground slam range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, groundSlamRange);
    }

    public void RageActivate()
    {
        if (isRageActive)
            return;
            
        isRageActive = true;
        
        // Apply rage buffs
        firstAttackDamage *= rageDamageMultiplier;
        secondAttackDamage *= rageDamageMultiplier;
        thirdAttackDamage *= rageDamageMultiplier;
        groundSlamDamage *= rageDamageMultiplier;
        
        // Set move speed if player movement has the property
        playerMovement.SetMoveSpeed(originalMoveSpeed * rageMoveSpeedMultiplier);
        
        // Set attack speed through animator
        animator.SetFloat("AttackSpeed", rageAttackSpeedMultiplier);
        
        // Activate visual effects
        if (rageEffectPrefab != null)
        {
            GameObject effect = Instantiate(rageEffectPrefab, transform);
            Destroy(effect, rageDuration);
        }
        
        // Set animator parameter
        animator.SetBool("RageActive", true);
        
        // Start the timer to deactivate rage
        StartCoroutine(DeactivateRageAfterDuration());
    }
    
    private IEnumerator DeactivateRageAfterDuration()
    {
        yield return new WaitForSeconds(rageDuration);
        Debug.Log("Deactivating Rage");
        DeactivateRage();
    }
    
    private void DeactivateRage()
    {
        if (!isRageActive)
            return;
            
        isRageActive = false;
        
        // Reset stats to original values
        firstAttackDamage = originalFirstAttackDamage;
        secondAttackDamage = originalSecondAttackDamage;
        thirdAttackDamage = originalThirdAttackDamage;
        groundSlamDamage = originalGroundSlamDamage;
        
        // Reset move speed
        playerMovement.SetMoveSpeed(originalMoveSpeed);
        
        // Reset attack speed
        animator.SetFloat("AttackSpeed", 1f);
        
        // Reset animator parameter
        animator.SetBool("RageActive", false);
    }
    
    public bool IsRageActive()
    {
        return isRageActive;
    }

    private void SpawnSlashEffect()
    {
        if (slashEffectPrefab != null)
        {
            // Calculate position in front of the player based on facing direction
            Vector3 spawnPosition = transform.position + transform.forward * slashEffectOffset.z + transform.up * slashEffectOffset.y + transform.right * slashEffectOffset.x;
            
            // Instantiate the effect aligned with player's rotation
            GameObject slashEffect = Instantiate(slashEffectPrefab, spawnPosition, transform.rotation);
            
            // Destroy the effect after set duration
            Destroy(slashEffect, slashEffectDuration);
        }
    }
} 