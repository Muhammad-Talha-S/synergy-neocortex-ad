using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AnomalyDetectionTeamSynergy;

namespace TestingAnomalyDetectionTeamSynergy
{
    [TestClass]
    public class SequenceAnalyzerTests
    {
        [TestMethod]
        public void Test_FindDifferenceBetweenTwoMinValues()
        {
            // Arrange
            var trainingSequences = new List<List<double>>
            {
                new List<double> {5.0, 4.0 },
                new List<double> { 1.0, 3.0 }
            };
            var inferringSequences = new List<List<double>>
            {
                new List<double> { 2.0, 3.0 },
                new List<double> { 6.0, 4.0,}
            };
            var analyzer = new SequenceAnalyzer(trainingSequences, inferringSequences);

            // Act
            var difference = analyzer.FindDifferenceBetweenTwoMinValues();

            // Assert
            Assert.AreEqual(1.0, difference);
        }

        [TestMethod]
        public void Test_FindMaxValue()
        {
            // Arrange
            var trainingSequences = new List<List<double>>
            {
                new List<double> { -1.5, 2.3, 4.7 },
                new List<double> { 0.0, -3.2, 5.1 }
            };
            var inferringSequences = new List<List<double>>
            {
                new List<double> { -2.0, 3.8, 1.2 },
                new List<double> { 6.0, -4.5, 0.1 }
            };
            var analyzer = new SequenceAnalyzer(trainingSequences, inferringSequences);

            // Act
            var maxValue = analyzer.FindMaxValue();

            // Assert
            Assert.AreEqual(6.0, maxValue);
        }
    }
}
