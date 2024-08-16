using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMB
{
    public interface IData_Vendor
    {
        public long GetBuyPrice();
        public long GetSellPrice();
    }
}
