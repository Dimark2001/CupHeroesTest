using UnityEditor;

public class DomainReloadEditor : EditorWindow
{
    [MenuItem("Tools/Reload Domain")]
    private static void DomainReload()
    {
        EditorUtility.RequestScriptReload();
    }
}