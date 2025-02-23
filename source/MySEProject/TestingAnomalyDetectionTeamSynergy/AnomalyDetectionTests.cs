using AnomalyDetectionTeamSynergy;
namespace TestingAnomalyDetectionTeamSynergy
{
    [TestClass]
    public class AnomalyDetectionTests
    {
        [TestMethod]
        public void Test_IsAnomaly_WithAnomaly()
        {
            var anomalyDetection = new AnomalyDetection();
            double predictedValue = 100;
            double actualValue = 120;
            double absoluteThreshold = 1;
            double relativeThreshold = 0.1;
            bool result = anomalyDetection.IsAnomaly(predictedValue, actualValue, absoluteThreshold, relativeThreshold);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_IsAnomaly_WithoutAnomaly()
        {
            var anomalyDetection = new AnomalyDetection();
            double predictedValue = 100;
            double actualValue = 105;
            double absoluteThreshold = 1;
            double relativeThreshold = 0.1;
            bool result = anomalyDetection.IsAnomaly(predictedValue, actualValue, absoluteThreshold, relativeThreshold);

            Assert.IsFalse(result);
        }
    }
}
