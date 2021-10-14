/// Author: Matin Kamali
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// trying to create a behaviour tree so that the designers can chain attack moves, or design AI behaviour on enemies
public class Behaviour_Tree {
    /// conditions a node can return
    public enum CONDITIONS {
            OK,
            FAIL,
            PENDING,
    }

    public delegate CONDITIONS Execute_Delegate();

    public abstract class Node {
        public Execute_Delegate execution_code;
        public abstract void add(Node _node);

        public Behaviour_Tree.CONDITIONS execute() {
            return execution_code();
        }
    } // for polymorphism

    class Composite : Node {
        List<Node> nodes;
        public override void add(Node _node) {
            nodes.Add(_node);
        }
    }

    class Action : Node {
        Node next = null;
        public override void add(Node _node) {
            next = _node;
        }
    }

    class Condition : Node {
        Node next = null;
        public override void add(Node _node) {
            next = _node;
        }
    }
    
    //
    // fields
    //
    public Node root = new Composite();

    //
    // public methods
    //

    public void add_composition(Node _parent) {
        Composite c = new Composite();
        _parent.add(c);
    }

    public void add_action(Node _parent, Execute_Delegate _action_delegate) {
        Action a = new Action();
        a.execution_code += _action_delegate;
    }

    public void add_condition(Node _parent, Execute_Delegate _condition_delegate) {
        Condition c = new Condition();
        c.execution_code = _condition_delegate;
        _parent.add(c);
    }
    
    // update decisions and decide what action must be performed this frame
    int update() {
        Node current = root;
        
        while (current != null) {

        }
        
        return (int)CONDITIONS.FAIL;
    }
}
