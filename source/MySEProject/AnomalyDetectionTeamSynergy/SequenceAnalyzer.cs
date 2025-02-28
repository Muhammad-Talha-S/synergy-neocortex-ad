using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnomalyDetectionTeamSynergy
{
    public class SequenceAnalyzer
    {
        private List<List<double>> all_training_sequences;
        private List<List<double>> all_inferring_sequences;

        public SequenceAnalyzer(List<List<double>> all_training_sequences, List<List<double>> all_inferring_sequences)
        {
            this.all_training_sequences = all_training_sequences;
            this.all_inferring_sequences = all_inferring_sequences;
        }

        // Method to find the maximum value in both sequences combined
        public double FindMaxValue()
        {
            // Combine all sequences into a single list
            var combinedSequences = all_training_sequences.SelectMany(x => x)
                                                          .Concat(all_inferring_sequences.SelectMany(x => x))
                                                          .ToList();

            // Find and return the maximum value
            return combinedSequences.Max();
        }
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
