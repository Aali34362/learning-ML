namespace MachineLearningDomain.TrainingModels;

public class TransactionData
{
    public float TransactionAmount { get; set; }  // The amount involved in the transaction
    public string? MerchantCategory { get; set; }  // Category of the merchant (e.g., retail, food)
    public string? TransactionLocation { get; set; } // The location of the transaction
    public bool IsOnlineTransaction { get; set; }   // Whether the transaction was made online
    public int CustomerAge { get; set; }            // Age of the customer making the transaction
}
