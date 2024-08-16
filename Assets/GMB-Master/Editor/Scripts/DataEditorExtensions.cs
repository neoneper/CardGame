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

    public static class DataEditorExtensions
    {

        public static void SaveAsset(this Data data)
        {
            
            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssetIfDirty(data);
        }

        public static void SetAutoIncrementID(this Data dataObj)
        {
            SerializedObject sob = dataObj.GetSerializedObject();
            sob.FindProperty("_aid").intValue = GMBSettings.instance.GetAndAutoIncrementID();
            sob.ApplyModifiedPropertiesWithoutUndo();
        }
        public static void ResetAutoIncrementID(this Data dataObj)
        {
            SerializedObject sob = dataObj.GetSerializedObject();
            sob.FindProperty("_aid").intValue = -1;
            sob.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
