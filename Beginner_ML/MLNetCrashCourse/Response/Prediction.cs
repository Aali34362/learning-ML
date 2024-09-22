using Microsoft.ML.Data;

namespace MLNetCrashCourse.Response;

public class Prediction
{
    [ColumnName("Score")]
    public float Price { get; set; }
}
