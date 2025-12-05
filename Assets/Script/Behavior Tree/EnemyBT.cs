using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBT : MonoBehaviour {
    private Node root;
    private NavMeshAgent agent;

    [Header("References")]
    public Transform player;
    public Transform[] waypoints;

    void Start() {
        agent = GetComponent<NavMeshAgent>();

        // Patrol Node
        Node patrol = new Patrol(agent, waypoints);

        // Chase Sequence (cek player dekat ? kejar)
        Node checkRange = new CheckPlayerInRange(transform, player, 5f);
        Node chase = new MoveToPlayer(agent, player);
        Sequence chaseSeq = new Sequence(new List<Node> { checkRange, chase });

        // Root Selector ? kalau bisa chase ? kejar, kalau tidak patrol
        root = new Selector(new List<Node> { chaseSeq, patrol });
    }

    void Update() {
        root.Evaluate();
    }
}
