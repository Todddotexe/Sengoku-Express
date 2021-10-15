using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class AI_Tree : ScriptableObject {
    public AI_Tree_Node root = null;
    public List<AI_Tree_Node> nodes = new List<AI_Tree_Node>();
    // run update on roots
    public void update() {
        root.update();
    }
    //
    public AI_Tree_Node create_node(System.Type type) {
        AI_Tree_Node node = ScriptableObject.CreateInstance(type) as AI_Tree_Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        
        nodes.Add(node);
        if (root == null) root = node;

        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }
    //
    public void delete_node(AI_Tree_Node node) {
        if (node == root) {
            root = null;
            if (node.connection_next.Count > 0) root = (AI_Tree_Node)node.connection_next[0];
            else if (nodes.Count > 0) root = (AI_Tree_Node)nodes[0];
        }
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }
    //
    public void add_connection(AI_Tree_Node parent, AI_Tree_Node child) {
        parent.add_connection(child);
    }
    //
    public void remove_connection(AI_Tree_Node parent, AI_Tree_Node child) {
        parent.remove_connection(child);
    }
    //
    public List<AI_Tree_Node> get_children(AI_Tree_Node parent) {
        return parent.connection_next;
    } 
    //
    public AI_Tree clone() {
        AI_Tree tree = Instantiate(this);
        //tree.roots = tree.roots.ConvertAll(c => c.clone());
        tree.root = root.clone();
        return tree;
    }
}
