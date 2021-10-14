using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;

public class Behaviour_Tree_View : GraphView { // up to 47:17 following along the vid AI #11
    public new class UxmlFactory : UxmlFactory<Behaviour_Tree_View, GraphView.UxmlTraits> {}
    Behaviour_Tree tree;
    public Behaviour_Tree_View() {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviourTree/Behaviour_Tree_Editor.uss");
        styleSheets.Add(styleSheet);
    }

    internal void populate_view(Behaviour_Tree _tree) {
        tree = _tree;
        graphViewChanged -= on_graph_view_changed;
        DeleteElements(graphElements.ToList());
        graphViewChanged += on_graph_view_changed;

        tree.nodes.ForEach(n => create_node_view(n));
    }

    private GraphViewChange on_graph_view_changed(GraphViewChange graphViewChange) {
        if (graphViewChange.elementsToRemove != null) {
            graphViewChange.elementsToRemove.ForEach(elem => {
                Node_View node_view = elem as Node_View;
                if (node_view != null) {
                    tree.delete_node(node_view.node);
                }
            });
        }
        return graphViewChange;
    }

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

    void create_node(System.Type type) {
        BT_Node node = tree.create_node(type);
        create_node_view(node);
    }

    void create_node_view(BT_Node node) {
        Node_View node_view = new Node_View(node);
        AddElement(node_view);
    }
}

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
    public InspectorView() {
    }
}

public class Node_View : UnityEditor.Experimental.GraphView.Node {
    public BT_Node node;
    public Node_View(BT_Node _node) {
        node = _node;
        title = node.name;
    }
}
