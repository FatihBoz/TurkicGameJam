using UnityEngine;

public class AI : MonoBehaviour
{
    public enum AIState { Patrol, Alert, Trap }
    public AIState currentState;

    public Transform patrolParent;
    public Transform[] patrolPoints;  // Yapay zekanýn gideceði noktalara dizilmiþ Patrol noktalarý
    private int currentPatrolIndex = 0;  // Þu anda gidilen patrol noktasý

    public Transform player;  // Oyuncu referansý
    public float detectionRange = 10f;  // Oyuncuyu algýlama mesafesi
    public float moveSpeed = 3f;  // Hareket hýzý

    public Transform trapTriggerArea;  // Tuzak tetikleme bölgesi
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
        // parent'teki tüm çocuklarý al
        patrolPoints = new Transform[patrolParent.childCount];

        // Her çocuðu patrol noktasý olarak ata
        for (int i = 0; i < patrolParent.childCount; i++)
        {
            patrolPoints[i] = patrolParent.GetChild(i);
        }
    }

    // Patrol Yapma
    void PatrolBehavior()
    {
        // Patrol noktasýna doðru hareket et
        if (patrolPoints.Length > 0)
        {
            Transform targetPoint = patrolPoints[currentPatrolIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

            // Eðer hedef noktaya ulaþýldýysa, bir sonraki noktaya geç
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
    
    // Alarm durumunda yapýlacaklar
    void AlertBehavior()
    {
        // Oyuncuya doðru hareket et
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        /*
        // Eðer oyuncu Trap bölgesine girerse, tuzaðý tetikle
        if (Vector3.Distance(transform.position, trapTriggerArea.position) < 2f)
        {
            currentState = AIState.Trap;
        }

        // Eðer oyuncu belirli bir mesafeye girerse tekrar Patrol'a dön
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
            // Tuzak tetiklendiðinde yapýlacak þeyler
            isTrapTriggered = true;

            // Burada tuzaðýn animasyonu veya diðer tetikleme olaylarý olabilir.
        }
    }
    */
    // Gizmos ile görselleþtirme: Algýlama alaný
    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);  // Algýlama alanýný göster
        }
    }
}
