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

            Console.WriteLine("\n===========================================");
            Console.WriteLine("        ANOMALY DETECTION STARTED         ");
            Console.WriteLine("===========================================");
            Console.WriteLine($"\nSequence to Analyze: [{string.Join(", ", sequence)}]\n");
            Console.WriteLine("-------------------------------------------");

            for (int i = 0; i < sequence.Count - 1; i++)
            {
                double currentNumber = sequence[i];
                double nextNumber = sequence[i + 1];
                var predictionResults = predictor.Predict(currentNumber);

                Console.WriteLine($"Processing Element: {currentNumber}");

                if (predictionResults.Count > 0)
                {
                    var bestPrediction = predictionResults.First();
                    string predictedInput = bestPrediction.PredictedInput;
                    string[] predictedSequenceParts = predictedInput.Split('-');
                    double similarity = bestPrediction.Similarity;

                    // Parse the predicted next element from the sequence
                    var predictedNextElement = double.Parse(predictedSequenceParts.Last());


                    Console.WriteLine($"   - Predicted Next Element: {predictedNextElement}");
                    Console.WriteLine($"   - Actual Next Element   : {nextNumber}");
                    Console.WriteLine($"   - Predicted Sequence    : {predictedInput}");
                    Console.WriteLine($"   - Similarity Score      : {similarity}");

                    // Check for anomaly
                    bool anomalyDetected = IsAnomaly(predictedNextElement, nextNumber, threshold, tolerance);
                    if (anomalyDetected)
                    {
                        Console.WriteLine("\n   !!! Anomaly Detected !!!");
                        Console.WriteLine($"   - Expected: {predictedNextElement}, Found: {nextNumber}");
                        Console.WriteLine("   - Skip the anomalous value.");
                        i++; // skip next element due to anomaly
                    }
                    else
                    {
                        Console.WriteLine("No Anomaly Detected!");
                    }

                }
                else
                {
                    Console.WriteLine("No Predictions available");
                }
                Console.WriteLine("-------------------------------------------");
            }
            Console.WriteLine("\n===========================================");
            Console.WriteLine("        ANOMALY DETECTION COMPLETED       ");
            Console.WriteLine("===========================================\n");
        }

    }
}