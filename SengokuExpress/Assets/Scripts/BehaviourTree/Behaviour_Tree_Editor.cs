using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class Behaviour_Tree_Editor : EditorWindow {
    Behaviour_Tree_View tree_view;
    InspectorView inspector_view;

    [MenuItem("Behaviour_Tree_Editor/Editor ...")]
    public static void OpenWindow()
    {
        Behaviour_Tree_Editor wnd = GetWindow<Behaviour_Tree_Editor>();
        wnd.titleContent = new GUIContent("Behaviour_Tree_Editor");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/BehaviourTree/Behaviour_Tree_Editor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/BehaviourTree/Behaviour_Tree_Editor.uss");
        root.styleSheets.Add(styleSheet);

        tree_view = root.Q<Behaviour_Tree_View>();
        inspector_view = root.Q<InspectorView>();
        tree_view.on_node_selected = on_node_selection_changed;
        OnSelectionChange();
    }

    private void OnSelectionChange() {
        Behaviour_Tree tree = Selection.activeObject as Behaviour_Tree;
        // the commented out section below does not exist in the current version of unity
        if (tree != null /*&& AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID())*/) {
            tree_view.populate_view(tree);
        }
    }

    // ==
    // custom functions
    // ==
    void on_node_selection_changed(Node_View node) {
        inspector_view.update_selection(node);
    }
}