using MatchingGame.GameEngine;
using MatchingGame.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.GameManagement
{
    class GameManager
    {
        private LoadManager loadManager = null;
        private Logger logger;
        private List<Participant> participants = new List<Participant>();
        private List<KeyValuePair<string, Constraint>> constraints = new List<KeyValuePair<string, Constraint>>();

        public GameManager(Logger logger)
        {
            this.logger = logger;
        }

        public GameManager()
        {
            this.logger = new ConsoleLogger();
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
            logger.Log($"Added constraint of type '{constraint.GetType().Name}' to participant {name}");
        }

        public LoadManager getLoadManager()
        {
            if (loadManager != null)
                throw new Exception($"Attemping to load a new file while another file load is in progress");

            loadManager = new LoadManager(this.AddParticipant, this.AddConstraint, logger);
            
            return loadManager;
        }
    }
}
