using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace GMBEditor
{
    public class EditorStringsProvider
    {
        public const string _LISTVIEW_NONE_OPTIONS_ = "0 - NONE";
        public const string _LISTVIEW_NEW_OPTIONS_ = "1 - CREATE NEW";

        private static string _ROOTFOLDER_
        {
            get
            {
               
                return _PATH_GMB_;
            }
        }

        public const string _PATH_GMB_ = "Assets/GMB-Master/";



        public static string _PATH_GMB_RESOURCES_ = _ROOTFOLDER_ + "Behaviour/Resources/";
        public static string _PATH_GMB_EDITOR_ = _ROOTFOLDER_ + "Editor/";
        public static string _PATH_GMB_EDITOR_RESOURCES_ = _PATH_GMB_EDITOR_ + "Resources/";
        public static string _PATH_GMB_EDITOR_TEMPLATES_ = _PATH_GMB_EDITOR_RESOURCES_ + "VisualTemplates/";
        public static string _PATH_GMB_EDITOR_TEMPLATES_DEFAULTS = _PATH_GMB_EDITOR_TEMPLATES_ + "Defaults/";


    }

}