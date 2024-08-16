using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMB
{
    public interface IData_Tags
    {
        public List<Data_Tag> GetTags();
        public List<string> GetTagsName();
        public bool ContainsTagName(string tag);
        public int GetTagIndex(Data_Tag tag);
    }
}
