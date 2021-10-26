using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu()]
public class AI_Tree : ScriptableObject {
    // public bool current_state = true; // true means OK, false means FAIL
    public AI_Tree_Root root = null;
    public List<AI_Tree_Node> nodes = new List<AI_Tree_Node>();
    
    public AI_Tree_Root init_from_ai_tree_view() {
        // create root
        root = create_node(typeof(AI_Tree_Root), null) as AI_Tree_Root; // can only call create_node from graph view because it calls ScriptableObject
        return root; // have to return root in order to make it persistant wtf,, ai_tree_view
    }
    // run update on roots
    public void update(MonoBehaviour caller) {
        root.update(caller);
    }
    //
    public AI_Tree_Node create_node(System.Type type, string method_name) { // TODO set delegates
        AI_Tree_Node node = ScriptableObject.CreateInstance(type) as AI_Tree_Node;
        node.name = type.Name;

        #if UNITY_EDITOR
        node.guid = GUID.Generate().ToString();
        #endif

        node.method_name = method_name;
        node.tree = this;
        nodes.Add(node);

        #if UNITY_EDITOR
        AssetDatabase.AddObjectToAsset(node, this); // add this node to the asset viewed in the folders
        AssetDatabase.SaveAssets();
        #endif

        return node;
    }
    //
    public void delete_node(AI_Tree_Node node) {
        nodes.Remove(node);
        #if UNITY_EDITOR
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
        #endif
    }
    //
    public void add_connection_ok(AI_Tree_Node parent, AI_Tree_Node child) {
        parent.add_connection_ok(child);
    }
    //
    public void remove_connection_ok(AI_Tree_Node parent, AI_Tree_Node child) {
        parent.remove_connection_ok(child);
    }
    //
    public void add_connection_fail(AI_Tree_Node parent, AI_Tree_Node child) {
        parent.add_connection_fail(child);
    }
    //
    public void remove_connection_fail(AI_Tree_Node parent, AI_Tree_Node child) {
        parent.remove_connection_fail(child);
    }
    //
    public List<AI_Tree_Node> get_connection_ok(AI_Tree_Node parent) {
        return parent.connection_ok;
    } 
    //
    public List<AI_Tree_Node> get_connection_fail(AI_Tree_Node parent) {
        return parent.connection_fail;
    } 
    //
    public AI_Tree clone() {
        AI_Tree tree = Instantiate(this);
        //tree.roots = tree.roots.ConvertAll(c => c.clone());
        tree.root = (AI_Tree_Root)root.clone();
        return tree;
    }
}
