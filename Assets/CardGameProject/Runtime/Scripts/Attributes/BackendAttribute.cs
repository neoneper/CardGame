using System;

namespace CardGameProject.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class BackendAttribute : Attribute
    {
        public string Template { get; }
        public string Style { get; }
        public string ListViewItemTemplate { get; }

        public bool HasStyle=>!String.IsNullOrEmpty(Style);
        public bool HasTemplate=>!String.IsNullOrEmpty(Template);
        public bool HasListViewItem=>!String.IsNullOrEmpty(ListViewItemTemplate);
        public BackendAttribute(string template, string style= "", string listViewItemTemplate = "")
        {
            Template = template;
            Style = style;
            ListViewItemTemplate = listViewItemTemplate;
        }
    }
}