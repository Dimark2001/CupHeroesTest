// say you have Resources/test1.prefab
//                             /folder
//                                sound.wav
// this will generate
// class GameResources
//        GameObject test1
//        class Folder
//            AudioClip sound
// and you'll be able to do
// GameResources.Folder.sound in your code!
// no more string links

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ResourceClassGenerator : AssetPostprocessor
{
    public const string PATH_TO_RESOURCES_FOLDER = "Game/Resources";
    public const string PATH_TO_OUTPUT = "Generated";
    public const string OUTPUT_CLASS_NAME = "GameResources";

    private static bool _pendingGenerate = false;

    [MenuItem("Tools/Generate Resource Class")]
    public static void GenerateResourceClass()
    {
        string resourcesPath = Path.Combine(Application.dataPath, PATH_TO_RESOURCES_FOLDER);
        if (!Directory.Exists(resourcesPath))
        {
            Debug.LogError("Resources folder not found at path: " + resourcesPath);
            return;
        }

        string outputDirectory = Path.Combine(Application.dataPath, PATH_TO_OUTPUT);
        string outputPath = Path.Combine(outputDirectory, $"{OUTPUT_CLASS_NAME}.cs");

        // Ensure the output directory exists
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        HashSet<string> namespaces = new HashSet<string> { "UnityEngine" };

        StringBuilder classBuilder = new StringBuilder();
        classBuilder.AppendLine("// This file is auto-generated. Do not modify manually.");
        classBuilder.AppendLine();

        // Generate class body
        classBuilder.AppendLine($"public static class {OUTPUT_CLASS_NAME}");
        classBuilder.AppendLine("{");

        GenerateClassForFolder(resourcesPath, classBuilder, "    ", namespaces);
        classBuilder.AppendLine("}");

        // Add collected namespaces at the top of the file
        StringBuilder finalBuilder = new StringBuilder();
        foreach (string ns in namespaces.OrderBy(n => n))
        {
            finalBuilder.AppendLine($"using {ns};");
        }

        finalBuilder.AppendLine();
        finalBuilder.Append(classBuilder.ToString());

        // Write the file
        var isNewFile = !File.Exists(outputPath);
        File.WriteAllText(outputPath, finalBuilder.ToString());

        // Refresh if it's new
        if (isNewFile)
        {
            AssetDatabase.Refresh();
            Debug.Log($"Generated {OUTPUT_CLASS_NAME} class at: {outputPath}");
        }
    }

    private static void GenerateClassForFolder(string folderPath, StringBuilder classBuilder, string indent,
        HashSet<string> namespaces)
    {
        string[] subFolders = Directory.GetDirectories(folderPath);
        string[] files = Directory.GetFiles(folderPath);

        foreach (string subFolder in subFolders)
        {
            string folderName = EscapeToValidIdentifier(Path.GetFileName(subFolder));
            classBuilder.AppendLine($"{indent}public static class {folderName}");
            classBuilder.AppendLine($"{indent}{{");
            GenerateClassForFolder(subFolder, classBuilder, indent + "    ", namespaces);
            classBuilder.AppendLine($"{indent}}}");
        }

        foreach (string file in files)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
            if (string.IsNullOrEmpty(fileNameWithoutExtension)) continue;

            string fileName = EscapeToValidIdentifier(fileNameWithoutExtension);
            string relativePath = GetRelativePath(file);

            string assetType = GetAssetType(file, relativePath, namespaces);
            if (!string.IsNullOrEmpty(assetType))
            {
                classBuilder.AppendLine(
                    $"{indent}public static {assetType} {fileName} => Resources.Load<{assetType}>(\"{relativePath}\");");
            }
        }
    }

    private static string GetAssetType(string filePath, string relativePath, HashSet<string> namespaces)
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();

        switch (fileExtension)
        {
            case ".prefab":
                string componentType = GetFirstMonoBehaviourType(relativePath, namespaces);
                return string.IsNullOrEmpty(componentType) ? "GameObject" : componentType;
            case ".asset":
                string fullPath = $"Assets/{PATH_TO_RESOURCES_FOLDER}/" + relativePath + ".asset";
                ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(fullPath);
                return so == null ? null : so.GetType().ToString();
            case ".txt":
            case ".json":
            case ".xml":
                return "TextAsset";
            case ".mp3":
            case ".wav":
            case ".ogg":
                return "AudioClip";
            case ".png":
            case ".jpg":
            case ".psd":
                return "Sprite";
            case ".jpeg":
                return "Sprite";
            case ".mat":
                return "Material";
            case ".shader":
                return "Shader";
            case ".uss":
                return "StyleSheet";
            default:
                return null;
        }
    }

    private static string GetFirstScriptableObject(string relativePath, HashSet<string> namespaces)
    {
        string fullPath = $"Assets/{PATH_TO_RESOURCES_FOLDER}/" + relativePath + ".asset";
        ScriptableObject prefab = AssetDatabase.LoadAssetAtPath<ScriptableObject>(fullPath);
        return prefab.GetType().ToString();
    }

    private static string GetFirstMonoBehaviourType(string relativePath, HashSet<string> namespaces)
    {
        string fullPath = $"Assets/{PATH_TO_RESOURCES_FOLDER}/" + relativePath + ".prefab";
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(fullPath);
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found: <color=red>{fullPath}</color>");
            return null;
        }

        Component[] components = prefab.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component != null && component is MonoBehaviour)
            {
                Type type = component.GetType();
                if (!string.IsNullOrEmpty(type.Namespace))
                {
                    namespaces.Add(type.Namespace);
                }

                return type.Name;
            }
        }

        return null;
    }

    private static string EscapeToValidIdentifier(string name)
    {
        StringBuilder validName = new StringBuilder();
        foreach (char c in name)
        {
            if (char.IsLetterOrDigit(c) || c == '_')
            {
                validName.Append(c);
            }
            else
            {
                validName.Append('_');
            }
        }

        if (char.IsDigit(validName[0]))
        {
            validName.Insert(0, '_');
        }

        string finalName = validName.ToString();
        if (IsCSharpKeyword(finalName))
        {
            finalName = $"@{finalName}";
        }

        return finalName;
    }

    private static bool IsCSharpKeyword(string word)
    {
        string[] keywords =
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const",
            "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit",
            "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in",
            "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator",
            "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte",
            "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw",
            "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void",
            "volatile", "while"
        };

        return ArrayUtility.Contains(keywords, word);
    }

    private static string GetRelativePath(string fullPath)
    {
        int index = fullPath.IndexOf("Resources", StringComparison.Ordinal);
        if (index == -1)
        {
            Debug.LogError($"Could not find 'Resources' in path: {fullPath}");
            return null;
        }

        string relativePath = fullPath.Substring(index + "Resources/".Length);
        return relativePath.Replace("\\", "/").Replace(Path.GetExtension(fullPath), "");
    }

    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        // Schedule class generation after recompilation
        if (!_pendingGenerate)
        {
            _pendingGenerate = true;
            EditorApplication.delayCall += () =>
            {
                if (EditorApplication.isCompiling)
                {
                    EditorApplication.delayCall += OnRecompileCompleted;
                }
                else
                {
                    OnRecompileCompleted();
                }
            };
        }
    }

    private static void OnRecompileCompleted()
    {
        if (!EditorApplication.isCompiling)
        {
            _pendingGenerate = false;
            GenerateResourceClass();
        }
    }
}