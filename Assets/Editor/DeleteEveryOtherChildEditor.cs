using UnityEditor;
using UnityEngine;

public class DeleteEveryOtherChildEditor : EditorWindow
{
    [MenuItem("Tools/Delete Every Other Child")]
    public static void ShowWindow()
    {
        GetWindow<DeleteEveryOtherChildEditor>("Delete Every Other Child");
    }

    private GameObject parentObject;

    void OnGUI()
    {
        GUILayout.Label("Select Parent Object", EditorStyles.boldLabel);
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);

        if (parentObject != null && GUILayout.Button("Delete Every Other Child"))
        {
            DeleteAlternateChildren();
        }
    }

    void DeleteAlternateChildren()
    {
        if (parentObject == null)
        {
            Debug.LogError("No parent object selected!");
            return;
        }

        Transform parentTransform = parentObject.transform;
        int childCount = parentTransform.childCount;

        // Loop through the children in reverse order to avoid index issues while deleting
        for (int i = childCount - 1; i >= 0; i -= 2)
        {
            DestroyImmediate(parentTransform.GetChild(i).gameObject);
        }

        Debug.Log("Every other child deleted successfully!");
    }
}