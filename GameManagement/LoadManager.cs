using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchingGame.Parsing;

namespace MatchingGame.GameManagement
{
    public class LoadManager
    {
        private string fileName;
        public LoadManager(String fileName)
        {
            this.fileName = fileName;
        }

        public void start()
        {
            var parser = new InputFileParser(fileName);
            var results = parser.parse();
        }
    }
}
