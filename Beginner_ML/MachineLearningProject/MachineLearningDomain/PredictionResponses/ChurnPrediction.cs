namespace MachineLearningDomain.PredictionResponses;

public class ChurnPrediction
{
    public bool WillChurn { get; set; }    // Whether the customer is predicted to churn (leave the service)
    public float Probability { get; set; } // Confidence level in the prediction
}
