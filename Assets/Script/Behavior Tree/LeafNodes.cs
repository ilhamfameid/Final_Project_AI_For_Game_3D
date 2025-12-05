using UnityEngine;
using UnityEngine.AI;

public class CheckPlayerInRange : Node {
    private Transform npc, player;
    private float range;

    public CheckPlayerInRange(Transform npc, Transform player, float range) {
        this.npc = npc;
        this.player = player;
        this.range = range;
    }

    public override NodeState Evaluate() {
        float dist = Vector3.Distance(npc.position, player.position);
        return (dist < range) ? NodeState.Success : NodeState.Failure;
    }
}

public class MoveToPlayer : Node {
    private NavMeshAgent agent;
    private Transform player;

    public MoveToPlayer(NavMeshAgent agent, Transform player) {
        this.agent = agent;
        this.player = player;
    }

    public override NodeState Evaluate() {
        agent.SetDestination(player.position);
        return NodeState.Running;
    }
}

public class Patrol : Node {
    private NavMeshAgent agent;
    private Transform[] waypoints;
    private int currentIndex = 0;

    public Patrol(NavMeshAgent agent, Transform[] waypoints) {
        this.agent = agent;
        this.waypoints = waypoints;
    }

    public override NodeState Evaluate() {
        if (waypoints.Length == 0) return NodeState.Failure;

        if (!agent.pathPending && agent.remainingDistance < 0.5f) {
            currentIndex = (currentIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentIndex].position);
        }

        return NodeState.Running;
    }
}
