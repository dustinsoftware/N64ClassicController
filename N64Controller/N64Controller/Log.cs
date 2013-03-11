using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N64Controller
{
    public static class Log
    {
        public static void Write(IEnumerable<byte> objects)
        {
            Write(String.Join(",", objects.Select(x => x.ToString())));
        }

        public static void Write(string str)
        {
            Console.WriteLine();
            Console.Write(str);
        }
    }
}
