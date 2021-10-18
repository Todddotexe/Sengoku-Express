using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Linq;
using System;

public class AI_Tree_View : GraphView {
    // -- use this for it to show up in the Unity UI Builder and potentially unity editor itself
    public new class UxmlFactory : UxmlFactory<AI_Tree_View, GraphView.UxmlTraits> {}
    // -- fields
    AI_Tree tree;
    public Action<AI_Tree_Node_View> on_node_selected;

    // ==
    // CONSTRUCTOR
    // ==
    public AI_Tree_View() {
        Insert(0, new GridBackground());
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        var style_sheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/AI_Tree/Editor/AI_Tree_Editor.uss");
        styleSheets.Add(style_sheet);
    }
    // ==
    // OVERRIDES
    // ==
    // -- right click option menu displays the nodes we can add
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt) {
        if (tree == null) return;
        if (tree.root == null) return;
        { // -- action methods
            var methods = TypeCache.GetMethodsWithAttribute<AI_Function_Attribute>();
            foreach (var method in methods) {
                evt.menu.AppendAction($"[function] {method.Name}", 
                (a) => create_node(typeof(AI_Tree_Node), method.Name, method.Name));
            }
        }
    }
    //
    public override List<Port> GetCompatiblePorts(Port s, NodeAdapter adapter) {
        AI_Tree_Node_View s_node_view = s.node as AI_Tree_Node_View;
        // don't allow duplicate edges (a node connected to another twice)
        return ports.ToList().Where(end => 
        end.direction != s.direction && 
        end.node != s.node && !s_node_view.node.connection_next.Contains((((AI_Tree_Node_View)end.node).node))).ToList();
    }
    // ==
    // CUSTOM
    // ==
    internal void populate_view(AI_Tree _tree) {
        tree = _tree;
        graphViewChanged -= on_graph_view_changed;
        DeleteElements(graphElements.ToList());
        graphViewChanged += on_graph_view_changed;

        // -- create root node 
        if (tree.root == null) {
            // tree.root = tree.create_node(typeof(AI_Tree_Root)) as AI_Tree_Root;
            var root = tree.init_from_ai_tree_view();
            EditorUtility.SetDirty(tree); // so that changes don't get lost after an assembly reload
            AssetDatabase.SaveAssets();
        }

        // -- create node views in the graph
        tree.nodes.ForEach(n => create_node_view(n));

        // -- create the links between node views
        tree.nodes.ForEach(n => {
            var children = tree.get_children(n);
            children.ForEach(c => {
                AI_Tree_Node_View parent_view = find_node_view(n);
                AI_Tree_Node_View child_view = find_node_view(c);
                Edge edge = parent_view.output.ConnectTo(child_view.input);
                AddElement(edge);
            });
        });
    }
    // find node view from node
    AI_Tree_Node_View find_node_view(AI_Tree_Node node) {
        return GetNodeByGuid(node.guid) as AI_Tree_Node_View;
    }
    // handle deletion and creation of nodes and edges
    GraphViewChange on_graph_view_changed(GraphViewChange graph_view_change) {
        // -- deletion of nodes and links
        if (graph_view_change.elementsToRemove != null) {
            graph_view_change.elementsToRemove.ForEach(e => {
                // -- node view themselves
                AI_Tree_Node_View node_view = e as AI_Tree_Node_View;
                if (node_view != null) {
                    AI_Tree_Root as_root = node_view.node as AI_Tree_Root;
                    if (as_root == null) {  // only delete this node if it's not a root
                        tree.delete_node(node_view.node);
                    }
                }
                // -- links
                Edge edge = e as Edge;
                if (edge != null) {
                    var parent_view = edge.output.node as AI_Tree_Node_View;
                    var child_view = edge.input.node as AI_Tree_Node_View;
                    tree.remove_connection(parent_view.node, child_view.node);
                }
            });
        }
        // -- when connection is made between node views, update the children of the nodes
        if (graph_view_change.edgesToCreate != null) {
            graph_view_change.edgesToCreate.ForEach(e => {
                var parent_view = e.output.node as AI_Tree_Node_View;
                var child_view = e.input.node as AI_Tree_Node_View;
                tree.add_connection(parent_view.node, child_view.node);
                Debug.Log("some connection occurred");
            });
        }
        return graph_view_change;
    }
    // create a node of the given type, and create the assotiated node view
    void create_node(System.Type type, string title, string method_name) {
        AI_Tree_Node node = tree.create_node(type, method_name);
        node.nodeName = title;
        create_node_view(node);
    }
    //
    void create_node_view(AI_Tree_Node node) {
        AI_Tree_Node_View node_view = new AI_Tree_Node_View(node);
        node_view.on_node_selected = on_node_selected;
        AddElement(node_view);
    }
}
