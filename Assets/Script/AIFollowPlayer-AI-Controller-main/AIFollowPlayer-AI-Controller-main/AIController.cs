using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour {
    private GameObject player;
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Settings")]
    public float chaseRange = 5f;
    public Transform[] waypoints;

    [Header("Probability Settings")]
    public float stopProbability = 0.3f;
    public float minStopTime = 1f;
    public float maxStopTime = 2f;

    private int currentWaypoint = 0;
    private bool isStopped = false;
    private float stopTimer = 0f;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if (waypoints.Length > 0)
            GoToNextWaypoint();
    }

    void Update() {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

    
        if (distance < chaseRange) { // chase
            agent.speed = 3.5f;
            agent.isStopped = false;
            isStopped = false;

            agent.SetDestination(player.transform.position);
        }

       
        else {
            PatrolBehaviour(); // patrol
        }

        // Animator speed
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    void PatrolBehaviour() {
        // Jika sedang idle
        if (isStopped) {
            stopTimer -= Time.deltaTime;

            if (stopTimer <= 0f) {
                isStopped = false;
                agent.isStopped = false;
                GoToNextWaypoint();
            }
            return;
        }

        // Jika sudah sampai waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.5f) {

            // Cek probabilitas berhenti
            if (Random.value < stopProbability) {
                isStopped = true;
                agent.isStopped = true;
                stopTimer = Random.Range(minStopTime, maxStopTime);
                return;
            }

            // Kalau tidak berhenti, lanjut waypoint
            GoToNextWaypoint();
        }
    }

    void GoToNextWaypoint() {
        if (waypoints.Length == 0) return;

        agent.isStopped = false;
        agent.speed = 2f;

        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypoint].position);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
