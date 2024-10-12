using MachineLearningDomain.Base;

namespace MachineLearningDomain.Models;

public class Transaction : BaseModel
{
    public float TransactionAmount { get; set; }
    public string? MerchantCategory { get; set; }
    public string? TransactionLocation { get; set; }
    public bool IsOnlineTransaction { get; set; }
    public int CustomerAge { get; set; }
}
