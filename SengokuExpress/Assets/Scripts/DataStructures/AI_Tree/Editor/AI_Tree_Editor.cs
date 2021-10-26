using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Callbacks;

public class AI_Tree_Editor : EditorWindow
{
    AI_Tree_View tree_view;
    AI_Tree_Inspector inspector_view;

    [MenuItem("AI_Tree_Editor/Editor")]
    public static void OpenWindow()
    {
        AI_Tree_Editor wnd = GetWindow<AI_Tree_Editor>();
        wnd.titleContent = new GUIContent("AI_Tree_Editor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instance_id, int line) {
        if (Selection.activeObject is AI_Tree) {
            OpenWindow();
            return true;
        }
        return false;
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/DataStructures/AI_Tree/Editor/AI_Tree_Editor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/DataStructures/AI_Tree/Editor/AI_Tree_Editor.uss");
        root.styleSheets.Add(styleSheet);

        // -- setup editor for me
        tree_view = root.Q<AI_Tree_View>();
        inspector_view = root.Q<AI_Tree_Inspector>();
        tree_view.on_node_selected = on_node_selection_changed;
        OnSelectionChange();
    }

    void OnSelectionChange() {
        AI_Tree tree = Selection.activeObject as AI_Tree;
        if (tree != null) {
            tree_view.populate_view(tree);
        } else {
            // if a tree was not selected
            inspector_view.Clear();
        }
    }

    void on_node_selection_changed(AI_Tree_Node_View node_view) {
        inspector_view.update_selection(node_view);
    }
}