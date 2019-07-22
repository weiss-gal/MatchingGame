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

    class GameManager
    {
        private LoadManager loadManager = null;
        private Logger logger;
        private List<Participant> participants = new List<Participant>();
        private List<KeyValuePair<string, Constraint>> constraints = new List<KeyValuePair<string, Constraint>>();
        private SetRunEnabledCB setRunEnabledCB;

        public GameManager(Logger logger)
        {
            this.logger = logger;
        }

        public GameManager(SetRunEnabledCB setRunEnabledCB)
        {
            this.logger = new ConsoleLogger();
            this.setRunEnabledCB = setRunEnabledCB;
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

        public void Run()
        {
            MessageBox.Show("Method 'run' is not implemented yet");
        }
    }
}
