using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnomalyDetectionTeamSynergy
{
    public class ConsoleArgumentsHandler
    {
        public int N { get; private set; }
        public string TrainingFile { get; private set; }
        public string InferringFile { get; private set; }
        public string TrainingFolder { get; private set; }
        public string InferringFolder { get; private set; }

        // Default values
        private const int DefaultN = 0;
        private const string DefaultStringValue = "";

        public ConsoleArgumentsHandler(string[] args)
        {
            // Initialize with default values
            N = DefaultN;
            TrainingFile = DefaultStringValue;
            InferringFile = DefaultStringValue;
            TrainingFolder = DefaultStringValue;
            InferringFolder = DefaultStringValue;

            ParseArguments(args);
        }

        private void ParseArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-n":
                        if (i + 1 < args.Length && int.TryParse(args[i + 1], out int nValue))
                        {
                            N = nValue;
                            i++; // Skip the next argument since it's the value for N
                        }
                        break;

                    case "--training-file":
                        if (i + 1 < args.Length)
                        {
                            TrainingFile = args[i + 1];
                            i++; // Skip the next argument since it's the value for TrainingFile
                        }
                        break;

                    case "--inferring-file":
                        if (i + 1 < args.Length)
                        {
                            InferringFile = args[i + 1];
                            i++; // Skip the next argument since it's the value for InferringFile
                        }
                        break;

                    case "--training-folder":
                        if (i + 1 < args.Length)
                        {
                            TrainingFolder = args[i + 1];
                            i++; // Skip the next argument since it's the value for TrainingFolder
                        }
                        break;

                    case "--inferring-folder":
                        if (i + 1 < args.Length)
                        {
                            InferringFolder = args[i + 1];
                            i++; // Skip the next argument since it's the value for InferringFolder
                        }
                        break;

                    default:
                        // Handle unknown arguments or ignore them
                        Console.WriteLine($"Unknown argument: {args[i]}");
                        break;
                }
            }
        }
    }
}