using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AI_Tree_Node : ScriptableObject {
    public List<AI_Tree_Node> connection_next = new List<AI_Tree_Node>();
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 graph_pos; // used for the editor
    public string nodeName; // for debugging purposes and clarity in the editor
    public abstract void update();

    // add a new _node as a connection to this node
    public void add_connection(AI_Tree_Node _node) {
        connection_next.Add(_node);
    }

    // add remove the connection between this node and _node
    public void remove_connection(AI_Tree_Node _node) {
        connection_next.Remove(_node);
    }

    public AI_Tree_Node clone() {
        AI_Tree_Node node = Instantiate(this);
        node.connection_next = connection_next.ConvertAll(c => c.clone());
        return node;
    }
}

public class AI_Tree_Action : AI_Tree_Node {
    public delegate void Action();
    public UnityEvent action_event;
    public Action action = null;

    // run action if not null. Then update connection_next
	public override void update() {
        if (action != null) action();
        connection_next.ForEach(c => c.update());
	}
}

public class AI_Tree_Condition : AI_Tree_Node {
    public delegate bool Condition();
    public UnityEvent condition_event;
    public Condition condition = null;
    
    // run condition if not null. if true, run update on connection_next. else, return
    public override void update() {
        condition_event.AddListener(simp);
        if (condition == null) return;
        // if condition result is true, continue down connection_next
        if (condition() == true) connection_next.ForEach(c => c.update());
    }

    void simp() {}
}