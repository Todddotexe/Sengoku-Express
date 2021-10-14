using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// @debug for debugging purposes
public class Behaviour_Tree_Runner : MonoBehaviour {
    public Behaviour_Tree tree;

    void Start() {
        tree = tree.clone();    // VERY IMPORTANT TO CLONE THE TREE ASSET
        // tree = ScriptableObject.CreateInstance<Behaviour_Tree>();

        // var log1 = ScriptableObject.CreateInstance<BT_Debug_Node>();
        // log1.message = "test message 1";
        
        // var wait = ScriptableObject.CreateInstance<BT_Wait_Node>();

        // var log2 = ScriptableObject.CreateInstance<BT_Debug_Node>();
        // log2.message = "test message 2";
        // var log3 = ScriptableObject.CreateInstance<BT_Debug_Node>();
        // log3.message = "test message 3";

        // var seq = ScriptableObject.CreateInstance<BT_Sequencer_Node>();
        // seq.children.Add(log1);
        // seq.children.Add(wait);
        // seq.children.Add(log2);
        // seq.children.Add(log3);

        // tree.root = seq;
    }

    void Update() {
        tree.update();
    }
}
