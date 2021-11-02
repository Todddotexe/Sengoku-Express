using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AI_Tree_Node : ScriptableObject {
    public List<AI_Tree_Node> connection_ok = new List<AI_Tree_Node>();
    public List<AI_Tree_Node> connection_fail = new List<AI_Tree_Node>();
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 graph_pos; // used for the editor
    public string nodeName; // for debugging purposes and clarity in the editor
    public string method_name = null;
    public AI_Tree tree = null;

    public virtual void update(MonoBehaviour caller) {
        Debug.Assert(tree != null);
        
        if (method_name != null) {
            var method = caller.GetType().GetMethod(method_name, 
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            bool result = (bool)method.Invoke(caller, null);
            // Debug.Log("AI TREE CURRENTLY RUNNING: " + method_name);
            if (result == true) {
                connection_ok.ForEach(c => c.update(caller));
            } else {
                connection_fail.ForEach(c => c.update(caller));
            }
        }
    }
    
    // add a new _node as a connection to this node
    public void add_connection_ok(AI_Tree_Node _node) {
        connection_ok.Add(_node);
    }

    // add remove the connection between this node and _node
    public void remove_connection_ok(AI_Tree_Node _node) {
        connection_ok.Remove(_node);
    }

    // add a new _node as a connection to this node
    public void add_connection_fail(AI_Tree_Node _node) {
        connection_fail.Add(_node);
    }

    // add remove the connection between this node and _node
    public void remove_connection_fail(AI_Tree_Node _node) {
        connection_fail.Remove(_node);
    }

    public AI_Tree_Node clone() {
        AI_Tree_Node node = Instantiate(this);
        node.connection_ok = connection_ok.ConvertAll(c => c.clone());
        node.connection_fail = connection_fail.ConvertAll(c => c.clone());
        return node;
    }
}