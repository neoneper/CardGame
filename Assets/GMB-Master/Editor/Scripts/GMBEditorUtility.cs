using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GMBEditor
{
    public static class GMBEditorUtility
    {


        /// <summary>C#'s Script Icon [The one MonoBhevaiour Scripts have].</summary>
        private static Texture2D scriptIcon = (EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D);

        /// <summary>Creates a new C# Class.</summary>
        [MenuItem("Assets/Create/GMB/Data C# Script", false, 89)]
        private static void CreateData()
        {
            string[] guids = AssetDatabase.FindAssets("DataTemplate.cs");
            if (guids.Length == 0)
            {
                Debug.LogWarning("DataTemplate.cs.txt not found in asset database");
                return;
            }

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            CreateFromTemplate(
                "NewData.cs",
                path
            );
        }
        [MenuItem("Assets/Create/GMB/DataEditorWindow C# Script", false, 90)]
        private static void CreateEditorWindowData()
        {
            string[] guids = AssetDatabase.FindAssets("DataWindowTemplate.cs");
            if (guids.Length == 0)
            {
                Debug.LogWarning("DataWindowTemplate.cs.txt not found in asset database");
                return;
            }

            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            CreateFromTemplate(
                "NewEditorWindowData.cs",
                path
            );
        }

        public static void CreateFromTemplate(string initialName, string templatePath)
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,
                ScriptableObject.CreateInstance<DoCreateCodeFile>(),
                initialName,
                scriptIcon,
                templatePath
            );
        }

        /// Inherits from EndNameAction, must override EndNameAction.Action
        public class DoCreateCodeFile : UnityEditor.ProjectWindowCallback.EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                UnityEngine.Object o = CreateScript(pathName, resourceFile);
                ProjectWindowUtil.ShowCreatedAsset(o);
            }
        }

        /// <summary>Creates Script from Template's path.</summary>
        internal static UnityEngine.Object CreateScript(string pathName, string templatePath)
        {
            string className = Path.GetFileNameWithoutExtension(pathName).Replace(" ", string.Empty);
            string templateText = string.Empty;

            UTF8Encoding encoding = new UTF8Encoding(true, false);

            if (File.Exists(templatePath))
            {
                /// Read procedures.
                StreamReader reader = new StreamReader(templatePath);
                templateText = reader.ReadToEnd();
                reader.Close();

                templateText = templateText.Replace("#SCRIPTNAME#", className);
                templateText = templateText.Replace("#NOTRIM#", string.Empty);
                /// You can replace as many tags you make on your templates, just repeat Replace function
                /// e.g.:
                /// templateText = templateText.Replace("#NEWTAG#", "MyText");

                /// Write procedures.

                StreamWriter writer = new StreamWriter(Path.GetFullPath(pathName), false, encoding);
                writer.Write(templateText);
                writer.Close();

                AssetDatabase.ImportAsset(pathName);
                return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
            }
            else
            {
                Debug.LogError(string.Format("The template file was not found: {0}", templatePath));
                return null;
            }
        }


        public static List<T> FindAllFilesInPath<T>(string path, bool autoCreateDirectory = true) where T : GMB.Data
        {
            if (autoCreateDirectory)
                Directory.CreateDirectory(path);

            string[] allPaths = Directory.GetFiles(path, "*.asset", SearchOption.AllDirectories);
            List<T> result = new List<T>();

            foreach (string searchPath in allPaths)
            {
                string cleanedPath = searchPath.Replace("\\", "/");
                var file = AssetDatabase.LoadAssetAtPath(cleanedPath, typeof(T));
                result.Add((T)(GMB.Data)file);
            }

            return result;
        }
        public static List<GMB.Data> FindAllFilesInPath(string path, Type type, bool autoCreateDirectory = true) 
        {
            if (autoCreateDirectory)
                Directory.CreateDirectory(path);

            string[] allPaths = Directory.GetFiles(path, "*.asset", SearchOption.AllDirectories);
            List<GMB.Data> result = new List<GMB.Data>();

            foreach (string searchPath in allPaths)
            {
                string cleanedPath = searchPath.Replace("\\", "/");
                var file = AssetDatabase.LoadAssetAtPath(cleanedPath, type);
                result.Add((GMB.Data)file);
            }

            return result;
        }
        public static bool DeleteAsset(GMB.Data data)
        {
            string filePath = AssetDatabase.GetAssetPath(data);
            bool removed = AssetDatabase.DeleteAsset(filePath);

            if (removed)
            {
                //EditorUtility.SetDirty(db);
                AssetDatabase.SaveAssets();
            }

            return removed;


        }
        public static void CreateAsset(ScriptableObject instance, string filePath)
        {

            

            AssetDatabase.CreateAsset(instance, filePath);


            //EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();

        }


        public static PopupField<T> CreatePopupField<T>(VisualElement defaultElement) where T : GMB.Data
        {
            PopupField<T> popup = new PopupField<T>(defaultElement.name);
            popup.label = "";
            //popup.index = defaultElement.parent.IndexOf(defaultElement);
            defaultElement.parent.Insert(defaultElement.parent.IndexOf(defaultElement), popup);

            defaultElement.style.display = DisplayStyle.None;
            popup.name = defaultElement.name;
            //popup.AddToClassList("headder_PopupField"); 
            return popup;
        }



    }

 
    


}