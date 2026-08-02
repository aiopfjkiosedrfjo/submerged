using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class FontChanger : EditorWindow
{
    private TMP_FontAsset newFont;

    [MenuItem("Tools/TMP Font Replacer")]
    public static void ShowWindow()
    {
        GetWindow<FontChanger>("Font Changer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Replace All Fonts in Active Scene", EditorStyles.boldLabel);
        
        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font Asset", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Replace All Fonts"))
        {
            if (newFont == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a New Font Asset first!", "OK");
                return;
            }

            ReplaceFonts();
        }
    }

    private void ReplaceFonts()
    {
        TextMeshProUGUI[] uiTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        TextMeshPro[] worldTexts = Resources.FindObjectsOfTypeAll<TextMeshPro>();

        int changedCount = 0;
        Scene activeScene = SceneManager.GetActiveScene();

        foreach (var text in uiTexts)
        {
            if (text.gameObject.scene == activeScene)
            {
                Undo.RecordObject(text, "Replace TMP Font");
                text.font = newFont;
                EditorUtility.SetDirty(text);
                changedCount++;
            }
        }

        foreach (var text in worldTexts)
        {
            if (text.gameObject.scene == activeScene)
            {
                Undo.RecordObject(text, "Replace TMP Font");
                text.font = newFont;
                EditorUtility.SetDirty(text);
                changedCount++;
            }
        }

        if (changedCount > 0)
        {
            EditorSceneManager.MarkSceneDirty(activeScene);
            Debug.Log($"Successfully replaced fonts on {changedCount} TextMeshPro objects.");
            EditorUtility.DisplayDialog("Success", $"Replaced fonts on {changedCount} objects!", "OK");
        }
        else
        {
            Debug.LogWarning("No TextMeshPro objects found in the active scene.");
            EditorUtility.DisplayDialog("Finished", "No TextMeshPro objects found in this scene.", "OK");
        }
    }
}