using MachineLearningDomain.Base;

namespace MachineLearningDomain.Models;

public class Insurance : BaseModel
{
    public float PolicyDuration { get; set; }
    public float PremiumAmount { get; set; }
    public int NumberOfClaims { get; set; }
    public float ClaimAmount { get; set; }
    public int CustomerAge { get; set; }
    public string? PolicyType { get; set; }
    public bool IsHighRisk { get; set; }
}
