using System;

namespace AnomalyDetectionTeamSynergy
{
    /// <summary>
    /// Handles console arguments for configuring the anomaly detection system.
    /// This class parses command-line arguments and stores values for various parameters such as file paths, folder paths, and numerical values.
    /// </summary>
    public class ConsoleArgumentsHandler
    {
        /// <summary>
        /// Represents the numerical parameter N extracted from the command-line arguments.
        /// This value is used to trim the inferring sequences from the start.
        /// </summary>
        public int N { get; private set; }

        /// <summary>
        /// Path to the training file specified in the command-line arguments.
        /// This file is used for training the anomaly detection model.
        /// </summary>
        public string trainingFile { get; private set; }

        /// <summary>
        /// Path to the inferring file specified in the command-line arguments.
        /// This file is used for inferring (testing) the anomaly detection model.
        /// </summary>
        public string inferringFile { get; private set; }

        /// <summary>
        /// Path to the training folder specified in the command-line arguments.
        /// This folder contains multiple files for training the anomaly detection model.
        /// </summary>
        public string trainingFolder { get; private set; }

        /// <summary>
        /// Path to the inferring folder specified in the command-line arguments.
        /// This folder contains multiple files for inferring (testing) the anomaly detection model.
        /// </summary>
        public string inferringFolder { get; private set; }

        /// <summary>
        /// Tolerance value for the anomaly detection system.
        /// This value determines the sensitivity of the anomaly detection algorithm.
        /// </summary>
        public double toleranceValue { get; private set; }

        // Default values for parameters
        private const int DefaultN = 0;
        private const string DefaultStringPaths = "";
        private const double DefaultToleranceValue = 0.1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleArgumentsHandler"/> class.
        /// Parses the provided command-line arguments and assigns values to the corresponding properties.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        public ConsoleArgumentsHandler(string[] args)
        {
            // Initialize fields with default values
            N = DefaultN;
            trainingFile = DefaultStringPaths;
            inferringFile = DefaultStringPaths;
            trainingFolder = DefaultStringPaths;
            inferringFolder = DefaultStringPaths;
            toleranceValue = DefaultToleranceValue;

            // Parse the command-line arguments
            ParseArguments(args);
        }

        /// <summary>
        /// Parses the provided command-line arguments and assigns values to the corresponding properties.
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
                            trainingFile = args[i + 1];
                            i++; // Skip the next argument since it's assigned to trainingFile
                        }
                        break;

                    case "--inferring-file":
                        if (i + 1 < args.Length)
                        {
                            inferringFile = args[i + 1];
                            i++; // Skip the next argument since it's assigned to inferringFile
                        }
                        break;

                    case "--training-folder":
                        if (i + 1 < args.Length)
                        {
                            trainingFolder = args[i + 1];
                            i++; // Skip the next argument since it's assigned to trainingFolder
                        }
                        break;

                    case "--inferring-folder":
                        if (i + 1 < args.Length)
                        {
                            inferringFolder = args[i + 1];
                            i++; // Skip the next argument since it's assigned to inferringFolder
                        }
                        break;

                    case "--tolerance":
                        if (i + 1 < args.Length && double.TryParse(args[i + 1], out double readToleranceValue))
                        {
                            toleranceValue = readToleranceValue;
                            i++; // Skip the next argument since it's assigned to toleranceValue
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
