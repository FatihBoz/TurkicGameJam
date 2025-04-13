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
    public float attackRange = 3f;        // Sald�r� menzili
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

    private void Start()
    {

    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);     

        switch (currentState)
        {
            case State.Patrol:
                if (distanceToPlayer < detectionRange)
                {
                    currentState = State.Chase;
                }
                PatrolBehaviour();

                break;

            case State.Chase:
                if (distanceToPlayer <= attackRange)
                {
                    currentState = State.Attack;
                }
                ChasePlayer();

                break;

            case State.Attack:
                if (distanceToPlayer > attackRange)
                {
                    currentState = State.Chase;
                }
                AttackPlayer();

                break;

            case State.Idle:
                if (distanceToPlayer < detectionRange)
                {
                    currentState = State.Chase;
                }
                IdleWait();

                break;
        }
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
            // Burada m�zrak sald�r�s� animasyonu ve etkiyi tetikleyebilirsiniz.
            Debug.Log("Attack!");
            animator.SetTrigger("Attack");
            animator.SetBool("isChasing", false); // Y�r�y�� animasyonunu durdur


            // Sald�r�y� ger�ekle�tirdikten sonra sald�r� zaman�n� g�ncelle
            lastAttackTime = Time.time;
        }
    }
}
