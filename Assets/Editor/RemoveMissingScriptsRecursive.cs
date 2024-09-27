using UnityEditor;
using UnityEngine;

using UnityEngine;
using UnityEditor;

public class RemoveMissingScriptsRecursive : EditorWindow
{
    [MenuItem("Tools/Remove Missing Scripts Recursively")]
    public static void ShowWindow()
    {
        var window = GetWindow<RemoveMissingScriptsRecursive>();
        window.titleContent = new GUIContent("Remove Missing Scripts Recursively");
        window.Show();
    }

    void OnGUI()
    {
        if (GUILayout.Button("Remove Missing Scripts from Selected GameObjects and Their Children"))
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                RemoveScriptsRecursively(go);
            }
            Debug.Log("Missing scripts removed from selected GameObjects and their children.");
        }
    }

    private static void RemoveScriptsRecursively(GameObject go)
    {
        RemoveMissingScript(go); // GameObject에 있는 missing script를 삭제하기

        // 재귀적으로 모든 자식을 조회하기
        foreach (Transform child in go.transform)
        {
            RemoveScriptsRecursively(child.gameObject);
        }
    }

    private static void RemoveMissingScript(GameObject go)
    {
        var components = go.GetComponents<Component>();
        var serializedObject = new SerializedObject(go);
        var prop = serializedObject.FindProperty("m_Component");

        // 반복 루프를 위한 Counter
        int r = 0; 

        for (int j = 0; j < components.Length; j++)
        {
            if (components[j] == null)
            {
                prop.DeleteArrayElementAtIndex(j - r);
                r++;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}