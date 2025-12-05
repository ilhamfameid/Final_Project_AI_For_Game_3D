using System.Collections.Generic;

public class Selector : Node {
    private List<Node> children;

    public Selector(List<Node> children) {
        this.children = children;
    }

    public override NodeState Evaluate() {
        foreach (Node node in children) {
            var state = node.Evaluate();
            if (state == NodeState.Success) return NodeState.Success;
            if (state == NodeState.Running) return NodeState.Running;
        }
        return NodeState.Failure;
    }
}

public class Sequence : Node {
    private List<Node> children;

    public Sequence(List<Node> children) {
        this.children = children;
    }

    public override NodeState Evaluate() {
        bool running = false;
        foreach (Node node in children) {
            var state = node.Evaluate();
            if (state == NodeState.Failure) return NodeState.Failure;
            if (state == NodeState.Running) running = true;
        }
        return running ? NodeState.Running : NodeState.Success;
    }
}
