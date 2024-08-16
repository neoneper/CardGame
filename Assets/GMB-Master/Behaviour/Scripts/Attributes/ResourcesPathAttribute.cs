using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMB
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ResourcesPathAttribute : Attribute
    {
        private string _relativePath = "";


        /// <summary>
        /// Caminho absoluto de <seealso cref="StringsProvider._PATH_DATAS_"/> + <see cref="_relativePath"/>
        /// </summary>
        public string path { get { return StringsProvider._PATH_DATAS_ + "/" + _relativePath + "/"; } }
        public ResourcesPathAttribute(string relativePath)
        {
            _relativePath = relativePath;
        }



    }
}
