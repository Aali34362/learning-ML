using Microsoft.ML.Data;

namespace DeepLearning_ObjectDetection_Onnx.DataStructures;

public class ImageNetPrediction
{
    [ColumnName("grid")]
    public float[]? PredictedLabels;
}
