using CommandLine;
using System;
using System.Collections.Generic;

namespace MakeMono
{
    class CliOptionsParser
    {
        public MakeMonoOptions Parse(string[] args)
        {
            MakeMonoOptions output = null;
            Parser.Default.ParseArguments<MakeMonoOptions>(args)
                .WithParsed<MakeMonoOptions>(opt => { output = opt; })
                .WithNotParsed<MakeMonoOptions>(HandleParseError);
            return output;
        }

        private void HandleParseError(IEnumerable<Error> Errors)
        {
            foreach(Error error in Errors)
            {
                if(error.Tag != ErrorType.VersionRequestedError && error.Tag != ErrorType.HelpRequestedError)
                {
                    Console.WriteLine("Error: " + error.Tag.ToString());
                }
            }
        }
    }
}
