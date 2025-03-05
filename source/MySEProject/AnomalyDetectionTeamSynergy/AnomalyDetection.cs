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
        /// Detects anomalies in a sequence of numerical data using a trained predictive model.
        /// The method iterates through the sequence, predicts the next value, and compares 
        /// it with the actual next value to detect anomalies.
        /// </summary>
        /// <param name="predictor">The trained predictor model used for forecasting the next value in the sequence.</param>
        /// <param name="inferringSequence">The numerical sequence to analyze for anomalies.</param>
        /// <param name="csvFileName">The name of the CSV file where results will be saved.</param>
        public void DetectAnomaly(Predictor predictor, List<double> inferringSequence, string csvFileName)
        {
            List<string> predictedValues = new List<string> { "-" };
            string bestPredictionSequence = "";

            Console.WriteLine("\n===========================================");
            Console.WriteLine("        ANOMALY DETECTION STARTED         ");
            Console.WriteLine("===========================================");
            Console.WriteLine($"\nSequence to Analyze: [{string.Join(", ", inferringSequence)}]\n");
            Console.WriteLine("-------------------------------------------");

            for (int i = 0; i < inferringSequence.Count - 1; i++)
            {
                double currentValue = inferringSequence[i];
                double actualNextValue = inferringSequence[i + 1];
                var predictionList = predictor.Predict(currentValue);

                Console.WriteLine($"Processing Element: {currentValue}");

                if (predictionList.Count > 0)
                {
                    var topPrediction = predictionList.First();
                    bestPredictionSequence = topPrediction.PredictedInput;
                    string[] predictedSequenceParts = bestPredictionSequence.Split('-');
                    double predictionSimilarityScore = topPrediction.Similarity;

                    double predictedNextValue = double.Parse(predictedSequenceParts.Last());

                    Console.WriteLine($"   - Predicted Next Value : {predictedNextValue}");
                    Console.WriteLine($"   - Actual Next Value    : {actualNextValue}");
                    Console.WriteLine($"   - Similarity Score     : {predictionSimilarityScore}");
                    Console.WriteLine($"   - Predicted Sequence   : {bestPredictionSequence}");

                    predictedValues.Add(predictedNextValue.ToString());

                    // Check for anomaly
                    bool isAnomalous = IsAnomaly(predictedNextValue, actualNextValue);
                    if (isAnomalous)
                    {
                        Console.WriteLine("\n   !!! Anomaly Detected !!!");
                        Console.WriteLine($"   - Expected: {predictedNextValue}, Found: {actualNextValue}");
                        Console.WriteLine("   - Skipping the anomalous value.");
                        predictedValues.Add("-");
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
                    predictedValues.Add("-");
                }
                Console.WriteLine("-------------------------------------------");
            }

            Console.WriteLine($"\nPredicted Values: [{string.Join(", ", predictedValues)}]\n");
            Console.WriteLine("\n===========================================");
            Console.WriteLine("        ANOMALY DETECTION COMPLETED       ");
            Console.WriteLine("===========================================\n");

            // Save results to a CSV file
            if (!string.IsNullOrEmpty(bestPredictionSequence))
            {
                List<double> bestMatchedSequence = bestPredictionSequence.Substring(3) // Remove "S1_"
                                          .Split('-')
                                          .Select(double.Parse)
                                          .ToList();
                var csvWriter = new CSVHandler();
                csvWriter.SaveToCsv(csvFileName, inferringSequence, predictedValues, bestMatchedSequence);
                Console.WriteLine("CSV file created successfully!");
            }
            else
            {
                Console.WriteLine("No predictions were made. CSV file not created.");
            }
        }

    }
}
