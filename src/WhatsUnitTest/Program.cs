using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WhatsUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            NUnit.ConsoleRunner.Runner.Main(new string[]
           {
              Assembly.GetExecutingAssembly().Location, 
           });
        }
    }
}
