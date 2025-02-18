using NeoCortexApi;
using System.Linq.Expressions;

namespace AnomalyDetectionTeamSynergy
{
    /// <summary>
    /// The main program class for anomaly detection using HTM (Hierarchical Temporal Memory).
    /// It reads time-series data from CSV files, processes it for training and inferring,
    /// and detects anomalies based on learned patterns.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The entry point of the application.
        /// </summary>
        /// <param name="args">Command-line arguments containing file paths and parameters.</param>
        static void Main(string[] args)
        {
            // Handle command-line arguments
            var handler = new ConsoleArgumentsHandler(args);

            // Retrieve parsed argument values
            int N = handler.N; // Number of elements to consider in each sequence
            string trainingFile = handler.trainingFile; // Path to the training data file
            string inferringFile = handler.inferringFile; // Path to the inferring data file
            string trainingFolder = handler.trainingFolder; // Path to the folder containing training data files
            string inferringFolder = handler.inferringFolder; // Path to the folder containing inferring data files
            double toleranceValue = handler.toleranceValue; // Tolerance value for anomaly detection

            var fileHandler = new FileHandler();

            // Lists to store all training and inferring sequences
            List<List<double>> allTrainingSequences = new List<List<double>>();
            List<List<double>> allInferringSequences = new List<List<double>>();

            try
            {
                // Process files and extract relevant data
                fileHandler.ProcessFiles(trainingFile, inferringFile, trainingFolder, inferringFolder);

                // Retrieve the list of training and inferring data files
                var trainingFiles = fileHandler.TrainingDataFiles;
                var inferringFiles = fileHandler.InferringDataFiles;

                var csvReader = new CSVHandler();
                var csvHtmInput = new CSVToHTMInput();

                // Read and parse training data files
                foreach (var filePath in trainingFiles)
                {
                    Console.WriteLine($"\n--- Reading File: {Path.GetFileName(filePath)} ---");
                    var trainingSequences = csvReader.ParseSequencesFromCSV(filePath);
                    csvReader.DisplaySequenceData(trainingSequences);
                    allTrainingSequences.AddRange(trainingSequences);
                }

                // Convert training sequences to HTM input format
                var htmTrainingSequence = csvHtmInput.BuildHTMInput(allTrainingSequences);

                // Initialize and run the MultiSequenceLearning algorithm to train the model
                MultiSequenceLearning learning = new MultiSequenceLearning();
                var predictor = learning.Run(htmTrainingSequence);

                // Read and parse inferring data files
                foreach (var filePath in inferringFiles)
                {
                    Console.WriteLine($"\n--- Reading File: {Path.GetFileName(filePath)} ---");
                    var inferringSequences = csvReader.ParseSequencesFromCSV(filePath);
                    csvReader.DisplaySequenceData(inferringSequences);
                    allInferringSequences.AddRange(inferringSequences);
                }

                // Trim the inferring sequences to the specified length N
                Console.WriteLine("\n--- Displaying Trimmed Sequences ---");
                var trimmedInferringSequences = csvReader.TrimSequences(allInferringSequences, N);
                csvReader.DisplaySequenceData(trimmedInferringSequences);

                // Initialize the anomaly detection module with the specified tolerance value
                var anomalyDetection = new AnomalyDetection(toleranceValue);

                // Perform anomaly detection on each trimmed sequence
                int sequence_no = 1;
                foreach (var sequence in trimmedInferringSequences)
                {
                    string predictionDataCsv = $"sequence_{sequence_no}_predictions.csv";
                    anomalyDetection.DetectAnomaly(predictor, sequence, predictionDataCsv);
                    sequence_no++;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during execution
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Prompt the user to exit the application
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
