using MachineLearningDomain.Base;

namespace MachineLearningDomain.Responses;

public class UserProductResponse : BaseResponse
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public float ProductRating { get; set; }
    public int ProductCategory { get; set; }
}
