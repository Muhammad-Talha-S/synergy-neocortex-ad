using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnomalyDetectionTeamSynergy
{
    /// <summary>
    /// Handles console arguments for configuring the anomaly detection system.
    /// Parses command-line arguments and stores values for various parameters.
    /// </summary>
    public class ConsoleArgumentsHandler
    {
        /// <summary>
        /// Represents the numerical parameter N extracted from the command-line arguments.
        /// </summary>
        public int N { get; private set; }

        /// <summary>
        /// Path to the training file specified in the command-line arguments.
        /// </summary>
        public string TrainingFile { get; private set; }

        /// <summary>
        /// Path to the inferring file specified in the command-line arguments.
        /// </summary>
        public string InferringFile { get; private set; }

        /// <summary>
        /// Path to the training folder specified in the command-line arguments.
        /// </summary>
        public string TrainingFolder { get; private set; }

        /// <summary>
        /// Path to the inferring folder specified in the command-line arguments.
        /// </summary>
        public string InferringFolder { get; private set; }

        /// <summary>
        /// Tolerance value for Anomaly Detection.
        /// </summary>
        public string ToleranceValue { get; private set; }

        // Default values for parameters
        private const int DefaultN = 0;
        private const string DefaultStringValue = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleArgumentsHandler"/> class.
        /// Parses the provided command-line arguments.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public ConsoleArgumentsHandler(string[] args)
        {
            // Initialize fields with default values
            N = DefaultN;
            TrainingFile = DefaultStringValue;
            InferringFile = DefaultStringValue;
            TrainingFolder = DefaultStringValue;
            InferringFolder = DefaultStringValue;
            ToleranceValue = DefaultStringValue;

            // Parse the command-line arguments
            ParseArguments(args);
        }

        /// <summary>
        /// Parses the provided command-line arguments and assigns values accordingly.
        /// </summary>
        /// <param name="args">Array of command-line arguments.</param>
        private void ParseArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--n":
                        if (i + 1 < args.Length && int.TryParse(args[i + 1], out int nValue))
                        {
                            N = nValue;
                            i++; // Skip the next argument since it's assigned to N
                        }
                        break;

                    case "--training-file":
                        if (i + 1 < args.Length)
                        {
                            TrainingFile = args[i + 1];
                            i++; // Skip the next argument since it's assigned to TrainingFile
                        }
                        break;

                    case "--inferring-file":
                        if (i + 1 < args.Length)
                        {
                            InferringFile = args[i + 1];
                            i++; // Skip the next argument since it's assigned to InferringFile
                        }
                        break;

                    case "--training-folder":
                        if (i + 1 < args.Length)
                        {
                            TrainingFolder = args[i + 1];
                            i++; // Skip the next argument since it's assigned to TrainingFolder
                        }
                        break;

                    case "--inferring-folder":
                        if (i + 1 < args.Length)
                        {
                            InferringFolder = args[i + 1];
                            i++; // Skip the next argument since it's assigned to InferringFolder
                        }
                        break;

                    case "--tolerance":
                        if (i + 1 < args.Length)
                        {
                            ToleranceValue = args[i + 1];
                            i++; // Skip the next argument since it's assigned to Tolerance Value
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
