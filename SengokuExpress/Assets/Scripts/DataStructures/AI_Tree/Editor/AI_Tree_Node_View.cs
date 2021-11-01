using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Linq;
using System;

public class AI_Tree_Node_View : UnityEditor.Experimental.GraphView.Node {
    public static string PORT_OK = "OK";
    public static string PORT_FAIL = "FAIL";

    public Action<AI_Tree_Node_View> on_node_selected;
    public AI_Tree_Node node;
    public Port input;
    public Port output_ok;
    public Port output_fail;

    public AI_Tree_Node_View(AI_Tree_Node _node) {
        node = _node;
        this.title = $"[{node.name}] {node.nodeName}"; // update title
        this.viewDataKey = node.guid;
        this.style.left = node.graph_pos.x;
        this.style.top = node.graph_pos.y;

        create_input_ports();
        create_output_ports();
    }

    // ==
    // OVERRIDES
    // ==
    // set the positions of nodes
    public override void SetPosition(Rect new_pos) {
        base.SetPosition(new_pos);
        node.graph_pos.x = new_pos.xMin;
        node.graph_pos.y = new_pos.yMin;
    }
    // allow the selection of nodes
    public override void OnSelected() {
        base.OnSelected();
        if (on_node_selected != null) {
            on_node_selected.Invoke(this);
        }
    }
    // ==
    // CUSTOM
    // ==
    // input ports
    void create_input_ports() {
        input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
        if (input != null) {
            input.portName = "input";
            inputContainer.Add(input);
        }
    }
    // output ports
    void create_output_ports() {
        output_ok = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        output_fail = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
        if (output_ok != null) {
            output_ok.portName = PORT_OK;
            outputContainer.Add(output_ok);
        }
        if (output_fail != null) {
            output_fail.portName = PORT_FAIL;
            outputContainer.Add(output_fail);
        }
    }
}
