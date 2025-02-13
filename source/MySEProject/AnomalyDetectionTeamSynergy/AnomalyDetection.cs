using NeoCortexApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AnomalyDetectionTeamSynergy
{
    /// <summary>
    /// This class provides methods for detecting anomalies in a sequence of numerical data
    /// using a predictive model.
    /// </summary>
    public class AnomalyDetection
    {
        /// <summary>
        /// Determines whether a given value is an anomaly based on absolute and relative thresholds.
        /// </summary>
        /// <param name="predictedValue">The predicted value from the model.</param>
        /// <param name="actualValue">The actual value from the dataset.</param>
        /// <param name="absoluteThreshold">The absolute deviation threshold.</param>
        /// <param name="relativeThreshold">The relative deviation threshold (as a fraction).</param>
        /// <returns>Returns true if an anomaly is detected, otherwise false.</returns>
        public bool IsAnomaly(double predictedValue, double actualValue, double absoluteThreshold, double relativeThreshold)
        {
            // Calculate the absolute difference
            double absoluteDifference = Math.Abs(predictedValue - actualValue);

            // Calculate the relative difference (percentage deviation)
            double relativeDifference = absoluteDifference / actualValue;

            // Check if both thresholds are exceeded
            return absoluteDifference > absoluteThreshold && relativeDifference > relativeThreshold;
        }

        /// <summary>
        /// Detects anomalies in a given numerical sequence using a predictor model.
        /// </summary>
        /// <param name="predictor">The predictor model used for making predictions.</param>
        /// <param name="sequence">The sequence of numerical values to analyze.</param>
        /// <param name="fileName">The filename to store the results in CSV format.</param>
        public void DetectAnomaly(Predictor predictor, List<double> sequence, string fileName)
        {
            double tolerance = 0.1; // Tolerance level set to 10%
            double threshold = 1; // Absolute threshold for anomalies
            List<string> predictedSequence = new List<string> { "-" };

            Console.WriteLine("\n===========================================");
            Console.WriteLine("        ANOMALY DETECTION STARTED         ");
            Console.WriteLine("===========================================");
            Console.WriteLine($"\nSequence to Analyze: [{string.Join(", ", sequence)}]\n");
            Console.WriteLine("-------------------------------------------");

            // Iterate through the sequence to analyze anomalies
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
                    Console.WriteLine($"   - Similarity Score      : {similarity}");
                    Console.WriteLine($"   - Predicted Sequence    : {predictedInput}");

                    predictedSequence.Add(predictedNextElement.ToString());

                    // Check for anomaly
                    bool anomalyDetected = IsAnomaly(predictedNextElement, nextNumber, threshold, tolerance);
                    if (anomalyDetected)
                    {
                        Console.WriteLine("\n   !!! Anomaly Detected !!!");
                        Console.WriteLine($"   - Expected: {predictedNextElement}, Found: {nextNumber}");
                        Console.WriteLine("   - Skipping the anomalous value.");
                        predictedSequence.Add("-");
                        i++; // Skip next element due to anomaly
                    }
                    else
                    {
                        Console.WriteLine("No Anomaly Detected!");
                    }
                }
                else
                {
                    Console.WriteLine("No Predictions available");
                    predictedSequence.Add("-");
                }
                Console.WriteLine("-------------------------------------------");
            }

            Console.WriteLine($"\nPredicted Sequence: [{string.Join(", ", predictedSequence)}]\n");
            Console.WriteLine("\n===========================================");
            Console.WriteLine("        ANOMALY DETECTION COMPLETED       ");
            Console.WriteLine("===========================================\n");

            // Save results to a CSV file
            var csvWriter = new CSVHandler();
            csvWriter.SaveToCsv(fileName, sequence, predictedSequence);
            Console.WriteLine("CSV file created successfully!");
        }
    }
}
