using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AnomalyDetectionTeamSynergy;

namespace TestingAnomalyDetectionTeamSynergy
{
    [TestClass]
    public class CSVToHTMInputTests
    {
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
            Assert.AreEqual(3, result.Count); // Use Assert.AreEqual for MSTest
            CollectionAssert.AreEqual(new List<double> { 1.0, 2.0, 3.0 }, result["S1"]); // Use CollectionAssert for comparing lists
            CollectionAssert.AreEqual(new List<double> { 4.0, 5.0, 6.0 }, result["S2"]);
            CollectionAssert.AreEqual(new List<double> { 7.0, 8.0, 9.0 }, result["S3"]);
        }
    }
}
