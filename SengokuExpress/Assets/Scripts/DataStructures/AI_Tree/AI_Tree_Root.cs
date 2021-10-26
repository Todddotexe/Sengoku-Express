using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Tree_Root : AI_Tree_Node {
	public override void update(MonoBehaviour caller) {
		connection_ok.ForEach(c => c.update(caller));
	}
} // just exists so we can have a root node that doesn't get deleted. We might get rid of this and just add a bool to AI_Tree_Node