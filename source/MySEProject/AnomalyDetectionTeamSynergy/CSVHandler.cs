using System.Globalization;

namespace AnomalyDetectionTeamSynergy
{
    /// <summary>
    /// Provides functionality to read, parse, manipulate, and save sequences from/to CSV files.
    /// This class is designed to handle numerical data sequences, ensuring they are correctly formatted and processed.
    /// </summary>
    public class CSVHandler
    {
        /// <summary>
        /// Parses numerical sequences from a CSV file. Each line in the CSV file represents a sequence of double values.
        /// The first line is assumed to be a header and is skipped. Sequences with fewer than 3 values are ignored.
        /// </summary>
        /// <param name="filePath">The path to the CSV file containing the sequences.</param>
        /// <returns>A list of sequences, where each sequence is represented as a list of double values.</returns>
        /// <exception cref="FileNotFoundException">Thrown if the specified file path does not exist.</exception>
        /// <exception cref="FormatException">Thrown if any value in the CSV cannot be parsed as a double.</exception>
        public List<List<double>> ParseSequencesFromCSV(string filePath)
        {
            // Initialize a list to store sequences
            var sequences = new List<List<double>>();

            // Read all lines from the CSV file
            Console.WriteLine("Reading lines from the file...");
            var lines = File.ReadAllLines(filePath);

            Console.WriteLine($"Total lines read: {lines.Length}");

            // Skip the first line (header) and iterate through the remaining lines
            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];
                Console.WriteLine($"Processing line {i + 1}: {line}");

                // Split the line by commas
                var values = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine($"Split values: {string.Join(", ", values)}");

                // Try to convert all values to double
                if (values.All(v => double.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out _)))
                {
                    var sequence = values.Select(v => double.Parse(v, CultureInfo.InvariantCulture)).ToList();

                    // Skip sequences with only two values
                    if (sequence.Count > 2)
                    {
                        Console.WriteLine("Valid sequence added.");
                        sequences.Add(sequence);
                    }
                    else
                    {
                        Console.WriteLine("Skipped sequence with less than 3 values.");
                    }
                }
                else
                {
                    Console.WriteLine("Skipped line due to invalid numerical data.");
                }
            }

            Console.WriteLine("Finished processing file.");
            return sequences;
        }

        /// <summary>
        /// Trims the first N elements from each sequence in a list of sequences.
        /// This method is useful for removing initial data points that may not be relevant for analysis.
        /// </summary>
        /// <param name="sequences">A list of sequences, where each sequence is a list of double values.</param>
        /// <param name="N">The number of elements to remove from the beginning of each sequence.</param>
        /// <returns>A new list of sequences with the first N elements removed from each sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the sequences parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown if N is greater than the length of any sequence.</exception>
        public List<List<double>> TrimSequences(List<List<double>> sequences, int N)
        {
            List<List<double>> trimmedSequences = new List<List<double>>();

            foreach (var sequence in sequences)
            {
                List<double> trimmedSequence = sequence.Skip(N).ToList();
                trimmedSequences.Add(trimmedSequence);
            }

            return trimmedSequences;
        }

        /// <summary>
        /// Displays the parsed sequence data in the console.
        /// This method is primarily used for debugging and verification purposes.
        /// </summary>
        /// <param name="sequences">A list of sequences to display, where each sequence is a list of double values.</param>
        /// <exception cref="ArgumentNullException">Thrown if the sequences parameter is null.</exception>
        public void DisplaySequenceData(List<List<double>> sequences)
        {
            Console.WriteLine("Displaying sequence data:");
            for (int i = 0; i < sequences.Count; i++)
            {
                Console.WriteLine($"Sequence {i + 1}: {string.Join(", ", sequences[i])}");
            }
        }

        /// <summary>
        /// Saves a sequence and its corresponding predicted sequence to a CSV file.
        /// The file is saved in a specified directory within the project, and the directory is created if it does not exist.
        /// </summary>
        /// <param name="fileName">The name of the CSV file to save the data to.</param>
        /// <param name="sequence">The actual sequence of double values.</param>
        /// <param name="predictedSequence">The predicted sequence of string values.</param>
        /// <exception cref="ArgumentException">Thrown if the sequence and predictedSequence lists are not of equal size.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown if the specified directory path is invalid.</exception>
        /// <exception cref="IOException">Thrown if an I/O error occurs while writing to the file.</exception>
        public void SaveToCsv(string fileName, List<double> sequence, List<string> predictedSequence)
        {
            string projectbaseDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName;
            string folderPath = Path.Combine(projectbaseDirectory, "ModelPredictions");

            // Ensure the directory exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);

            if (sequence.Count != predictedSequence.Count)
            {
                throw new ArgumentException("Both lists must be of equal size.");
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // Write column headers
                writer.WriteLine("Actual,Predicted");

                // Write data rows
                for (int i = 0; i < sequence.Count; i++)
                {
                    writer.WriteLine($"{sequence[i]},{predictedSequence[i]}");
                }
            }
        }
    }
}
