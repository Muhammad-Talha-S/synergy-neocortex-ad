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
            int N = handler.N;
            string trainingFile = handler.TrainingFile;
            string inferringFile = handler.InferringFile;
            string trainingFolder = handler.TrainingFolder;
            string inferringFolder = handler.InferringFolder;

            var fileHandler = new FileHandler();

            // Lists to store all training and inferring sequences
            List<List<double>> all_training_sequences = new List<List<double>>();
            List<List<double>> all_inferring_sequences = new List<List<double>>();

            try
            {
                // Process files and extract relevant data
                fileHandler.ProcessFiles(trainingFile, inferringFile, trainingFolder, inferringFolder);

                var training_files = fileHandler.TrainingDataFiles;
                var inferring_files = fileHandler.InferringDataFiles;

                var csv_reader = new CSVHandler();
                var csv_htm_input = new CSVToHTMInput();

                // Read and parse training data files
                foreach (var filePath in training_files)
                {
                    Console.WriteLine($"\n--- Reading File: {Path.GetFileName(filePath)} ---");
                    var training_sequences = csv_reader.ParseSequencesFromCSV(filePath);
                    csv_reader.DisplaySequenceData(training_sequences);
                    all_training_sequences.AddRange(training_sequences);
                }

                // Convert training sequences to HTM input format
                var htm_training_sequence = csv_htm_input.BuildHTMInput(all_training_sequences);
                MultiSequenceLearning learning = new MultiSequenceLearning();
                var predictor = learning.Run(htm_training_sequence);

                // Read and parse inferring data files
                foreach (var filePath in inferring_files)
                {
                    Console.WriteLine($"\n--- Reading File: {Path.GetFileName(filePath)} ---");
                    var inferring_sequences = csv_reader.ParseSequencesFromCSV(filePath);
                    csv_reader.DisplaySequenceData(inferring_sequences);
                    all_inferring_sequences.AddRange(inferring_sequences);
                }

                Console.WriteLine("\n--- Displaying Trimmed Sequences ---");
                var trimmed_inferring_sequences = csv_reader.TrimSequences(all_inferring_sequences, N);
                csv_reader.DisplaySequenceData(trimmed_inferring_sequences);

                var anomaly_detection = new AnomalyDetection();

                // Perform anomaly detection on the trimmed sequences
                int sequence_no = 1;
                foreach (var sequence in trimmed_inferring_sequences)
                {
                    string prediction_data_csv = $"sequence_{sequence_no}_predictions.csv";
                    anomaly_detection.DetectAnomaly(predictor, sequence, prediction_data_csv);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
