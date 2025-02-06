using NeoCortexApi;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnomalyDetectionTeamSynergy
{
    public class AnomalyDetection
    {
        // Method to calculate anomaly based on similarity threshold
        public bool IsAnomaly(double predictedValue, double actualValue, double absoluteThreshold, double relativeThreshold)
        {
            // Calculate the absolute difference
            double absoluteDifference = Math.Abs(predictedValue - actualValue);

            // Calculate the relative difference (percentage deviation)
            double relativeDifference = absoluteDifference / actualValue;

            // Check if both thresholds are exceeded
            if (absoluteDifference > absoluteThreshold && relativeDifference > relativeThreshold)
            {
                return true; // Anomaly detected
            }

            return false; // No anomaly
        }

        public void DetectAnomaly(Predictor predictor, List<double> sequence)
        {
            double tolerance = 0.1; // Tolerance level set to 10%
            double threshold = 1; // Absolute Threshold

            Console.WriteLine("------------------------------");
            Console.WriteLine();
            Console.WriteLine("Testing the sequence for anomaly detection: " + string.Join(", ", sequence) + ".");

            for (int i = 0; i < sequence.Count - 1; i++)
            {
                double currentNumber = sequence[i];
                double nextNumber = sequence[i + 1];
                var predictionResults = predictor.Predict(currentNumber);

                Console.WriteLine($"Current Element: {currentNumber}");

                if (predictionResults.Count > 0)
                {
                    var bestPrediction = predictionResults.First();
                    string predictedInput = bestPrediction.PredictedInput;
                    string[] predictedSequenceParts = predictedInput.Split('-');
                    double similarity = bestPrediction.Similarity;

                    // Parse the predicted next element from the sequence
                    if (double.TryParse(predictedSequenceParts.Last(), out double predictedNextElement))
                    {
                        Console.WriteLine($"Predicted Next Element: {predictedNextElement}");
                        Console.WriteLine($"Actual Next Element: {nextNumber}");
                        Console.WriteLine($"Predicted Sequence: {predictedInput}");

                        // Check for anomaly
                        bool anomalyDetected = IsAnomaly(predictedNextElement, nextNumber, threshold, tolerance);
                        if (anomalyDetected)
                        {
                            Console.WriteLine("Anomaly Detected!");
                            Console.WriteLine("Skipping anomaly value. Using predicted value for the next element in the sequence.");

                            // Replace the anomaly with the predicted value
                            sequence[i + 1] = predictedNextElement;
                        }
                    }
                }
            }
        }

    }
}
