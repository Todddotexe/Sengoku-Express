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
}

public abstract class BT_Node : ScriptableObject {
    public enum States {
        run, ok, fail,
    }
    public States state = States.run;
    public bool started = false;
    public string guid;

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

    protected abstract void on_start();
    protected abstract void on_stop();
    protected abstract States on_update();
}

public abstract class BT_Action_Node : BT_Node {}

public abstract class BT_Decorator_Node : BT_Node {
    public BT_Node child;
}

public abstract class BT_Composite_Node : BT_Node {
    public List<BT_Node> children = new List<BT_Node>();
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
            } break;
            case States.fail: {
                return States.fail;
            } break;
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