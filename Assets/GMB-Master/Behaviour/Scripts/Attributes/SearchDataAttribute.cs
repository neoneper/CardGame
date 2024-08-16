using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMB
{
   
    
    public class SearchDataAttribute : PropertyAttribute
    {
        public Type searchObjectType;
        public SearchDataAttribute(Type searchObjectType)
        {
            this.searchObjectType = searchObjectType;
        }
        public SearchDataAttribute()
        {
            this.searchObjectType = null;
        }
    }

}