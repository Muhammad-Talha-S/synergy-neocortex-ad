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
            string trainingFile = handler.trainingFile;
            string inferringFile = handler.inferringFile;
            string trainingFolder = handler.trainingFolder;
            string inferringFolder = handler.inferringFolder;
            double toleranceValue = handler.toleranceValue;

            var fileHandler = new FileHandler();

            // Lists to store all training and inferring sequences
            List<List<double>> allTrainingSequences = new List<List<double>>();
            List<List<double>> allInferringSequences = new List<List<double>>();

            try
            {
                // Process files and extract relevant data
                fileHandler.ProcessFiles(trainingFile, inferringFile, trainingFolder, inferringFolder);

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

                // Read and parse inferring data files
                foreach (var filePath in inferringFiles)
                {
                    Console.WriteLine($"\n--- Reading File: {Path.GetFileName(filePath)} ---");
                    var inferringSequences = csvReader.ParseSequencesFromCSV(filePath);
                    csvReader.DisplaySequenceData(inferringSequences);
                    allInferringSequences.AddRange(inferringSequences);
                }

                Console.WriteLine("\n--- Displaying Trimmed Sequences ---");
                var trimmedInferringSequences = csvReader.TrimSequences(allInferringSequences, N);
                csvReader.DisplaySequenceData(trimmedInferringSequences);

                var sequenceAnalyzer = new SequenceAnalyzer(allTrainingSequences, trimmedInferringSequences);
                var maxValue = sequenceAnalyzer.FindMaxValue();

                // Convert training sequences to HTM input format
                var htmTrainingSequence = csvHtmInput.BuildHTMInput(allTrainingSequences);
                MultiSequenceLearning learning = new MultiSequenceLearning(maxValue);
                var predictor = learning.Run(htmTrainingSequence);

                var anomalyDetection = new AnomalyDetection(toleranceValue);

                // Perform anomaly detection on the trimmed sequences
                int sequence_no = 1;
                foreach (var sequence in trimmedInferringSequences)
                {
                    string prediction_data_csv = $"sequence_{sequence_no}_predictions.csv";
                    anomalyDetection.DetectAnomaly(predictor, sequence, prediction_data_csv);
                    sequence_no++;
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
