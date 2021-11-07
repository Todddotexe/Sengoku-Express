using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Linq;
using System;

public class AI_Tree_Inspector : VisualElement {
    // -- use this for it to show up in the Unity UI Builder and potentially unity editor itself
    public new class UxmlFactory : UxmlFactory<AI_Tree_Inspector, VisualElement.UxmlTraits> {}
    Editor editor;

    public void update_selection(AI_Tree_Node_View node_view) {
        Clear();
        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(node_view.node);
        IMGUIContainer container = new IMGUIContainer( () => {
            editor.OnInspectorGUI();
        });
        Add(container);
    }
}
