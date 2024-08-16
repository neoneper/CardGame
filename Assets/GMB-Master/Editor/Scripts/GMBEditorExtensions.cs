using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.UIElements;
using System.Linq;
namespace GMBEditor
{
    public static class GMBEditorExtensions
    {



        //Attributes
        public static T GetPropertyAttribute<T>(this SerializedProperty prop, bool inherit) where T : PropertyAttribute
        {
            if (prop == null) { Debug.Log(0); return null; }

            Type t = prop.serializedObject.targetObject.GetType();
            FieldInfo f = null;
            PropertyInfo p = null;

            //return null;
            var path = prop.propertyPath;
            var elements = path.Split('.');


            string[] slices = prop.propertyPath.Split('.');
            System.Type type = prop.serializedObject.targetObject.GetType();

            //Debug.Log(prop.propertyPath);

            //Field
            for (int i = 0; i < slices.Length; i++)
            {
                if (slices[i] == "Array")
                {
                    i++; //skips "data[x]"

                    Type nextType = type.GetElementType();
                    if (nextType == null)
                    {
                        nextType = type.GetGenericArguments()[0];
                    }
                    type = nextType;
                }
                else if (type != null)
                {

                    f = type.GetField(slices[i], (BindingFlags)(-1));
                    p = type.GetProperty(slices[i], (BindingFlags)(-1));
                    type = f.FieldType;

                }
                else
                {
                    Debug.Log(slices[i]);
                    Debug.Log(type);
                }
            }


            T[] attributes;
            if (f != null)
            {
                attributes = f.GetCustomAttributes(typeof(T), inherit) as T[];

            }
            else if (p != null)
            {
                attributes = p.GetCustomAttributes(typeof(T), inherit) as T[];
            }
            else
            {
                return null;
            }

            return attributes.Length > 0 ? attributes[0] : null;
        }

        //VisualElements
        public static void SetVisible(this VisualElement element, bool visible)
        {

            element.style.visibility = visible == true ? Visibility.Visible : Visibility.Hidden;
        }
        public static void SetDisplay(this VisualElement element, bool display)
        {

            element.style.display = display == true ? DisplayStyle.Flex : DisplayStyle.None;
        }


        //Strings
        public static string Combine(this string a, string b)
        {
            return a + b;
        }

        public static string RemoveAfter(this string input, string character)
        {

            int index = input.IndexOf(character);
            if (index >= 0)
                input = input.Substring(0, index);

            return input;
        }
        public static string RemoveBefore(this string input, string character)
        {

            int index = input.IndexOf(character);
            if (index >= 0)
                input = input.Substring(index + 1);

            return input;
        }



        public static object GetValue(this SerializedProperty property)
        {
            System.Type parentType = property.serializedObject.targetObject.GetType();
            System.Reflection.FieldInfo fi = parentType.GetFieldViaPath(property.propertyPath);
            return fi.GetValue(property.serializedObject.targetObject);
        }
        public static void SetValue(this SerializedProperty property, object value)
        {
            System.Type parentType = property.serializedObject.targetObject.GetType();
            System.Reflection.FieldInfo fi = parentType.GetFieldViaPath(property.propertyPath);//this FieldInfo contains the type.
            fi.SetValue(property.serializedObject.targetObject, value);
            property.serializedObject.ApplyModifiedProperties();
        }
        public static System.Type GetFieldType(this SerializedProperty property)
        {
            System.Type parentType = property.serializedObject.targetObject.GetType();
            System.Reflection.FieldInfo fi = parentType.GetFieldViaPath(property.propertyPath);


            /*
            if (fi.FieldType.IsArray)
            {
                return fi.FieldType.GetElementType();
            }
            else if (fi.FieldType.IsGenericType)
            {
                return fi.FieldType.GetGenericArguments()[0];
            }
            else
            {
                return fi.FieldType;
            }*/
            return fi.FieldType;
        }
        public static System.Reflection.FieldInfo GetFieldViaPath(this System.Type type, string path)
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var parent = type;
            var fi = parent.GetField(path, flags);
            var paths = path.Split('.');

            for (int i = 0; i < paths.Length; i++)
            {
                fi = parent.GetField(paths[i], flags);
                if (fi != null)
                {
                    // there are only two container field type that can be serialized:
                    // Array and List<T>
                    if (fi.FieldType.IsArray)
                    {
                        parent = fi.FieldType.GetElementType();
                        i += 2;
                        continue;
                    }

                    if (fi.FieldType.IsGenericType)
                    {
                        parent = fi.FieldType.GetGenericArguments()[0];
                        i += 2;
                        continue;
                    }
                    parent = fi.FieldType;
                }
                else
                {
                    break;
                }

            }
            if (fi == null)
            {
                if (type.BaseType != null)
                {
                    return GetFieldViaPath(type.BaseType, path);
                }
                else
                {
                    return null;
                }
            }
            return fi;
        }
    }


}
