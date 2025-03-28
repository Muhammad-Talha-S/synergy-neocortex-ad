using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AnomalyDetectionTeamSynergy;

namespace TestingAnomalyDetectionTeamSynergy
{
    /// <summary>
    /// Tests for CSVToHTMInput class that converts sequences to HTM-compatible format
    /// </summary>
    [TestClass]
    public class CSVToHTMInputTests
    {
        /// <summary>
        /// Tests BuildHTMInput with valid sequence data
        /// Verifies correct dictionary creation with sequence numbering
        /// </summary>
        [TestMethod]
        public void Test_BuildHTMInput_ValidInput()
        {
            // Arrange
            var csvToHTMInput = new CSVToHTMInput();
            var sequences = new List<List<double>>
            {
                new List<double> { 1.0, 2.0, 3.0 },
                new List<double> { 4.0, 5.0, 6.0 },
                new List<double> { 7.0, 8.0, 9.0 }
            };

            // Act
            var result = csvToHTMInput.BuildHTMInput(sequences);

            // Assert
            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(new List<double> { 1.0, 2.0, 3.0 }, result["S1"]);
            CollectionAssert.AreEqual(new List<double> { 4.0, 5.0, 6.0 }, result["S2"]);
            CollectionAssert.AreEqual(new List<double> { 7.0, 8.0, 9.0 }, result["S3"]);
        }
    }
}
