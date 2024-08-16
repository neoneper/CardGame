using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMB;
using System;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace GMBEditor
{
    public class DataElements_Window : GMBEditorWindow<Data_Element>
    {
        protected override void OnCloseGUI()
        {

        }

        protected override void OnCreateGUI()
        {

        }
        protected override void OnSelectedItemChanged()
        {
            if (listview_selectedItem != null)
            {
                GetGMBWindow().AddHistoric(this, listview_selectedItem.GetFriendlyName());
            }
        }
        protected override string GetTemplate_FilePath()
        {
            return EditorStringsProvider._PATH_GMB_EDITOR_TEMPLATES_DEFAULTS.Combine("Data.uxml");
        }

        protected override string GetTemplate_ListViewItemFilePath()
        {
            return EditorStringsProvider._PATH_GMB_EDITOR_TEMPLATES_DEFAULTS.Combine("Data_Listview_Item.uxml");
        }

        public override GMBWindowMenuItem GetGMBWindowMenuItem()
        {
            return new GMBWindowMenuItem(this, "menu_tag_items", "Elements", "Element");
        }
    }
}