using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N64Controller
{
    public static class ArrayUtility
    {
        public static bool IsEquivalentTo(this byte[] array, byte[] otherArray)
        {
            if (array == null && otherArray == null)
                return true;
            if (array == null || otherArray == null)
                return false;
            return array.SequenceEqual(otherArray);
        }
    }
}
