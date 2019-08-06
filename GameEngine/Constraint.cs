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
        public abstract override string ToString();
    }

    public class GenderBlockConstraint : Constraint
    {
        private readonly Gender genderToBlock;

        public GenderBlockConstraint(Gender gender)
        {
            this.genderToBlock = gender;
        }

        override public bool checkMatch(Participant participant)
        {
            return !(genderToBlock == participant.gender);
        }

        public override string ToString()
        {
            return $"Block all {this.genderToBlock}s";
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

        public override string ToString()
        {
            return $"Block {this.participantName}";
        }
    }
}
