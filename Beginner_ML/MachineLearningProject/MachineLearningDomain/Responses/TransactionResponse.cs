using MachineLearningDomain.Base;

namespace MachineLearningDomain.Responses;

public class TransactionResponse : BaseResponse
{
    public float TransactionAmount { get; set; }
    public string? MerchantCategory { get; set; }
    public string? TransactionLocation { get; set; }
    public bool IsOnlineTransaction { get; set; }
    public int CustomerAge { get; set; }
}
