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
            // Handle command-line arguments (such as file paths, folder paths, N value, tolerance value)
            var handler = new ConsoleArgumentsHandler(args);

            // Retrieve parsed argument values
            int N = handler.N;                                    // Number of elements to trim from inferring sequence
            string trainingFile = handler.trainingFile;           // Single training file path
            string inferringFile = handler.inferringFile;         // Single inferring file path
            string trainingFolder = handler.trainingFolder;       // Folder path for training files
            string inferringFolder = handler.inferringFolder;     // Folder path for inferring files
            double toleranceValue = handler.toleranceValue;       // User-defined anomaly tolerance value

            var fileHandler = new FileHandler();

            // Lists to store all training and inferring sequences after processing
            List<List<double>> allTrainingSequences = new List<List<double>>();
            List<List<double>> allInferringSequences = new List<List<double>>();

            try
            {
                // Process the provided files and folders to extract training and inferring file paths
                fileHandler.ProcessFiles(trainingFile, inferringFile, trainingFolder, inferringFolder);

                // Get the processed list of file paths
                var trainingFiles = fileHandler.TrainingDataFiles;
                var inferringFiles = fileHandler.InferringDataFiles;

                var csvReader = new CSVHandler();
                var csvHtmInput = new CSVToHTMInput();

                // --- Training Data Processing ---
                // Read, parse, and store sequences from each training CSV file
                foreach (var filePath in trainingFiles)
                {
                    Console.WriteLine($"\n--- Reading File: {Path.GetFileName(filePath)} ---");
                    var trainingSequences = csvReader.ParseSequencesFromCSV(filePath);   // Extract sequences
                    csvReader.DisplaySequenceData(trainingSequences);                    // Display the sequences
                    allTrainingSequences.AddRange(trainingSequences);                    // Add to master list
                }

                // --- Inferring Data Processing ---
                // Read, parse, and store sequences from each inferring CSV file
                foreach (var filePath in inferringFiles)
                {
                    Console.WriteLine($"\n--- Reading File: {Path.GetFileName(filePath)} ---");
                    var inferringSequences = csvReader.ParseSequencesFromCSV(filePath);   // Extract sequences
                    csvReader.DisplaySequenceData(inferringSequences);                    // Display the sequences
                    allInferringSequences.AddRange(inferringSequences);                   // Add to master list
                }

                // --- Sequence Trimming ---
                Console.WriteLine("\n--- Displaying Trimmed Sequences ---");
                // Trim each inferring sequence to only contain 'N' elements
                var trimmedInferringSequences = csvReader.TrimSequences(allInferringSequences, N);
                csvReader.DisplaySequenceData(trimmedInferringSequences);                 // Display trimmed sequences

                // --- Sequence Analysis ---
                var sequenceAnalyzer = new SequenceAnalyzer(allTrainingSequences, trimmedInferringSequences);

                // Find max value across all sequences for scaling input to HTM
                var maxValue = sequenceAnalyzer.FindMaxValue();

                // Calculate threshold based on min value differences (used for relative anomaly detection)
                var relativeThreshold = sequenceAnalyzer.FindDifferenceBetweenTwoMinValues();

                // --- HTM Model Training ---
                // Convert training sequences into HTM-compatible input format
                var htmTrainingSequence = csvHtmInput.BuildHTMInput(allTrainingSequences);

                // Initialize and train the HTM model using the processed input
                MultiSequenceLearning learning = new MultiSequenceLearning(maxValue);
                var predictor = learning.Run(htmTrainingSequence);

                // --- Anomaly Detection ---
                // Initialize anomaly detection class with tolerance and threshold values
                var anomalyDetection = new AnomalyDetection(toleranceValue, relativeThreshold);

                // Perform anomaly detection on each trimmed inferring sequence
                int sequence_no = 1;
                foreach (var sequence in trimmedInferringSequences)
                {
                    // Output file for storing predictions
                    string prediction_data_csv = $"sequence_{sequence_no}_predictions.csv";

                    // Detect anomalies and generate prediction CSV
                    anomalyDetection.DetectAnomaly(predictor, sequence, prediction_data_csv);
                    sequence_no++;
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Program end
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
