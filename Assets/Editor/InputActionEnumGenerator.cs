using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
namespace Varez
{
    public class InputActionEnumGenerator : EditorWindow
    {
        private string enumName = "InputActionEnum";
        private string outputPath = "Assets/_Source/Scripts";
        private string namespaceName = "Varez";

        [MenuItem("Tools/Generate Input Action Enum")]
        public static void ShowWindow()
        {
            GetWindow<InputActionEnumGenerator>("Input Action Enum Generator");
        }

        private void OnGUI()
        {
            enumName = EditorGUILayout.TextField("Enum Name:", enumName);
            namespaceName = EditorGUILayout.TextField("Namespace:", namespaceName);
            outputPath = EditorGUILayout.TextField("Output Path:", outputPath);

            if (GUILayout.Button("Generate Enum"))
            {
                GenerateEnum();
            }
        }

        private void GenerateEnum()
        {
            // Create output directory if it doesn't exist
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            StringBuilder enumBuilder = new StringBuilder();

            // Add namespace
            if (!string.IsNullOrEmpty(namespaceName))
            {
                enumBuilder.AppendLine($"namespace {namespaceName}");
                enumBuilder.AppendLine("{");
            }

            // Begin enum declaration
            enumBuilder.AppendLine($"    public enum {enumName}");
            enumBuilder.AppendLine("    {");

            // Get all input actions and sort them
            var actionNames = InputSystem.actions
                .Select(action => action.name)
                // .OrderBy(name => name)
                .Distinct()
                .ToList();

            // Add enum entries
            for (int i = 0; i < actionNames.Count; i++)
            {
                string enumValue = SanitizeEnumName(actionNames[i]);
                enumBuilder.AppendLine($"        {enumValue} = {i},");
            }

            // Close enum
            enumBuilder.AppendLine("    }");

            // Close namespace if used
            if (!string.IsNullOrEmpty(namespaceName))
            {
                enumBuilder.AppendLine("}");
            }

            // Write to file
            string filePath = Path.Combine(outputPath, $"{enumName}.cs");
            File.WriteAllText(filePath, enumBuilder.ToString());
            AssetDatabase.Refresh();

        }

        private string SanitizeEnumName(string actionName)
        {
            // Replace any invalid characters with underscore
            string sanitized = System.Text.RegularExpressions.Regex.Replace(actionName, @"[^\w_]", "_");

            // Ensure it starts with a letter
            if (char.IsDigit(sanitized[0]))
            {
                sanitized = "_" + sanitized;
            }

            return sanitized;
        }
    }
}
#endif