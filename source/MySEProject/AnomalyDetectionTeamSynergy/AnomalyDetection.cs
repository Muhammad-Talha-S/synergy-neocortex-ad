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
        /// Default constructor to initialize tolerance and threshold values with default settings.
        /// - Tolerance is set to 0.1 (10%), which is the relative deviation threshold.
        /// - Threshold is set to 1, which is the absolute deviation threshold.
        /// </summary>
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
            double absoluteDifference = Math.Abs(predictedValue - actualValue);
            double relativeDifference = absoluteDifference / actualValue;
            return absoluteDifference > this.threshold && relativeDifference > this.tolerance;
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
