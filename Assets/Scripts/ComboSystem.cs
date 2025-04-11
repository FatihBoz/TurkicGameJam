using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    [Header("Combo Settings")]
    
    // Animation references
    [SerializeField] private Animator animator;
    
    [Header("Attack Settings")]
    [SerializeField] private float firstAttackDamage = 20f;
    [SerializeField] private float secondAttackDamage = 35f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Transform attackPoint;
    
    // Combo state
    private bool firstAttack = false;
    private bool secondAttack = false;
    
    
    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
            
        if (attackPoint == null)
            attackPoint = transform;
    }
    
    private void Update()
    {

        if (Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
        animator.SetBool("firstAttack",firstAttack);
        animator.SetBool("secondAttack",secondAttack);
    }
    
    private void PerformAttack()
    {
        if (!firstAttack && !secondAttack)
        {
            firstAttack = true;
        }
        else if (firstAttack && !secondAttack)
        {
            secondAttack = true;
        }  
    }
    
    public void AfterFirstAttack()
    {
        if (!secondAttack)
        {
            firstAttack = false;
        }
    }
    
    public void AfterSecondAttack()
    {
        secondAttack = false;
        firstAttack = false;
    }
    
    // Called from animation event during first attack animation
    public void DealFirstAttackDamage()
    {
        DealDamageInArea(firstAttackDamage);
    }
    
    // Called from animation event during second attack animation
    public void DealSecondAttackDamage()
    {
        DealDamageInArea(secondAttackDamage);
    }
    
    private void DealDamageInArea(float damageAmount)
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
            }
        }
    }
    
    // Optional: Visualize the attack range in the editor
    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
            
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
} 