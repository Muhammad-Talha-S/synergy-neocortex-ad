using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnomalyDetectionTeamSynergy
{
    /// <summary>
    /// The SequenceAnalyzer class is designed to analyze sequences of numerical data.
    /// It provides methods to compute specific statistical properties from a collection
    /// of training and inferring sequences.
    /// </summary>
    public class SequenceAnalyzer
    {
        // A list of training sequences, where each sequence is a list of double values.
        private List<List<double>> all_training_sequences;

        // A list of inferring sequences, where each sequence is a list of double values.
        private List<List<double>> all_inferring_sequences;

        /// <summary>
        /// Initializes a new instance of the SequenceAnalyzer class.
        /// </summary>
        /// <param name="all_training_sequences">A list of training sequences to analyze.</param>
        /// <param name="all_inferring_sequences">A list of inferring sequences to analyze.</param>
        public SequenceAnalyzer(List<List<double>> all_training_sequences, List<List<double>> all_inferring_sequences)
        {
            this.all_training_sequences = all_training_sequences;
            this.all_inferring_sequences = all_inferring_sequences;
        }

        /// <summary>
        /// Finds the maximum value across all training and inferring sequences.
        /// </summary>
        /// <returns>The maximum value found in the combined sequences.</returns>
        public double FindMaxValue()
        {
            // Combine all sequences into a single list
            var combinedSequences = all_training_sequences.SelectMany(x => x)
                                                          .Concat(all_inferring_sequences.SelectMany(x => x))
                                                          .ToList();

            // Find and return the maximum value
            return combinedSequences.Max();
        }

        /// <summary>
        /// Calculates the difference between the two smallest values in the combined sequences.
        /// </summary>
        /// <returns>The difference between the two smallest values.</returns>
        public double FindDifferenceBetweenTwoMinValues()
        {
            // Combine all sequences into a single list
            var combinedSequences = all_training_sequences.SelectMany(x => x)
                                                          .Concat(all_inferring_sequences.SelectMany(x => x))
                                                          .ToList();

            // Find the two minimum values
            var sortedSequences = combinedSequences.OrderBy(x => x).Take(2).ToList();

            // Calculate the difference between the two minimum values
            return sortedSequences[1] - sortedSequences[0];
        }
    }
}
