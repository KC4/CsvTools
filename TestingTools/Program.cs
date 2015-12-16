using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvTools;

namespace TestingTools
{
    class Program
    {
        public static void Main(string[] args)
        {
            var csv = GetTestString();
            var csvObject = new CsvObject(csv);
            csvObject.test();
            Console.ReadLine();
        }

        public static string GetTestString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("A, \"B\", \"C\"");
            stringBuilder.AppendLine("\"1\", \"2,3\", 4");
            stringBuilder.AppendLine("5, 6, 7");
            stringBuilder.AppendLine("TEST1, TEST2, TEST3");
            stringBuilder.AppendLine("\"Test4, Test5\", \"Test6, Test7\", \"Test8, Test9\"");
            stringBuilder.AppendLine("\"\", \"\", \"\"");
            stringBuilder.AppendLine(",,");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("1,2,3");
            return stringBuilder.ToString();
        }
    }
}
