using System;
using System.Collections.Generic;
using System.Linq;

namespace GMB
{
	public static class GMBExtensions
	{
        public static T ListToEnumFlags<T>(List<string> enumFlagsAsList) where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new NotSupportedException(typeof(T).Name + " is not an Enum");
            T flags;
            enumFlagsAsList.RemoveAll(c => !Enum.TryParse(c, true, out flags));
            var commaSeparatedFlags = string.Join(",", enumFlagsAsList);
            Enum.TryParse(commaSeparatedFlags, true, out flags);
            return flags;
        }
        
    }

}