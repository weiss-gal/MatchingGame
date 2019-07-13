using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.GameEngine
{
    enum Gender
    {
        Male,
        Female
    }
    class Participant
    {
        Participant (String name, Gender gender)
        {
            this.name = name;
            this.gender = gender;
        }

        public String name {get; }
        public Gender gender {get; }

        /* Overrides */

        private static bool isEqual(Participant x, Participant y)
        {
            return (x.name == y.name && x.gender == y.gender);
        }

        public static bool operator ==(Participant x, Participant y)
        {
            return isEqual(x, y);
        }

        public static bool operator !=(Participant x, Participant y)
        {
            return !isEqual(x, y);
        }
    }
}
