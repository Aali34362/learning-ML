namespace MachineLearningDomain.PredictionResponses;

public class LoanPrediction
{
    public bool WillDefault { get; set; }    // Whether the customer is predicted to default on the loan
    public float RiskScore { get; set; }     // Risk score of loan default
}
