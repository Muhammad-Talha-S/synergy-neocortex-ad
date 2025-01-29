using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AnomalyDetectionTeamSynergy
{
    /// <summary>
    /// Provides functionality to read and parse sequences from a CSV file.
    /// </summary>
    public class CSVReader
    {
        /// <summary>
        /// Parses numerical sequences from a CSV file.
        /// </summary>
        /// <param name="filePath">The path to the CSV file.</param>
        /// <returns>A list of sequences, where each sequence is a list of double values.</returns>
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
        /// </summary>
        /// <param name="sequences">A list of lists, where each inner list represents a sequence of double values.</param>
        /// <param name="N">The number of elements to remove from the beginning of each sequence.</param>
        /// <returns>A new list of lists where each inner list has its first N elements removed.</returns>
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
        /// </summary>
        /// <param name="sequences">A list of numerical sequences to display.</param>
        public void DisplaySequenceData(List<List<double>> sequences)
        {
            Console.WriteLine("Displaying sequence data:");
            for (int i = 0; i < sequences.Count; i++)
            {
                Console.WriteLine($"Sequence {i + 1}: {string.Join(", ", sequences[i])}");
            }
        }
    }
}
