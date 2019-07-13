using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.GameEngine
{
    abstract class Constraint
    {
        abstract public bool checkMatch(Participant participant);
    }

    class GenderBlockConstraint : Constraint
    {
        private readonly Gender gender;

        GenderBlockConstraint(Gender gender)
        {
            this.gender = gender;
        }

        override public bool checkMatch(Participant participant)
        {
            return !(gender == participant.gender);
        }
    }

    class ParticipantBlockConstraint : Constraint
    {
        private readonly Participant participant;

        ParticipantBlockConstraint(Participant participant)
        {
            this.participant = participant;
        }

        public override bool checkMatch(Participant participant)
        {
            return !(this.participant == participant);
        }
    }
}
