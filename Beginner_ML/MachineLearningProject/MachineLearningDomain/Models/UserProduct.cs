using MachineLearningDomain.Base;

namespace MachineLearningDomain.Models;

public class UserProduct : BaseModel
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public float ProductRating { get; set; }
    public int ProductCategory { get; set; }
}
