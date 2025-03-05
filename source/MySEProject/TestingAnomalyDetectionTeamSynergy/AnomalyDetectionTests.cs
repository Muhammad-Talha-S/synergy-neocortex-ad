using AnomalyDetectionTeamSynergy;
namespace TestingAnomalyDetectionTeamSynergy
{
    [TestClass]
    public class AnomalyDetectionTests
    {
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
