using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
   

namespace MatchingGame.Parsing
{
    enum ParserState
    {
        Created, 
        Started,
        Parsing,
        Completed,
        Error //unrecoverable error
    }

    public class ParsingException : Exception
    {
        public ParsingException(string message) : base(message)
        {
        }
     
    }

    struct ParsedLine
    {
        uint index;
        IEnumerable<KeyValuePair<string, string>> items;

        public ParsedLine(uint index, IEnumerable<KeyValuePair<string, string>> items)
        {
            this.index = index;
            this.items = items;
        }
    }

class InputFileParser
    {
        private ParserState state = ParserState.Created;
        private string fileName;
        

        public InputFileParser(string fileName)
        {
            this.fileName = fileName;
        }

        private readonly String[] MandatoryFields = { "name", "gender", "dating males", "dating females" };

        public IEnumerable<ParsedLine> parse()
        {
            var csvParser = new TextFieldParser(fileName);
            csvParser.Delimiters = new string[] { "," };

            //Read header line
            var headerFields = csvParser.ReadFields().Select(f => f.Trim().ToLower()).ToArray();

            //verify mandatory headers
            var missingHeaders = MandatoryFields.Where(f => !headerFields.Contains(f));
            if (missingHeaders.Count() > 0)
                throw new ParsingException($"Failed parsing file '{fileName}'. The following headers are missing: {string.Join("; ", missingHeaders)}");

            //read line by line
            var parsedLines = new List<ParsedLine>();
            uint lineIndex = 0;
            var lineFields = csvParser.ReadFields();
           
            while (lineFields  != null)
            {
                //create a list of key/values pairs
                var items = headerFields.Zip(lineFields, (a, b) => new KeyValuePair<string, string>(a, b));
                parsedLines.Append(new ParsedLine(lineIndex, items));


                lineFields = csvParser.ReadFields();
                lineIndex++;
            }

            return parsedLines;
        }
    }
}
