namespace MachineLearningDomain.PredictionResponses;

internal class ProductRecommendation
{
    public float PredictedRating { get; set; }   // Predicted rating the user would give to the product
    public bool Recommend { get; set; }          // Whether the product is recommended for the user
}
