﻿using CommonLib;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using Microsoft.ML.Vision;
using static Microsoft.ML.DataOperationsCatalog;

class Program
{
    static void Main(string[] args)
    {
        ////Environment.SetEnvironmentVariable("TF_ENABLE_ONEDNN_OPTS", "0");
        var path = PathFind.GetProjectRootPath();

        var builder = new ConfigurationBuilder()
                 .SetBasePath(path)
                 .AddJsonFile("Properties/launchSettings.json", optional: false);

        IConfiguration config = builder.Build();
        string envVar = config[" profiles:ConsoleApp:environmentVariables:TF_ENABLE_ONEDNN_OPTS"]!;
        Console.WriteLine($"Environment Variable: {envVar}");

        
        var projectDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../"));
        var workspaceRelativePath = Path.Combine(path, "workspace");
        var assetsRelativePath = Path.Combine(path, "assets");

        MLContext mlContext = new MLContext();

        // You must unzip assets.zip before training
        IEnumerable<ImageData> images = LoadImagesFromDirectory(folder: assetsRelativePath, useFolderNameAsLabel: true);

        IDataView imageData = mlContext.Data.LoadFromEnumerable(images);

        IDataView shuffledData = mlContext.Data.ShuffleRows(imageData);

        var preprocessingPipeline = mlContext.Transforms.Conversion.MapValueToKey(
                inputColumnName: "Label",
                outputColumnName: "LabelAsKey")
            .Append(mlContext.Transforms.LoadRawImageBytes(
                outputColumnName: "Image",
                imageFolder: assetsRelativePath,
                inputColumnName: "ImagePath"));

        IDataView preProcessedData = preprocessingPipeline
                            .Fit(shuffledData)
                            .Transform(shuffledData);

        TrainTestData trainSplit = mlContext.Data.TrainTestSplit(data: preProcessedData, testFraction: 0.3);
        TrainTestData validationTestSplit = mlContext.Data.TrainTestSplit(trainSplit.TestSet);

        IDataView trainSet = trainSplit.TrainSet;
        IDataView validationSet = validationTestSplit.TrainSet;
        IDataView testSet = validationTestSplit.TestSet;

        var classifierOptions = new ImageClassificationTrainer.Options()
        {
            FeatureColumnName = "Image",
            LabelColumnName = "LabelAsKey",
            ValidationSet = validationSet,
            Arch = ImageClassificationTrainer.Architecture.ResnetV2101,
            MetricsCallback = (metrics) => Console.WriteLine(metrics),
            TestOnTrainSet = false,
            ReuseTrainSetBottleneckCachedValues = true,
            ReuseValidationSetBottleneckCachedValues = true
        };

        var trainingPipeline = mlContext.MulticlassClassification.Trainers.ImageClassification(classifierOptions)
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        ITransformer trainedModel = trainingPipeline.Fit(trainSet);

        ClassifySingleImage(mlContext, testSet, trainedModel);

        ClassifyImages(mlContext, testSet, trainedModel);

        Console.ReadKey();
    }

    public static void ClassifySingleImage(MLContext mlContext, IDataView data, ITransformer trainedModel)
    {
        PredictionEngine<ModelInput, ModelOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(trainedModel);

        ModelInput image = mlContext.Data.CreateEnumerable<ModelInput>(data, reuseRowObject: true).First();

        ModelOutput prediction = predictionEngine.Predict(image);

        Console.WriteLine("Classifying single image");
        OutputPrediction(prediction);
    }

    public static void ClassifyImages(MLContext mlContext, IDataView data, ITransformer trainedModel)
    {
        IDataView predictionData = trainedModel.Transform(data);

        IEnumerable<ModelOutput> predictions = mlContext.Data.CreateEnumerable<ModelOutput>(predictionData, reuseRowObject: true).Take(10);

        Console.WriteLine("Classifying multiple images");
        foreach (var prediction in predictions)
        {
            OutputPrediction(prediction);
        }
    }

    private static void OutputPrediction(ModelOutput prediction)
    {
        string imageName = Path.GetFileName(prediction.ImagePath!);
        Console.WriteLine($"Image: {imageName} | Actual Value: {prediction.Label} | Predicted Value: {prediction.PredictedLabel}");
    }

    public static IEnumerable<ImageData> LoadImagesFromDirectory(string folder, bool useFolderNameAsLabel = true)
    {
        var files = Directory.GetFiles(folder, "*",
            searchOption: SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if ((Path.GetExtension(file) != ".jpg") && (Path.GetExtension(file) != ".png"))
                continue;

            var label = Path.GetFileName(file);

            if (useFolderNameAsLabel)
                label = Directory.GetParent(file)!.Name;
            else
            {
                for (int index = 0; index < label.Length; index++)
                {
                    if (!char.IsLetter(label[index]))
                    {
                        label = label.Substring(0, index);
                        break;
                    }
                }
            }

            yield return new ImageData()
            {
                ImagePath = file,
                Label = label
            };
        }
    }
}

class ImageData
{
    public string? ImagePath { get; set; }
    public string? Label { get; set; }
}
class ModelInput
{
    public byte[]? Image { get; set; }
    public UInt32 LabelAsKey { get; set; }
    public string? ImagePath { get; set; }
    public string? Label { get; set; }
}
class ModelOutput
{
    public string? ImagePath { get; set; }
    public string? Label { get; set; }
    public string? PredictedLabel { get; set; }
}