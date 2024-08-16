using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMB;
using UnityEditor;
using System.Reflection;
using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace GMBEditor
{



    public class DataEditorUtility
    {




        /// <summary>
        /// <para>
        /// Retorna o caminho absoluto da pasta de dados apartir de <see cref="StringsProvider._PATH_DATAS_"/>, do qual e salvo todos os ScriptableObjects da implementacao do tipo de <typeparamref name="T"/>
        /// </para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>
        /// Implementacao do qual deseja-se obter o caminho relativo (<seealso cref="ResourcesPathAttribute.path"/>), dos dados salvos
        /// </para>
        /// </typeparam>
        /// <returns>
        /// <para>
        /// <seealso cref="ResourcesPathAttribute.path"/>, ou nada, caso o atributo nao tenha sido setado na classe da implementacao
        /// </para>
        /// </returns>
        public static string GetAbsoluteResourcesDatasPath<T>() where T : Data
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));


            Type result = types.FirstOrDefault(r => r == typeof(T));
            ResourcesPathAttribute attribute = result.GetCustomAttribute<GMB.ResourcesPathAttribute>(true);


            string path = attribute.path;
            path = path.Replace("//", "/");
            path = path.Replace("///", "/");
            return path;
        }
        public static string GetAbsoluteResourcesDatasPath(Type type)
        {

           // Debug.Log(type.FullName);


            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));


            Type result = types.FirstOrDefault(r => r == type);

            ResourcesPathAttribute attribute = result.GetCustomAttribute<GMB.ResourcesPathAttribute>(true);


            string path = attribute.path;
            path = path.Replace("//", "/");
            path = path.Replace("///", "/");
            return path;
        }

        public static bool ShowSearchWindow(string title, Vector2 position, Type t, Action<GMBEditor.GMBEditorSearchProvider.SearchResult> callback, List<Data> ignore = null)
        {

            List<Data> items = GMBEditorUtility.FindAllFilesInPath(GetAbsoluteResourcesDatasPath(t), t);
            if (ignore != null)
            {
                items = items.Except(ignore).ToList();
            }


            List<string> searhPath = items.Count > 0 ? items.Select(r => r.GetNameAsRelativePath().Combine(":").Combine(r.GetID())).ToList() : new List<string>();
            GMBEditorSearchProvider provider = GMBEditorSearchProvider.CreateProvider(searhPath.ToArray(), string.IsNullOrEmpty(title) ? t.Name : title, callback);
            SearchWindowContext searchWindow = new SearchWindowContext(position);
            return SearchWindow.Open(searchWindow, provider);

        }
        public static bool ShowSearchWindow<T>(string title, Vector2 position, Action<GMBEditor.GMBEditorSearchProvider.SearchResult> callback, List<T> ignore = null) where T : Data
        {

            List<T> items = GMBEditorUtility.FindAllFilesInPath<T>(GetAbsoluteResourcesDatasPath<T>());
            if (ignore != null)
            {
                items = items.Except(ignore).ToList();
            }


            List<string> searhPath = items.Count > 0 ? items.Select(r => r.GetNameAsRelativePath().Combine(":").Combine(r.GetID())).ToList() : new List<string>();
            GMBEditorSearchProvider provider = GMBEditorSearchProvider.CreateProvider(searhPath.ToArray(), string.IsNullOrEmpty(title) ? typeof(T).Name : title, callback);
            SearchWindowContext searchWindow = new SearchWindowContext(position);
            return SearchWindow.Open(searchWindow, provider);

        }


    }
}
