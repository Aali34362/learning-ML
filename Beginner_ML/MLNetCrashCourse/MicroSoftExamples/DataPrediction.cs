using Microsoft.ML;
using MLNetCrashCourse.Model;

namespace MLNetCrashCourse.MicroSoftExamples;

public static class DataPrediction
{
    public static void DataPredictionMain()
    {
        #region File Location
        string fileName = "data.csv";
        FileInfo f = new FileInfo(fileName);
        string fullName = f.FullName;
        var fileDir = f.Directory!.Parent!.Parent!.Parent;
        string dir = System.IO.Directory.GetCurrentDirectory();
        #endregion

        var mlContext = new MLContext();

        // 1. Load Data
        IDataView data = mlContext.Data.LoadFromTextFile<ModelInput>(fileDir + "\\data.csv", hasHeader: true, separatorChar: ',');

        // 2. Split Data
        var splitData = mlContext.Data.TrainTestSplit(data, testFraction: 0.2);

        // 3. Define Data Processing Pipeline
        var dataProcessPipeline = mlContext.Transforms.Text.FeaturizeText("Features", "TextColumn")
                                .Append(mlContext.Transforms.Concatenate("Features", "Features"));

        // 4. Train Model
        var trainer = mlContext.BinaryClassification.Trainers.SdcaLogisticRegression();
        var trainingPipeline = dataProcessPipeline.Append(trainer);
        var trainedModel = trainingPipeline.Fit(splitData.TrainSet);

        // 5. Evaluate Model
        var predictions = trainedModel.Transform(splitData.TestSet);
        var metrics = mlContext.BinaryClassification.Evaluate(predictions);
        Console.WriteLine($"Accuracy: {metrics.Accuracy}");

        // 6. Save Model
        mlContext.Model.Save(trainedModel, splitData.TrainSet.Schema, fileDir + "\\model.zip");
    }
}
