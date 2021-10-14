using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class Behaviour_Tree : ScriptableObject {
    public BT_Node root;
    public BT_Node.States state = BT_Node.States.run;
    public List<BT_Node> nodes = new List<BT_Node>();

    public BT_Node.States update() {
        if (root.state == BT_Node.States.run) {
            state = root.update();
        }
        return state;
    }

    public BT_Node create_node(System.Type type) {
        BT_Node node = ScriptableObject.CreateInstance(type) as BT_Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void delete_node(BT_Node node) {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void add_child(BT_Node parent, BT_Node child) { // @incomplete get rid of diff types
        BT_Decorator_Node decorator = parent as BT_Decorator_Node;
        if (decorator != null) {
            decorator.child = child;
        }

        // @incomplete remove all dis diff kinds of nodes wtf people
        BT_Root_Node root = parent as BT_Root_Node;
        if (root != null) {
            root.child = child;
        }

        BT_Composite_Node composite = parent as BT_Composite_Node;
        if (composite != null) {
            composite.children.Add(child);
        }
    }
    public void remove_child(BT_Node parent, BT_Node child) {
        BT_Decorator_Node decorator = parent as BT_Decorator_Node;
        if (decorator != null) {
            decorator.child = null;
        }

        // @incomplete remove all dis diff kinds of nodes wtf people
        BT_Root_Node root = parent as BT_Root_Node;
        if (root != null) {
            root.child = null;
        }

        BT_Composite_Node composite = parent as BT_Composite_Node;
        if (composite != null) {
            composite.children.Remove(child);
        }
    }

    public List<BT_Node> get_children(BT_Node parent) {
        List<BT_Node> result = new List<BT_Node>();
        BT_Decorator_Node decorator = parent as BT_Decorator_Node;
        if (decorator != null && decorator.child != null) {
            result.Add(decorator.child);
        }

        // @incomplete remove all dis diff kinds of nodes wtf people
        BT_Root_Node root = parent as BT_Root_Node;
        if (root != null && root.child != null) {
            result.Add(root.child);
        }

        BT_Composite_Node composite = parent as BT_Composite_Node;
        if (composite != null) {
            return composite.children;
        }
        return result;
    }

    public Behaviour_Tree clone() {
        Behaviour_Tree tree = Instantiate(this);
        tree.root = tree.root.clone();
        return tree;
    }
} // === END OF BEHAVIOUR TREE ===

public class BT_Root_Node : BT_Node {   // @incomplete remove this crap
    public BT_Node child;

    protected override void on_start() {}
    protected override void on_stop() {}
    protected override States on_update() {
        return child.update();
    }

    public override BT_Node clone() {
        BT_Root_Node node = Instantiate(this);
        node.child = child.clone();
        return node;
    }
}

public abstract class BT_Node : ScriptableObject {
    public enum States {
        run, ok, fail,
    }

    [HideInInspector] public States state = States.run;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 graph_pos; // used for the editor
    
    public States update() {
        if (!started) {
            on_start();
            started = true;
        }

        state = on_update();

        if (state == States.ok || state == States.fail) {
            on_stop();
            started = false;
        }
        return state;
    }

    // used to clone this node, so if multiple assets use the same ref to the same behaviour tree, they will have their own and run their own
    public virtual BT_Node clone() {
        return Instantiate(this);
    }

    protected abstract void on_start();
    protected abstract void on_stop();
    protected abstract States on_update();
}

public abstract class BT_Action_Node : BT_Node {}

public abstract class BT_Decorator_Node : BT_Node {
    [HideInInspector] public BT_Node child;

    public override BT_Node clone() {
        BT_Decorator_Node node = Instantiate(this);
        node.child = child.clone();
        return node;
    }
}

public abstract class BT_Composite_Node : BT_Node {
    [HideInInspector] public List<BT_Node> children = new List<BT_Node>();
    public override BT_Node clone() {
        BT_Composite_Node node = Instantiate(this);
        node.children = children.ConvertAll(c => c.clone());
        return node;
    }
}

public class BT_Debug_Node : BT_Action_Node {
    public string message;
    protected override void on_start() {
        Debug.Log($"on_start {message}");
    }

    protected override void on_stop() {
        Debug.Log($"on_stop {message}");
    }

    protected override States on_update() {
        Debug.Log($"on_update {message}");
        return States.ok;
    }
}

public class BT_Repeat_Node : BT_Decorator_Node {
    protected override void on_start() {
    }

    protected override void on_stop() {
    }

    protected override States on_update() {
        child.update();
        return States.run;
    }
}

/// if one of the children fails, the whole tree fails. returns succeed if all children succeed
public class BT_Sequencer_Node : BT_Composite_Node {
    int current;
    protected override void on_start() {
        current = 0;
    }

    protected override void on_stop() {
    }

    protected override States on_update() {
        var child = children[current];
        switch (child.update()) {
            case States.run: {
                return States.run;
            }// break;
            case States.fail: {
                return States.fail;
            }// break;
            case States.ok: {
                current++;
            } break;
        }
        return current == children.Count ? States.ok : States.run;
    }
}

public class BT_Wait_Node : BT_Action_Node {
    public float duration = 1;
    float start_time;
    protected override void on_start() {
        start_time = Time.time;
    }

    protected override void on_stop() {
    }

    protected override States on_update() {
        if (Time.time - start_time > duration) {
            return States.ok;
        }
        return States.run;
    }
}