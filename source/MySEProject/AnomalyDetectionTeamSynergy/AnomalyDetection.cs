using NeoCortexApi;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnomalyDetectionTeamSynergy
{
    public class AnomalyDetection
    {
        // Method to calculate anomaly based on similarity threshold
        public bool IsAnomaly(double predictedValue, double actualValue, double tolerance)
        {
            var anomalyScore = Math.Abs(predictedValue - actualValue);
            var deviation = anomalyScore / actualValue;

            if (deviation < tolerance)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void DetectAnomaly(Predictor predictor, List<double> sequence)
        {
            double tolerance = 0.1; // Tolerance level set to 10%

            Console.WriteLine("------------------------------");
            Console.WriteLine();
            Console.WriteLine("Testing the sequence for anomaly detection: " + string.Join(", ", sequence) + ".");

            for (int i = 0; i < sequence.Count - 1; i++)
            {
                var current_number = sequence[i];
                var next_number = sequence[i + 1];
                var result = predictor.Predict(current_number);
                Console.WriteLine($"Current Element: {current_number}");

                Console.WriteLine($"Result: {result}");
                Console.WriteLine($"Result Count: {result.Count}");

                if (result.Count > 0)
                {
                    var predicted_input = result.First().PredictedInput;
                    var prrdicted_sequence = predicted_input.Split('-');
                    var similarity = result.First().Similarity;

                    double predicted_next_element = double.Parse(prrdicted_sequence.Last());
                    Console.WriteLine($"predicted next element: {predicted_next_element}");
                    Console.WriteLine($"actual next element: {next_number}");
                    Console.WriteLine($"predicted sequence: {predicted_input}");
                    bool anomaly_detected = IsAnomaly(predicted_next_element, next_number, tolerance);
                    if (anomaly_detected)
                    {
                        Console.WriteLine($"Anomaly Detected!");
                    }

                }
            }
        }

        }
}
