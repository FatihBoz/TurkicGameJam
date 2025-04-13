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
    public float attackRange = 1f;        // Saldýrý menzili
    public float detectionRange = 10f;    // Tespit menzili
    public float retreatRange = 1.5f;     // Geri çekilme mesafesi
    public float moveSpeed = 2f;          // Hareket hýzý
    public float attackCooldown = 1.5f;   // Saldýrý bekleme süresi

    private NavMeshAgent agent;           // AI'nin hareket ettiði ajan
    private float lastAttackTime = 0f;    // Son saldýrý zamaný

    private enum State { Patrol, Chase, Attack, Idle }
    private State currentState;

    private Animator animator;            // Animator bileþeni



    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentState = DecideFirstState(); // Ýlk durumu belirle

        if (currentState == State.Idle)
        {
            animator.SetBool("isPatrol", false); // Yürüyüþ animasyonunu durdur
        }
        else if (currentState == State.Patrol)
        {
            animator.SetBool("isPatrol", true); // Yürüyüþ animasyonunu baþlat
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
                // Eðer saldýrý animasyonu devam ediyorsa, state deðiþtirme
                if (animState.IsName("NormalAttack02_SwordShield") || animState.IsName("NormalAttack01_SwordShield"))
                {
                    print("Attack Animation is playing");
                    animator.SetBool("isChasing", true); 
                    // Hâlâ saldýrý animasyonu oynuyor
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
            animator.SetBool("isChasing", true); // Yürüyüþ animasyonunu durdur
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

    void IdleWait()
    {
        agent.speed = 0f; // Beklerken hareket etmesin
        animator.SetBool("isPatrol", false); // Yürüyüþ animasyonunu durdur
    }

    // Oyuncuyu takip etme
    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isChasing", true); // Yürüyüþ animasyonunu baþlat
        agent.speed = moveSpeed * 1.5f;


    }

    // Mýzrakla oyuncuya saldýrma
    void AttackPlayer()
    {
        agent.speed = 0f;  // Saldýrýrken hareket etmesin
        if (Time.time - lastAttackTime > attackCooldown)
        {
            animator.SetTrigger("Attack");

            player.GetComponent<PlayerHealth>().TakeDamage(5);

            animator.SetBool("isChasing", false); // Yürüyüþ animasyonunu durdur

            transform.LookAt(player.position); // Oyuncuya bakma

            // Saldýrýyý gerçekleþtirdikten sonra saldýrý zamanýný güncelle
            lastAttackTime = Time.time;
        }
    }
}
