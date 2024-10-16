using Microsoft.ML.TimeSeries;
using Microsoft.ML;
using SrCnnEntireDetection.DataStructures;

internal static class Program
{
    private static string BaseDatasetsRelativePath = @"../../../../Data";
    private static string DatasetRelativePath = $"{BaseDatasetsRelativePath}/phone-calls.csv";

    private static string DatasetPath = GetAbsolutePath(DatasetRelativePath);

    private static MLContext mlContext;

    static void Main()
    {

        string trainData = "phone-calls.csv";
        FileInfo ftrainData = new FileInfo(trainData);
        var fileDirtrainData = $"{ftrainData.Directory!.Parent!.Parent!.Parent}/Data/phone-calls.csv";
        DatasetPath = fileDirtrainData.ToString();

        // Create MLContext to be shared across the model creation workflow objects 
        mlContext = new MLContext();

        //Load the data into IDataView.
        //This dataset is used to detect anomaly in time-series dataset.
        IDataView dataView = mlContext.Data.LoadFromTextFile<PhoneCallsData>(path: DatasetPath, hasHeader: true, separatorChar: ',');

        //To detech temporay changes in the pattern
        DetectAnomaly(dataView);

        Console.WriteLine("=============== End of process, hit any key to finish ===============");

        Console.ReadLine();
    }

    static void DetectAnomaly(IDataView dataView)
    {
        Console.WriteLine("===============Detect anomalies in pattern===============");

        //STEP 1: Specify the input column and output column names.
        string inputColumnName = nameof(PhoneCallsData.value);
        string outputColumnName = nameof(PhoneCallsPrediction.Prediction);

        //STEP 2: Detect period on the given series.
        int period = mlContext.AnomalyDetection.DetectSeasonality(dataView, inputColumnName);
        Console.WriteLine("Period of the series is: {0}.", period);

        //STEP 3: Setup the parameters
        var options = new SrCnnEntireAnomalyDetectorOptions()
        {
            Threshold = 0.3,
            Sensitivity = 64.0,
            DetectMode = SrCnnDetectMode.AnomalyAndMargin,
            Period = period,
        };

        //STEP 4: Invoke SrCnn algorithm to detect anomaly on the entire series.
        var outputDataView = mlContext.AnomalyDetection.DetectEntireAnomalyBySrCnn(dataView, outputColumnName, inputColumnName, options);

        //STEP 5: Get the detection results as an IEnumerable
        var predictions = mlContext.Data.CreateEnumerable<PhoneCallsPrediction>(
            outputDataView, reuseRowObject: false);

        Console.WriteLine("The anomaly detection results obtained.");
        var index = 0;

        Console.WriteLine("Index\tData\tAnomaly\tAnomalyScore\tMag\tExpectedValue\tBoundaryUnit\tUpperBoundary\tLowerBoundary");
        foreach (var p in predictions)
        {
            if (p.Prediction[0] == 1)
            {
                Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}  <-- alert is on, detecte anomaly", index,
                    p.Prediction[0], p.Prediction[1], p.Prediction[2], p.Prediction[3], p.Prediction[4], p.Prediction[5], p.Prediction[6]);
            }
            else
            {
                Console.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", index,
                    p.Prediction[0], p.Prediction[1], p.Prediction[2], p.Prediction[3], p.Prediction[4], p.Prediction[5], p.Prediction[6]);
            }
            ++index;

        }

        Console.WriteLine("");
    }

    public static string GetAbsolutePath(string relativePath)
    {
        FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
        string assemblyFolderPath = _dataRoot.Directory!.FullName;

        string fullPath = Path.Combine(assemblyFolderPath, relativePath);

        return fullPath;
    }
}