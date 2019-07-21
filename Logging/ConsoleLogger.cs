using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.Logging
{
    class ConsoleLogger : Logger
    {
        override public void Log(string line)
        {
            Console.WriteLine(line);
        }
    }
}
