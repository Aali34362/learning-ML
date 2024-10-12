namespace MachineLearningDomain.PredictionResponses;

public class InsurancePrediction
{
    public float PredictedClaimAmount { get; set; }  // Predicted total claim amount
    public float RiskScore { get; set; }             // Predicted risk score (higher score indicates higher risk)
}
