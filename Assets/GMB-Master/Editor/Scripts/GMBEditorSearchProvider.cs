using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using GMB;

namespace GMBEditor
{

    public class GMBEditorSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        public class SearchResult
        {
            public string[] relativepath;
            public string resultFriendlyName;
            public string resultFileName;
            public string windowTitle = "Datas";

            public string GetFullPath()
            {
                string result = "";
                foreach (string s in relativepath)
                {
                    result += "/" + s;
                }

                result = result.Replace("//", "/");
                return result;
            }
            public T GetDataFile<T>() where T : GMB.Data
            {
                string path = DataEditorUtility.GetAbsoluteResourcesDatasPath<T>().Combine(resultFileName);
                // Debug.Log(path);
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<T>(path);
                return obj as T;
            }
            public UnityEngine.Object GetDataFile(Type type)
            {
                string path = DataEditorUtility.GetAbsoluteResourcesDatasPath(type).Combine(resultFileName);
                // Debug.Log(path);
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, type);
                return obj;
            }

            public SearchResult(string[] entry)
            {
                resultFriendlyName = entry.Last().Split(":").First();
                resultFileName = entry.Last().Split(":").Last().Combine(".asset");

                relativepath = entry;

                relativepath[relativepath.Length - 1] = relativepath[relativepath.Length - 1].RemoveBefore(":");
            }
        }

        public string[] listItems;
        public string winTitle;

        public Action<SearchResult> onSetIndexCallback;

        public static GMBEditorSearchProvider CreateProvider(string[] itemsRelativePath, string windowTitle, Action<SearchResult> callback)
        {
            GMBEditorSearchProvider data = ScriptableObject.CreateInstance<GMBEditorSearchProvider>();
            data.listItems = itemsRelativePath;
            ArrayUtility.Insert(ref data.listItems, 0, "Options/" + EditorStringsProvider._LISTVIEW_NONE_OPTIONS_);
            ArrayUtility.Insert(ref data.listItems, 0, "Options/" + EditorStringsProvider._LISTVIEW_NEW_OPTIONS_);
            data.onSetIndexCallback = callback;
            data.winTitle = windowTitle;
            return data;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> list = new List<SearchTreeEntry>();
            list.Add(new SearchTreeGroupEntry(new GUIContent(winTitle)));



            List<string> sortedListItems = listItems.ToList();
            sortedListItems.Sort((a, b) =>
            {
                string[] splits1 = a.Split('/');
                string[] split2 = b.Split('/');
                for (int i = 0; i < splits1.Length; i++)
                {
                    if (i >= split2.Length)
                    {
                        return 1;
                    }
                    int value = splits1[i].CompareTo(split2[i]);
                    if (value != 0)
                    {
                        if (splits1.Length != split2.Length && (i == splits1.Length - 1 || i == split2.Length - 1))
                            return splits1.Length < split2.Length ? 1 : -1;
                        return value;
                    }
                }
                return 0;
            });


            List<string> groups = new List<string>();
            foreach (string item in sortedListItems)
            {
                string[] entryTitle = item.Split('/');
                string groupName = "";
                for (int i = 0; i < entryTitle.Length - 1; i++)
                {
                    groupName += entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        list.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }



                SearchResult result = new SearchResult(entryTitle);

                Data data = result.GetDataFile<Data>();
                Texture image = GMBEditorStyles.TextureUnKnow;
                if (data != null)
                {
                    if (data.GetIcon() != null)
                    {
                        image = data.GetIcon().texture;
                    }
                    else
                    {
                        image = GMBEditorStyles.TextureUnKnow;
                    }

                }

                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(result.resultFriendlyName, image));
                entry.level = entryTitle.Length;

                entry.userData = result;

                //entry.userData = entryTitle.Last();
                list.Add(entry);
            }


            return list;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            onSetIndexCallback?.Invoke((SearchResult)SearchTreeEntry.userData);
            return true;
        }
    }
}