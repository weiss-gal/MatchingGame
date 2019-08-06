using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.GameEngine
{
    public class DraftException : Exception
    {
        public DraftException(string message) : base(message)
        {

        }
    }

    //A draft returns a collection of matches, a match is a set of participants
    abstract public class Draft
    {
        public abstract HashSet<HashSet<Participant>> Run();
    }
}
