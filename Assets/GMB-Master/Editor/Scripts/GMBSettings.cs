using System.Collections;
using System.Collections.Generic;
using GMB;
using UnityEditor;
using UnityEngine;

namespace GMBEditor
{
    [FilePath("GMB/GMBSettigns.asset", FilePathAttribute.Location.PreferencesFolder)]
    public class GMBSettings : ScriptableSingleton<GMBSettings>
    {
        [SerializeField]
        int _aid = -1;


        public int CurrentID { get { return _aid; } }

        public int GetAndAutoIncrementID()
        {
            _aid++;

            Save(true);
            //Debug.Log("Saved to: " + GetFilePath());

            return _aid;
        }

        [MenuItem("Tools/GMBEditorSettings/ResetAIDs")]
        public static void ResetDataIDs()
        {
            List<Data> datas = GMBEditorUtility.FindAllFilesInPath<Data>(StringsProvider._PATH_DATAS_);

            instance._aid = -1;


            foreach (Data d in datas)
            {

                d.SetAutoIncrementID();
            }

            instance.Save(true);
            Debug.Log("GMBSettigns Saved to: " + GetFilePath());
        }
    }
}
