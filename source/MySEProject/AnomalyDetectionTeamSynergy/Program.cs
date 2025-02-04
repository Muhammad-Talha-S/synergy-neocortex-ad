using NeoCortexApi;
using System.Linq.Expressions;

namespace AnomalyDetectionTeamSynergy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var handler = new ConsoleArgumentsHandler(args);

            // Access the parsed values
            int N = handler.N;
            string trainingFile = handler.TrainingFile;
            string inferringFile = handler.InferringFile;
            string trainingFolder = handler.TrainingFolder;
            string inferringFolder = handler.InferringFolder;

            var fileHandler = new FileHandler();

            List<List<double>> all_training_sequences = new List<List<double>>();
            List<List<double>> all_inferring_sequences = new List<List<double>>();

            try
            {
                fileHandler.ProcessFiles(trainingFile, inferringFile, trainingFolder, inferringFolder);

                var training_files = fileHandler.TrainingDataFiles;
                var inferring_files = fileHandler.InferringDataFiles;

                var csv_reader = new CSVReader();
                var csv_htm_input = new CSVToHTMInput();

                foreach (var filePath in fileHandler.TrainingDataFiles)
                {
                    Console.WriteLine($"\n--- Reading File: {Path.GetFileName(filePath)} ---");
                    var training_sequences = csv_reader.ParseSequencesFromCSV(filePath);
                    csv_reader.DisplaySequenceData(training_sequences);
                    all_training_sequences.AddRange(training_sequences);
                }

                var htm_training_sequence = csv_htm_input.BuildHTMInput(all_training_sequences);
                MultiSequenceLearning learning = new MultiSequenceLearning();
                var predictor = learning.Run(htm_training_sequence);

                foreach (var filePath in fileHandler.InferringDataFiles)
                {
                    Console.WriteLine($"\n--- Reading File: {Path.GetFileName(filePath)} ---");

                    var inferring_sequences = csv_reader.ParseSequencesFromCSV(filePath);
                    csv_reader.DisplaySequenceData(inferring_sequences);
                    all_inferring_sequences.AddRange(inferring_sequences);
                }

                Console.WriteLine("\n--- Displaying Trimmed Sequences ---");
                var trimed_inferring_sequences = csv_reader.TrimSequences(all_inferring_sequences, N);
                csv_reader.DisplaySequenceData(trimed_inferring_sequences);

                var anomaly_detection = new AnomalyDetection();

                foreach (var sequence in trimed_inferring_sequences)
                {
                    anomaly_detection.DetectAnomaly(predictor, sequence);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
          //  RunMultiSequenceLearningExperiment.Run();
        }
       
    }
}
