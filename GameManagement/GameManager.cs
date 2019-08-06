using MatchingGame.GameEngine;
using MatchingGame.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MatchingGame.GameManagement
{
    public delegate void SetRunEnabledCB(bool enabled);
    public delegate void SetResultsCB(string results);

    class GameManager
    {
        private LoadManager loadManager = null;
        private Logger logger;
        private List<Participant> participants = new List<Participant>();
        private List<KeyValuePair<string, Constraint>> constraints = new List<KeyValuePair<string, Constraint>>();
        private SetRunEnabledCB setRunEnabledCB;
        private SetResultsCB setResultsCB;

        public GameManager(SetRunEnabledCB setRunEnabledCB, SetResultsCB setResultsCB)
        {
            this.logger = new ConsoleLogger();
            this.setRunEnabledCB = setRunEnabledCB;
            this.setResultsCB = setResultsCB;
        }

        private void AddParticipant(Participant participant)
        {
            participants.Add(participant);
        }

        private void AddConstraint(string name, Constraint constraint)
        {
            if (!participants.Exists(p => p.name == name))
                throw new ArgumentException($"Attempting to add constraint to non-existins participant '{name}'");

            constraints.Add(new KeyValuePair<string, Constraint>(name, constraint));
            logger.Log($"Added constraint '{constraint}' to participant {name}");
        }

        private void LoadComplete()
        {
            this.setRunEnabledCB(true);
        }

        public LoadManager getLoadManager()
        {
            if (loadManager != null)
                throw new Exception($"Attemping to load a new file while another file load is in progress");

            loadManager = new LoadManager(this.AddParticipant, this.AddConstraint, this.LoadComplete, logger);
            
            return loadManager;
        }

        private string ResultsToString(HashSet<HashSet<Participant>> results)
        {
            var sb = new StringBuilder();
            uint matchCount = 1;
            foreach (var r in results)
            {
                sb.AppendLine($"Match {matchCount}: {string.Join(" with ", r)}");
                ++matchCount;
            }

            return sb.ToString();
        }

        public void Run()
        {
            var draft = new SimplePairsDraft(participants, constraints, logger);
            var results = draft.Run();
            setResultsCB(ResultsToString(results));
        }
    }
}
