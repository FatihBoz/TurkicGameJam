using UnityEngine;

public class ComboSystem : MonoBehaviour
{
    [Header("Combo Settings")]
    
    // Animation references
    [SerializeField] private Animator animator;
    
    // Combo state
    private bool firstAttack = false;
    private bool secondAttack = false;
    
    
    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }
    
    private void Update()
    {

        // Check for attack input (replace with your input method)
        if (Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }

        animator.SetBool("firstAttacked",firstAttack);
        animator.SetBool("secondAttack",secondAttack);
    }
    
    private void PerformAttack()
    {
        // First attack in combo
        if (!firstAttack && !secondAttack)
        {
            firstAttack = true;
            
            // Play first attack animation
            if (animator != null)
                animator.SetBool("firstAttack",true);
            
            Debug.Log("First Attack");
        }
        // Second attack in combo
        else if (firstAttack && !secondAttack)
        {
            secondAttack = true;
            
            // Play second attack animation
            if (animator != null)
                animator.SetBool("secondAttack",true);
            
            Debug.Log("Second Attack");
            
        }
    }
    
    private void ResetCombo()
    {
        firstAttack = false;
        secondAttack = false;
        Debug.Log("Combo Reset");
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
} 