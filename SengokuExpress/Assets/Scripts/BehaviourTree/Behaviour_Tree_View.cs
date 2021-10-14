using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Linq;
using System;

public class Behaviour_Tree_View : GraphView { // up to 47:17 following along the vid AI #11
    // -- use this for it to show up in the Unity UI Builder and potentially unity editor itself
    public new class UxmlFactory : UxmlFactory<Behaviour_Tree_View, GraphView.UxmlTraits> {}
    // -- fields
    Behaviour_Tree tree;
    public Action<Node_View> on_node_selected;

    // ===
    // CONSTRUCTOR
    // ===
    public Behaviour_Tree_View() {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        //focusable = true;

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviourTree/Behaviour_Tree_Editor.uss");
        styleSheets.Add(styleSheet);
    }

    // ===
    // OVERRIDES
    // ===
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
        { // -- Action Node
            var types = TypeCache.GetTypesDerivedFrom<BT_Action_Node>();
            foreach(var type in types) {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => create_node(type));
            }
        }
        { // -- Composite Node
            var types = TypeCache.GetTypesDerivedFrom<BT_Composite_Node>();
            foreach(var type in types) {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => create_node(type));
            }
        }
        { // -- Decorator Node
            var types = TypeCache.GetTypesDerivedFrom<BT_Decorator_Node>();
            foreach(var type in types) {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => create_node(type));
            }
        }
    }

    // we need this to be able to link our nodes
    public override List<Port> GetCompatiblePorts(Port start_port, NodeAdapter node_adapter) {
        return ports.ToList().Where(end_port => 
        end_port.direction != start_port.direction && 
        end_port.node != start_port.node).ToList();
    }

    // ==
    // BEHAVIOUR TREE VIEW FUNCTIONS
    // ==

    /// populate the editor based on the selected tree
    internal void populate_view(Behaviour_Tree _tree) {
        tree = _tree;
        graphViewChanged -= on_graph_view_changed;
        DeleteElements(graphElements.ToList());
        graphViewChanged += on_graph_view_changed;

        // -- create root node
        if (tree.root == null) {
            tree.root = tree.create_node(typeof(BT_Root_Node)) as BT_Root_Node;
            EditorUtility.SetDirty(tree); // so that changes don't get lost after an assembly reload
            AssetDatabase.SaveAssets();
        }

        // -- create node views in the graph
        tree.nodes.ForEach(n => create_node_view(n));

        // -- create the links between node views
        tree.nodes.ForEach(n => {
            var children = tree.get_children(n);
            children.ForEach(c => {
                Node_View parent_view = find_node_view(n);
                Node_View child_view = find_node_view(c);
                Edge edge = parent_view.output.ConnectTo(child_view.input);
                AddElement(edge);
            });
        });

    }

    /// find node view from node
    Node_View find_node_view(BT_Node node) {
        return GetNodeByGuid(node.guid) as Node_View;
    }

    private GraphViewChange on_graph_view_changed(GraphViewChange graphViewChange) {
        // deletion of nodes and links
        if (graphViewChange.elementsToRemove != null) {
            graphViewChange.elementsToRemove.ForEach(elem => {
                // -- node view themselves
                Node_View node_view = elem as Node_View;
                if (node_view != null) {
                    tree.delete_node(node_view.node);
                }

                // -- links
                Edge edge = elem as Edge;
                if (edge != null) {
                    Node_View parent_view = edge.output.node as Node_View;
                    Node_View child_view = edge.input.node as Node_View;
                    tree.remove_child(parent_view.node, child_view.node);
                }
            });
        }

        // when connection is made between node views, update the children of the nodes
        if (graphViewChange.edgesToCreate != null) {
            graphViewChange.edgesToCreate.ForEach(edge => {
                Node_View parent_view = edge.output.node as Node_View;
                Node_View child_view = edge.input.node as Node_View;
                tree.add_child(parent_view.node, child_view.node);
            });
        }
        return graphViewChange;
    }

    void create_node(System.Type type) {
        BT_Node node = tree.create_node(type);
        create_node_view(node);
    }

    void create_node_view(BT_Node node) {
        Node_View node_view = new Node_View(node);
        node_view.on_node_selected = on_node_selected;
        AddElement(node_view);
    }
} // -------------------- end of Behaviour Tree View -------------------- 

// public class SplitView : TwoPaneSplitView {
//     public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> {}
// }
public class SplitView : VisualElement {
    public new class UxmlFactory : UxmlFactory<SplitView, VisualElement.UxmlTraits> {}
    public SplitView() {
    }
}

public class InspectorView : VisualElement {
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> {}
    Editor editor;
    public InspectorView() {}

    public void update_selection(Node_View node_view) {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(node_view.node);
        IMGUIContainer container = new IMGUIContainer( () => {
            editor.OnInspectorGUI();
        });
        Add(container);
    }
}

public class Node_View : UnityEditor.Experimental.GraphView.Node {
    public Action<Node_View> on_node_selected;
    public BT_Node node;
    public Port input;
    public Port output;

    public Node_View(BT_Node _node) {
        node = _node;
        this.title = node.name;
        this.viewDataKey = node.guid;
        this.style.left = node.graph_pos.x;
        this.style.top = node.graph_pos.y;

        create_input_ports();
        create_output_ports();
    }

    public override void SetPosition(Rect new_pos) {
        base.SetPosition(new_pos);
        node.graph_pos.x = new_pos.xMin;
        node.graph_pos.y = new_pos.yMin;
    }

    void create_input_ports() { // @incomplete remove this stupidity of checking what type the node is
        if (node is BT_Action_Node) {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        } else if (node is BT_Composite_Node) {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        } else if (node is BT_Decorator_Node) {
            input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
        } else if (node is BT_Root_Node) {
            // no input so leave blank
        }

        if (input != null) {
            input.portName = "";
            inputContainer.Add(input);
        }
    }

    void create_output_ports() {
        if (node is BT_Action_Node) {
            // action node doesn't have any children, so leave this blank. @incomplete get rid of all types of nodes and just have our plain old BT_Node that does what we want wtf people
        } else if (node is BT_Composite_Node) {
            // multi output
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        } else if (node is BT_Decorator_Node) {
            // single output
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        } else if (node is BT_Root_Node) {
            // single output
            output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (output != null) {
            output.portName = "";
            outputContainer.Add(output);
        }
    }

    public override void OnSelected() {
        base.OnSelected();
        if (on_node_selected != null) {
            on_node_selected.Invoke(this);
        }
    }
}
