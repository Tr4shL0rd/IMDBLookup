using System;
using CommandLine;
using System.Collections.Generic;


namespace imdbLookup
{
    class Options
    {
        [Option('V', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('v', "version", Required = false,  HelpText = "Displays version Number.")]
        public bool version { get; set; }
        
        
        [Option('c', "commands", Required = false, HelpText = "Displays all commands.")]
        public bool commands { get; set; }
        
        [Option('s', "show", Required = false, HelpText = "Name of the Show/Movie.")]
        public string show { get; set; }

        

    }
}
