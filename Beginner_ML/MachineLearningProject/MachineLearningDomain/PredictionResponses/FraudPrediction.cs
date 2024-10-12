namespace MachineLearningDomain.PredictionResponses;

public class FraudPrediction
{
    public bool IsFraudulent { get; set; }     // Whether the transaction is predicted to be fraudulent
    public float FraudProbability { get; set; } // Confidence level in the prediction
}
