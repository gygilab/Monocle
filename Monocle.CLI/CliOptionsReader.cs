using CommandLine;
using System;
using System.Collections.Generic;

namespace MakeMono
{
    /// <summary>
    /// Class for processing CLI input arguments
    /// </summary>
    public class CliOptionsParser
    {
        /// <summary>
        /// Parse user input arguments
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public MakeMonoOptions Parse(string[] args)
        {
            MakeMonoOptions output = new MakeMonoOptions();
            Parser.Default.ParseArguments<MakeMonoOptions>(args)
                .WithParsed(opt => { output = opt; })
                .WithNotParsed(HandleParseError);
            return output;
        }

        /// <summary>
        /// Handle and report errors in arguments
        /// </summary>
        /// <param name="Errors"></param>
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
