using UnityEngine;

public class AI : MonoBehaviour
{
    public enum AIState { Patrol, Alert, Trap }
    public AIState currentState;

    public Transform patrolParent;
    public Transform[] patrolPoints;  // Yapay zekan�n gidece�i noktalara dizilmi� Patrol noktalar�
    private int currentPatrolIndex = 0;  // �u anda gidilen patrol noktas�

    public Transform player;  // Oyuncu referans�
    public float detectionRange = 10f;  // Oyuncuyu alg�lama mesafesi
    public float moveSpeed = 3f;  // Hareket h�z�

    public Transform trapTriggerArea;  // Tuzak tetikleme b�lgesi
    public bool isTrapTriggered = false;  // Tuzak tetiklendi mi?

    void Start()
    {
        GetPatrolPoints();

        currentState = AIState.Patrol;
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                PatrolBehavior();
                break;
            case AIState.Alert:
                AlertBehavior();
                break;
            case AIState.Trap:
                //TrapBehavior();
                break;
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

    // Patrol Yapma
    void PatrolBehavior()
    {
        // Patrol noktas�na do�ru hareket et
        if (patrolPoints.Length > 0)
        {
            Transform targetPoint = patrolPoints[currentPatrolIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

            // E�er hedef noktaya ula��ld�ysa, bir sonraki noktaya ge�
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }

        // Oyuncuyu tespit etme
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            currentState = AIState.Alert;
        }
    }
    
    // Alarm durumunda yap�lacaklar
    void AlertBehavior()
    {
        // Oyuncuya do�ru hareket et
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        /*
        // E�er oyuncu Trap b�lgesine girerse, tuza�� tetikle
        if (Vector3.Distance(transform.position, trapTriggerArea.position) < 2f)
        {
            currentState = AIState.Trap;
        }

        // E�er oyuncu belirli bir mesafeye girerse tekrar Patrol'a d�n
        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = AIState.Patrol;
        }
        */
    }
    /*


    // Tuzak tetikleme
    void TrapBehavior()
    {
        if (!isTrapTriggered)
        {
            Debug.Log("Trap activated!");
            // Tuzak tetiklendi�inde yap�lacak �eyler
            isTrapTriggered = true;

            // Burada tuza��n animasyonu veya di�er tetikleme olaylar� olabilir.
        }
    }
    */
    // Gizmos ile g�rselle�tirme: Alg�lama alan�
    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);  // Alg�lama alan�n� g�ster
        }
    }
}
