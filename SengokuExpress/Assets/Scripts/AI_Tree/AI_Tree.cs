﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        node.guid = GUID.Generate().ToString();
        
        node.method_name = method_name;
        node.tree = this;
        nodes.Add(node);

        AssetDatabase.AddObjectToAsset(node, this); // add this node to the asset viewed in the folders
        AssetDatabase.SaveAssets();
        return node;
    }
    //
    public void delete_node(AI_Tree_Node node) {
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
        tree.root = (AI_Tree_Root)root.clone();
        return tree;
    }
}
