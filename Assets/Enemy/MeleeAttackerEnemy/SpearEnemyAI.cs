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

    private enum State { Patrol, Chase, Attack }
    private State currentState;
    

    void Start()
    {
        GetPatrolPoints();
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Patrol;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        print("current state : " + currentState);

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


    // Oyuncuyu takip etme
    void ChasePlayer()
    {
        agent.speed = moveSpeed * 1.5f;
        agent.SetDestination(player.position);
    }

    // M�zrakla oyuncuya sald�rma
    void AttackPlayer()
    {
        agent.speed = 0f;  // Sald�r�rken hareket etmesin
        if (Time.time - lastAttackTime > attackCooldown)
        {
            // Burada m�zrak sald�r�s� animasyonu ve etkiyi tetikleyebilirsiniz.
            Debug.Log("Spear Attack!");

            // Sald�r�y� ger�ekle�tirdikten sonra sald�r� zaman�n� g�ncelle
            lastAttackTime = Time.time;
        }
    }

}
