using Microsoft.ML.Data;

namespace MLNetCrashCourse.Response;

public class ResultModel
{
    [ColumnName("Score")]
    public float Salary { get; set; }
}
