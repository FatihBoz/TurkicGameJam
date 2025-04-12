using UnityEngine;
using UnityEngine.AI;

public class SpearEnemyAI : MonoBehaviour
{
    [Header("Patrol")]
    public Transform patrolParent;
    private Transform[] patrolPoints;  // Yapay zekanýn gideceði noktalara dizilmiþ Patrol noktalarý
    private int currentPatrolIndex = 0;

    [Header("Combat Properties")]
    public Transform player;              // Oyuncu
    public float attackRange = 3f;        // Saldýrý menzili
    public float detectionRange = 10f;    // Tespit menzili
    public float retreatRange = 1.5f;     // Geri çekilme mesafesi
    public float moveSpeed = 2f;          // Hareket hýzý
    public float attackCooldown = 1.5f;   // Saldýrý bekleme süresi

    private NavMeshAgent agent;           // AI'nin hareket ettiði ajan
    private float lastAttackTime = 0f;    // Son saldýrý zamaný

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

        // Patrol noktasýna doðru hareket et
        if (patrolPoints.Length > 0)
        {
            Transform targetPoint = patrolPoints[currentPatrolIndex];

            agent.SetDestination(targetPoint.position);

            // Eðer hedef noktaya ulaþýldýysa, bir sonraki noktaya geç
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
        // parent'teki tüm çocuklarý al
        patrolPoints = new Transform[patrolParent.childCount];

        // Her çocuðu patrol noktasý olarak ata
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

    // Mýzrakla oyuncuya saldýrma
    void AttackPlayer()
    {
        agent.speed = 0f;  // Saldýrýrken hareket etmesin
        if (Time.time - lastAttackTime > attackCooldown)
        {
            // Burada mýzrak saldýrýsý animasyonu ve etkiyi tetikleyebilirsiniz.
            Debug.Log("Spear Attack!");

            // Saldýrýyý gerçekleþtirdikten sonra saldýrý zamanýný güncelle
            lastAttackTime = Time.time;
        }
    }

}
