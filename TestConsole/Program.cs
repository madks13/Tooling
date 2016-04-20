using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.Adapters;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            long n = 9081726354;
            NumberToBase b2b = new NumberToBase("findme");
            String s = b2b.Base10ToString(n);
            Console.WriteLine(s);
            Console.WriteLine(b2b.StringToBase10(s));
            Console.ReadKey();
        }
    }
}
