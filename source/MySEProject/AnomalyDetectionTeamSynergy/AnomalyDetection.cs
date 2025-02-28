using NeoCortexApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AnomalyDetectionTeamSynergy
{
    /// <summary>
    /// This class provides methods for detecting anomalies in a sequence of numerical data
    /// using a predictive model. Anomalies are detected based on both relative and absolute
    /// deviation thresholds.
    /// </summary>
    public class AnomalyDetection
    {
        private double tolerance;
        private double threshold;

        /// <summary>
        /// Initializes the anomaly detection model with specified tolerance and threshold values.
        ///
        /// - `toleranceValue` is set to 0.1 (10%) by default. If the user wants to change this value, 
        ///   they must pass the `--tolerance` argument via the console.
        /// - `threshold` is dynamically set to the minimum of two numbers in both the training and 
        ///   inferring sequences.
        /// </summary>
        /// <param name="toleranceValue">The relative deviation tolerance (default: 0.1 or 10%).</param>
        /// <param name="relativeThreshold">The absolute deviation threshold, determined as the minimum 
        ///                                 value from both training and inference sequences.</param>
        public AnomalyDetection(double toleranceValue, double relativeThreshold)
        {
            this.tolerance = toleranceValue;
            this.threshold = relativeThreshold;
        }


        /// <summary>
        /// Determines if the difference between the predicted value and the actual value
        /// constitutes an anomaly based on the predefined tolerance and threshold.
        /// </summary>
        /// <param name="predictedValue">The value predicted by the model.</param>
        /// <param name="actualValue">The actual observed value.</param>
        /// <returns>True if an anomaly is detected, otherwise false.</returns>
        public bool IsAnomaly(double predictedValue, double actualValue)
        {
            // Calculate the absolute difference between predicted and actual values
            double absoluteDifference = Math.Abs(predictedValue - actualValue);

            // Calculate the relative difference as a fraction of the actual value
            double relativeDifference = absoluteDifference / actualValue;

            // Check if the absolute difference exceeds the defined threshold
            bool exceedsThreshold = absoluteDifference > this.threshold;

            // Check if the relative difference exceeds the allowed tolerance
            bool exceedsTolerance = relativeDifference > this.tolerance;

            // If either condition is not met, return false
            if (!exceedsThreshold)
            {
                return false;
            }

            if (!exceedsTolerance)
            {
                return false;
            }

            // Both conditions are met, so return true (indicating an anomaly)
            return true;
        }

        /// <summary>
        /// Detects anomalies in a sequence of numerical data using a predictive model.
        /// The method processes each element in the sequence, predicts the next value,
        /// and checks for anomalies based on the predicted and actual values.
        /// </summary>
        /// <param name="predictor">The predictor model used for forecasting the next value in the sequence.</param>
        /// <param name="sequence">The sequence of numerical data to analyze.</param>
        /// <param name="fileName">The name of the CSV file to save the results.</param>
        public void DetectAnomaly(Predictor predictor, List<double> sequence, string fileName)
        {
            List<string> predictedSequence = new List<string> { "-" };

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

                    var predictedNextElement = double.Parse(predictedSequenceParts.Last());

                    Console.WriteLine($"   - Predicted Next Element: {predictedNextElement}");
                    Console.WriteLine($"   - Actual Next Element   : {nextNumber}");
                    Console.WriteLine($"   - Similarity Score      : {similarity}");
                    Console.WriteLine($"   - Predicted Sequence    : {predictedInput}");

                    predictedSequence.Add(predictedNextElement.ToString());

                    // Check for anomaly
                    bool anomalyDetected = IsAnomaly(predictedNextElement, nextNumber);
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
