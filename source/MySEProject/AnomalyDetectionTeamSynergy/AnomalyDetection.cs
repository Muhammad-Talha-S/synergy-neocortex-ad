using NeoCortexApi;
using System.Collections.Generic;

namespace AnomalyDetectionTeamSynergy
{
    public class AnomalyDetection
    {
        private double _threshold;

        // Method to calculate anomaly based on similarity threshold
        public bool IsAnomaly(double predictedValue, double actualValue)
        {
            return Math.Abs(predictedValue - actualValue) > _threshold;
        }

        public void DetectAnomaly(Predictor predictor, List<double> sequence)
        {
            double tolerance = 0.1; // Tolerance level set to 10%

            Console.WriteLine("------------------------------");
            Console.WriteLine();
            Console.WriteLine("Testing the sequence for anomaly detection: " + string.Join(", ", sequence) + ".");

            for (int i = 0; i < sequence.Count; i++)
            {
                var current_number = sequence[i];
                var result = predictor.Predict(current_number);
                Console.WriteLine($"Current Element: {current_number}");

            }


        }
    }
}
