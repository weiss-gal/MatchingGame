using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.GameEngine
{
    public abstract class Constraint
    {
        abstract public bool checkMatch(Participant participant);
    }

    public class GenderBlockConstraint : Constraint
    {
        private readonly Gender gender;

        public GenderBlockConstraint(Gender gender)
        {
            this.gender = gender;
        }

        override public bool checkMatch(Participant participant)
        {
            return !(gender == participant.gender);
        }
    }

    public class ParticipantBlockConstraint : Constraint
    {
        private readonly string participantName;

        public ParticipantBlockConstraint(string name)
        {
            this.participantName = name;
        }

        public override bool checkMatch(Participant participant)
        {
            return !(this.participantName == participant.name);
        }
    }
}
