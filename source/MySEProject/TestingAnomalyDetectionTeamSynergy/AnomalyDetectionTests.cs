using AnomalyDetectionTeamSynergy;
namespace TestingAnomalyDetectionTeamSynergy
{
    /// <summary>
    /// Unit tests for AnomalyDetection class
    /// </summary>
    [TestClass]
    public class AnomalyDetectionTests
    {
        /// <summary>
        /// Tests anomaly detection when deviation exceeds both thresholds
        /// </summary>
        [TestMethod]
        public void Test_IsAnomaly_WithAnomaly()
        {
            double relativeThreshold = 0.1;
            double absoluteThreshold = 1.0;
            var anomalyDetection = new AnomalyDetection(relativeThreshold, absoluteThreshold);
            double predictedValue = 100;
            double actualValue = 120;

            bool result = anomalyDetection.IsAnomaly(predictedValue, actualValue);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests normal case where deviation is within thresholds
        /// </summary>
        [TestMethod]
        public void Test_IsAnomaly_WithoutAnomaly()
        {
            double relativeThreshold = 0.1;
            double absoluteThreshold = 1.0;
            var anomalyDetection = new AnomalyDetection(relativeThreshold, absoluteThreshold);
            double predictedValue = 100;
            double actualValue = 105;

            bool result = anomalyDetection.IsAnomaly(predictedValue, actualValue);

            Assert.IsFalse(result);
        }
    }
}
