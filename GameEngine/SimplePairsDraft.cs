using MatchingGame.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.GameEngine
{
    

    internal class ParticipantData
    {
        public Participant p { get; private set; }
        public int mCount {get; private set; }//Matches count
        List<Constraint> cs;
        

        public ParticipantData(Participant p)
        {
            this.p = p;
            cs = new List<Constraint>();
            mCount = 0;
        }

        public void AddConstraint(Constraint c)
        {
            this.cs.Add(c);
        }

        public bool CanMatch(Participant other)
        {
            var result =  cs.All(c => c.checkMatch(other));
            Console.WriteLine($"Checking if participant '{this.p.name}' can match '{other.name}'. result: {result}");
            return result;
        }

        public void IncMatchCount()
        {
            mCount++;
        }
    }

    internal class ParticipantsData
    {
        

        internal ParticipantsData(ICollection<Participant> participants, ICollection<KeyValuePair<string, Constraint>> constraints, Logger logger)
        {
            this.logger = logger;

            foreach (var p in participants)
            {
                this.AddParticipantData(new ParticipantData(p));
                logger.Log($"Added participant '{p.name}' to participants data");
                //Adding self Constraint
                this.AddConstraint(p, new ParticipantBlockConstraint(p.name));
                logger.Log($"Added self-constraint to participant '{p.name}'");
            }

            foreach (var pair in constraints)
            {
                this.AddConstraintByName(pair.Key, pair.Value);
                logger.Log($"Added constraint of type {pair.Value} to participant '{pair.Key}'");
            }
        }

        private Dictionary<int, ParticipantData> d = new Dictionary<int, ParticipantData>();
        private Logger logger;

        private int getKey(ParticipantData pd)
        {
            return pd.p.GetHashCode();
        }
        private int getKey(Participant p)
        {
            return p.GetHashCode();
        }

        private void AddParticipantData(ParticipantData pd)
        {
            if (this.d.ContainsKey(getKey(pd)))
                throw new ArgumentException($"Failed adding participant. '{pd.p.name}' already exists");

            this.d.Add(getKey(pd), pd);
        }

        public void AddConstraintByName(string name, Constraint constraint)
        {
            d.Where(pair => pair.Value.p.name == name).First().Value.AddConstraint(constraint);
        }

        private void AddConstraint(Participant participant, Constraint constraint)
        {
            d[getKey(participant)].AddConstraint(constraint);
        }

        public IEnumerable<ParticipantData> GetParticipantsData()
        {
            return this.d.Select(pair => pair.Value);
        }

        public bool IsMatchAllowed(Participant a, Participant b)
        { 
            var result =  d[getKey(a)].CanMatch(b) && d[getKey(b)].CanMatch(a);

            logger.Log($"Matching of '{a}' and '{b}' is {(result ? "allowed" : "not allowed")}");

            return result;
        }

        public ICollection<ParticipantData> GetCandidatesForParticipant(Participant p)
        {
            var result = this.d.Values.Where(pd => IsMatchAllowed(pd.p, p)).Select(pd => pd).ToList();

            logger.Log($"Found the following candidates for participant '{p}': {string.Join("; ", result)}");

            return result;
        }

        public void MakeMatch(Participant a, Participant b)
        {
            //Update match counts
            d[getKey(a)].IncMatchCount();
            d[getKey(b)].IncMatchCount();
            //Add constraint to prevent this match from repeating (need to block only in one way, it works both ways)
            AddConstraint(a, new ParticipantBlockConstraint(b.name));
        }

    }

    public class SimplePairsDraft : Draft
    {
        private ICollection<Participant> participants;
        private ICollection<KeyValuePair<string, Constraint>> constraints;
        private Logger logger;
  

        public SimplePairsDraft(ICollection<Participant> participants,
            ICollection<KeyValuePair<string, Constraint>> constraints,
            Logger logger)
        {
            this.participants = participants;
            this.constraints = constraints;
            this.logger = logger;
        }


        private HashSet<HashSet<Participant>> Draft()
        {
            //Initialiations
            var participantsData = new ParticipantsData(this.participants, this.constraints, this.logger);
            var result = new HashSet<HashSet<Participant>>();
            var rand = new Random();
            int round = 0;
            while (participantsData.GetParticipantsData().Where(pd => pd.mCount == 0).Count() > 0)
            {
                //1. Find the next participant to match
                logger.Log($"Finding next participant to match, round {round}");
                var pendingMatchList = participantsData.GetParticipantsData().Where(pd => pd.mCount == 0);
                logger.Log($"Participants with no match: {string.Join("; ", pendingMatchList.Select(pd => pd.p.name))}");
                //Using ToList() as a way to force Select to work only once.
                var pendingMatchListWithCandidates = pendingMatchList.Select(pd =>Tuple.Create(pd, participantsData.GetCandidatesForParticipant(pd.p))).ToList();
                //Take the participants with minimal number of candidates, there could be several with the same number
                logger.Log("before count");
                var minCandidatesNum = pendingMatchListWithCandidates.Min(t => t.Item2.Count());
                logger.Log("after count");
                if (minCandidatesNum == 0)
                    throw new DraftException("Draft failed: We have remaining participants that have no candidates");

                pendingMatchListWithCandidates = pendingMatchListWithCandidates.Where(t => t.Item2.Count() == minCandidatesNum).ToList();
                logger.Log($"Taking only participants with {minCandidatesNum} candidates: {string.Join("; ", pendingMatchListWithCandidates.Select(x => x.Item1.p))}");

                //Randomly choose a participant
                var nextParticipant = pendingMatchListWithCandidates.ToArray()[rand.Next(pendingMatchListWithCandidates.Count())];
                logger.Log($"Participant selected to be matched: {nextParticipant.Item1.p}");

                //2. Randomize a match
                var minMatches = nextParticipant.Item2.Min(pd => pd.mCount);
                if (minMatches > 2)
                    logger.Log($"Warning: participant {nextParticipant.Item1.p} have only candidates with {minMatches} or more matches");

                var candidates = nextParticipant.Item2.Where(pd => pd.mCount == minMatches);
                logger.Log($"Taking only candidates with {minMatches} matches, candidates: {string.Join(";", candidates.Select(pd => pd.p))}");

                var matchParticipant = candidates.ToArray()[rand.Next(candidates.Count())].p;
                logger.Log($"Match result: '{nextParticipant.Item1.p}' with '{matchParticipant}'");

                //3. Match participants
                var p1 = nextParticipant.Item1.p;
                var p2 = matchParticipant;

                result.Add(new HashSet<Participant>() {p1, p2});
                participantsData.MakeMatch(p1, p2);

                ++round;
            }

            return result;
        }

        public override HashSet<HashSet<Participant>> Run()
        {
            return Draft();
        }
    }
}
