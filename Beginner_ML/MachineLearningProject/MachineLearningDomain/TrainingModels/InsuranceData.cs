namespace MachineLearningDomain.TrainingModels;

public class InsuranceData
{
    public float PolicyDuration { get; set; } // Duration of the insurance policy in years
    public float PremiumAmount { get; set; }  // The premium amount the customer pays
    public int NumberOfClaims { get; set; }   // Number of claims made by the customer
    public float ClaimAmount { get; set; }    // Total claim amount paid out
    public int CustomerAge { get; set; }      // Age of the customer
    public string? PolicyType { get; set; }   // Type of the insurance policy (e.g., "Auto", "Home", "Life")
    public bool IsHighRisk { get; set; }      // Whether the policyholder is classified as high risk
}
