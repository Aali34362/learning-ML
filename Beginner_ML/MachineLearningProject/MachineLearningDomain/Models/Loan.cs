using MachineLearningDomain.Base;

namespace MachineLearningDomain.Models;

public class Loan : BaseModel
{
    public float LoanAmount { get; set; }
    public int LoanDuration { get; set; }
    public float InterestRate { get; set; }
    public int CustomerCreditScore { get; set; }
    public bool HasCollateral { get; set; }
}
