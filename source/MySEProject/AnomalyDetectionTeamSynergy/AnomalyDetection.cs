namespace AnomalyDetectionTeamSynergy
{
    public class AnomalyDetection
    {
        private double _threshold;

        public AnomalyDetection(double threshold = 0.8)
        {
            _threshold = threshold;
        }

        // Method to calculate anomaly based on similarity threshold
        public bool IsAnomaly(double predictedValue, double actualValue)
        {
            return Math.Abs(predictedValue - actualValue) > _threshold;
        }

    }
}
