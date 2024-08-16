using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GMB;
using System;
using System.Reflection;
using UnityEditor.Experimental.GraphView;

namespace GMBEditor
{

    [CustomPropertyDrawer(typeof(SearchDataAttribute))]
    public class SearchDataAttributeDrawer : PropertyDrawer
    {


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            position.width -= 60;
            if (property.objectReferenceValue != null)
            {
                if (property.objectReferenceValue is Data)
                {
                    Data objData = (Data)property.objectReferenceValue;

                    Sprite Icon = objData.GetIcon();
                    if (Icon == null)
                    {
                        Icon = GMBEditorStyles.SpriteUnKnow;
                    }

                    GUIContent superLabel = new GUIContent(objData.GetFriendlyName(), Icon.texture);

                    label = superLabel;
                }
            }


            EditorGUI.ObjectField(position, property, label);
            //EditorGUI.LabelField(position, label);



            position.x += position.width;
            position.width = 60;
            if (GUI.Button(position, new GUIContent("Find")))
            {
               

                Type dataType = property?.GetPropertyAttribute<SearchDataAttribute>(true).searchObjectType;



                if (dataType == null)
                {

                    if (fieldInfo.FieldType.IsGenericType)
                    {
                        dataType = fieldInfo.FieldType.GetGenericArguments()[0];
                    }
                    else if (fieldInfo.FieldType.IsArray)
                    {
                        dataType = fieldInfo.FieldType.GetElementType();
                    }
                    else
                    {
                        dataType = fieldInfo.FieldType;
                    }
                }

                DataEditorUtility.ShowSearchWindow("Data", GUIUtility.GUIToScreenPoint(Event.current.mousePosition), dataType, (result) =>
                {
                    property.objectReferenceValue = result.GetDataFile(dataType);
                    property.serializedObject.ApplyModifiedProperties();
                });


            }

        }


    }
}