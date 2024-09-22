using Microsoft.ML.Data;

namespace MLNetCrashCourse.Model;

public class ModelInput
{
    // Text column representing the input text for classification
    [LoadColumn(0)] // First column in CSV
    public string? TextColumn { get; set; }

    // Label column representing the classification outcome (1 for positive, 0 for negative)
    [LoadColumn(1)] // Second column in CSV
    public bool Label { get; set; }
}
