namespace MachineLearningDomain.TrainingModels;

public class UserProductData
{
    public int UserId { get; set; }            // Unique ID of the user
    public int ProductId { get; set; }         // Unique ID of the product
    public float ProductRating { get; set; }   // Rating the user gave to the product
    public int ProductCategory { get; set; }   // Category of the product
}
