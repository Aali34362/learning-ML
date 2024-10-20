using Microsoft.ML.Data;

namespace ImageClassification_WebApp.ImageClassification.Shared.DataModels;

public class ImagePrediction
{
    [ColumnName("Score")]
    public float[]? Score;
    [ColumnName("PredictedLabel")]
    public string? PredictedLabel;
}
