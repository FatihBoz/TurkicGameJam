using UnityEngine;
using UnityEngine.AI;

public class SpearEnemyAI : MonoBehaviour
{
    [Header("Patrol")]
    public Transform patrolParent;
    private Transform[] patrolPoints;  // Yapay zekan�n gidece�i noktalara dizilmi� Patrol noktalar�
    private int currentPatrolIndex = 0;

    [Header("Combat Properties")]
    public Transform player;              // Oyuncu
    public float attackRange = 1f;        // Sald�r� menzili
    public float detectionRange = 10f;    // Tespit menzili
    public float retreatRange = 1.5f;     // Geri �ekilme mesafesi
    public float moveSpeed = 2f;          // Hareket h�z�
    public float attackCooldown = 1.5f;   // Sald�r� bekleme s�resi

    private NavMeshAgent agent;           // AI'nin hareket etti�i ajan
    private float lastAttackTime = 0f;    // Son sald�r� zaman�

    private enum State { Patrol, Chase, Attack, Idle }
    private State currentState;

    private Animator animator;            // Animator bile�eni



    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentState = DecideFirstState(); // �lk durumu belirle

        if (currentState == State.Idle)
        {
            animator.SetBool("isPatrol", false); // Y�r�y�� animasyonunu durdur
        }
        else if (currentState == State.Patrol)
        {
            animator.SetBool("isPatrol", true); // Y�r�y�� animasyonunu ba�lat
            GetPatrolPoints();
            print("Walking");
        }

    }

    State DecideFirstState()
    {
        State firstState = State.Idle;
        if (patrolParent == null)
        {
            firstState = State.Idle;
            Debug.Log("Patrol Parent is not assigned in the inspector.");
        }
        else if (patrolParent != null)
        {
            firstState = State.Patrol;
        }

        return firstState;
    }

    void Update()
    {
        player = PlayerChange.Instance.GetPlayer().transform;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);     


        switch (currentState)
        {
            case State.Patrol:
                PatrolBehaviour();

                if (distanceToPlayer < detectionRange)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Chase:

                AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
                // E�er sald�r� animasyonu devam ediyorsa, state de�i�tirme
                if (animState.IsName("NormalAttack02_SwordShield") || animState.IsName("NormalAttack01_SwordShield"))
                {
                    print("Attack Animation is playing");
                    animator.SetBool("isChasing", true); 
                    // H�l� sald�r� animasyonu oynuyor
                    break;
                }
                ChasePlayer();


                if (distanceToPlayer <= attackRange)
                {
                    currentState = State.Attack;
                }

                break;
            

            case State.Attack:
                AttackPlayer();
                if (distanceToPlayer > attackRange)
                {
                    currentState = State.Chase;
                }

                break;

            case State.Idle:
                IdleWait();

                if (distanceToPlayer < detectionRange)
                {
                    currentState = State.Chase;
                }
                break;

        }

        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        print(" Current State Name: " + currentState);
    }

    // Patrol Yapma
    void PatrolBehaviour()
    {
        agent.speed = moveSpeed;

        // Patrol noktas�na do�ru hareket et
        if (patrolPoints.Length > 0)
        {
            Transform targetPoint = patrolPoints[currentPatrolIndex];

            agent.SetDestination(targetPoint.position);

            // E�er hedef noktaya ula��ld�ysa, bir sonraki noktaya ge�
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }

        // Oyuncuyu tespit etme
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            animator.SetBool("isChasing", true); // Y�r�y�� animasyonunu durdur
            currentState = State.Chase;
        }
    }

    void GetPatrolPoints()
    {
        // parent'teki t�m �ocuklar� al
        patrolPoints = new Transform[patrolParent.childCount];

        // Her �ocu�u patrol noktas� olarak ata
        for (int i = 0; i < patrolParent.childCount; i++)
        {
            patrolPoints[i] = patrolParent.GetChild(i);
        }
    }

    void IdleWait()
    {
        agent.speed = 0f; // Beklerken hareket etmesin
        animator.SetBool("isPatrol", false); // Y�r�y�� animasyonunu durdur
    }

    // Oyuncuyu takip etme
    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isChasing", true); // Y�r�y�� animasyonunu ba�lat
        agent.speed = moveSpeed * 1.5f;


    }

    // M�zrakla oyuncuya sald�rma
    void AttackPlayer()
    {
        agent.speed = 0f;  // Sald�r�rken hareket etmesin
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.SetTrigger("Attack");

            player.GetComponent<PlayerHealth>().TakeDamage(5);

            animator.SetBool("isChasing", false); // Y�r�y�� animasyonunu durdur

            transform.LookAt(player.position); // Oyuncuya bakma

            // Sald�r�y� ger�ekle�tirdikten sonra sald�r� zaman�n� g�ncelle
            lastAttackTime = Time.time;
        }
    }
}
