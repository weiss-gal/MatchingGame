using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchingGame.Parsing;
using MatchingGame.Logging;
using MatchingGame.GameEngine;
using MatchingGame.GameConstants;

namespace MatchingGame.GameManagement
{
    public delegate void AddParticipantCB(Participant p);
    public delegate void AddConstraintToParticipantCB(string name, Constraint c);

    public class LoadManager
    {
        private Logger logger;
        private AddParticipantCB addParticipantCB;
        private AddConstraintToParticipantCB addConstraintToParticipantCB; 

        private string fileName;
        public LoadManager(AddParticipantCB addParticipantCB,
            AddConstraintToParticipantCB addConstraintToParticipantCB,
            Logger logger)
        {
            this.logger = logger;
            this.addParticipantCB = addParticipantCB;
            this.addConstraintToParticipantCB = addConstraintToParticipantCB;
        }


        public void SetFileName(string fileName)
        {
            this.fileName = fileName;
        }

        private readonly String[] MandatoryFields = { "name", "gender", "dating males", "dating females" };
        private readonly String[] BlockPhrases = { "block", "no" };

        public void Load()
        {
            var parser = new InputFileParser(logger);
            var results = parser.parse(fileName, MandatoryFields);
            
            //Add participants
            foreach(var line in results)
            {
                var name = line.items.FirstOrDefault(i => i.Key == ColNames.Name).Value;
                var gender = GenderUtils.Parse(line.items.FirstOrDefault(i => i.Key == ColNames.Gender).Value);

                this.addParticipantCB(new Participant(name,(Gender)gender));
                logger.Log($"Adding participant {name}");
            }

            //Add constraints
            foreach(var line in results)
            {
                var name = line.items.FirstOrDefault(i => i.Key == ColNames.Name).Value;

                //Add Gender constraints
                if (BlockPhrases.Contains(line.items.FirstOrDefault(i => i.Key == ColNames.DatingMales).Value))
                    addConstraintToParticipantCB(name, new GenderBlockConstraint(Gender.Male));
                if (BlockPhrases.Contains(line.items.FirstOrDefault(i => i.Key == ColNames.DatingFemales).Value))
                    addConstraintToParticipantCB(name, new GenderBlockConstraint(Gender.Female));

                //Add participant constraints
                foreach(var i in line.items.Where( i2 => !MandatoryFields.Contains(i2.Key)))
                {
                    if (BlockPhrases.Contains(i.Value))
                    {
                        //this means we have a column with participant name and its value is some block phrase (eg - block)
                        //so we take the column name (key) and add it as a constraint
                        addConstraintToParticipantCB(name, new ParticipantBlockConstraint(i.Key));
                    }
                }

            }

        }
    }
}
