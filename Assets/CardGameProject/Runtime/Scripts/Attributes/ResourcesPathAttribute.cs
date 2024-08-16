using System;

namespace CardGameProject.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ResourcesPathAttribute : Attribute
    {
        private string _relativePath = "";

        #if UNITY_EDITOR
        /// <summary>
        /// Caminho absoluto de <seealso cref="StringsProvider._PATH_DATAS_"/> + <see cref="_relativePath"/>
        /// </summary>
        public string path { get { return  "/" + _relativePath + "/"; } }
#endif

        public ResourcesPathAttribute(string relativePath)
        {
            _relativePath = relativePath;
        }



    }
}
