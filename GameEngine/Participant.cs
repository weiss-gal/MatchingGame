using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchingGame.GameEngine
{
    public enum Gender
    {
        Male,
        Female
    }

    public static class GenderUtils
    {

        static public Gender? Parse(string genderString)
        {
            switch (genderString.ToLower())
            {
                case "male":
                case "m":
                    return Gender.Male;
                case "female":
                case "f":
                    return Gender.Female;
            }

            return null;
        }

    }
    
    internal static class ParticipantUtils
    {
        internal static TextInfo textInfo = new CultureInfo("en-US",false).TextInfo;
    }

    public class Participant
    {
        public Participant (String name, Gender gender)
        {
            this.name = name.ToLower(); //participant name is case insensitive
            this.gender = gender;
        }

        public String name {get; }
        public Gender gender {get; }

        /* Overrides */

        public override string ToString()
        {
            return ParticipantUtils.textInfo.ToTitleCase(this.name);
        }

    }
}
