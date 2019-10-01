using CommandLine;
using System;
using System.Collections.Generic;

namespace MakeMono
{
    class CliOptionsParser
    {
        public MakeMonoOptions Parse(string[] args)
        {
            MakeMonoOptions output = new MakeMonoOptions();
            Parser.Default.ParseArguments<MakeMonoOptions>(args)
                .WithParsed<MakeMonoOptions>(opt => { output = opt; })
                .WithNotParsed<MakeMonoOptions>(HandleParseError);
            return output;
        }

        private void HandleParseError(IEnumerable<Error> Errors)
        {
            List<string> errors = new List<string>();
            foreach(Error error in Errors)
            {
                if(error.Tag != ErrorType.VersionRequestedError && error.Tag != ErrorType.HelpRequestedError)
                {
                    errors.Add(error.Tag.ToString());
                }
            }
            throw new Exception(String.Join("\n", errors));
        }
    }
}
