namespace MachineLearningDomain.TrainingModels;

public class LoanData
{
    public float LoanAmount { get; set; }       // Amount of the loan taken by the customer
    public int LoanDuration { get; set; }       // Loan duration in months
    public float InterestRate { get; set; }     // Interest rate on the loan
    public int CustomerCreditScore { get; set; } // Credit score of the customer
    public bool HasCollateral { get; set; }     // Whether the loan is backed by collateral
}
