using CommonLib;
using CustomerSegmentation.DataStructures;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace CustomerSegmentation;

public static class TrainProgram
{
    public static void TrainProgramMain()
    {
        string assetsRelativePath = @"\TrainData";
        string assetsPath = PathFind.GetAbsolutePath(assetsRelativePath);

        string transactionsCsv = Path.Combine(assetsPath, "inputs", "transactions.csv");
        string offersCsv = Path.Combine(assetsPath, "inputs", "offers.csv");
        string pivotCsv = Path.Combine(assetsPath, "inputs", "pivot.csv");
        string modelPath = Path.Combine(assetsPath, "outputs", "retailClustering.zip");

        try
        {
            //STEP 0: Special data pre-process in this sample creating the PivotTable csv file
            DataHelpers.PreProcessAndSave(offersCsv, transactionsCsv, pivotCsv);

            //Create the MLContext to share across components for deterministic results
            MLContext mlContext = new MLContext(seed: 1);  //Seed set to any number so you have a deterministic environment

            // STEP 1: Common data loading configuration
            var pivotDataView = mlContext.Data.LoadFromTextFile(path: pivotCsv,
                                        columns: new[]
                                        {
                                            new TextLoader.Column("Features", DataKind.Single, new[] {new TextLoader.Range(0, 31) }),
                                                        new TextLoader.Column(nameof(PivotData.LastName), DataKind.String, 32)
                                                    },
                                        hasHeader: true,
                                        separatorChar: ',');

            //STEP 2: Configure data transformations in pipeline
            var dataProcessPipeline = mlContext.Transforms.ProjectToPrincipalComponents(outputColumnName: "PCAFeatures", inputColumnName: "Features", rank: 2)
             .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "LastNameKey", inputColumnName: nameof(PivotData.LastName), OneHotEncodingEstimator.OutputKind.Indicator));


            // (Optional) Peek data in training DataView after applying the ProcessPipeline's transformations
            ConsoleHelper.PeekDataViewInConsole(mlContext, pivotDataView, dataProcessPipeline, 10);
            ConsoleHelper.PeekVectorColumnDataInConsole(mlContext, "Features", pivotDataView, dataProcessPipeline, 10);

            //STEP 3: Create the training pipeline
            var trainer = mlContext.Clustering.Trainers.KMeans(featureColumnName: "Features", numberOfClusters: 3);
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            //STEP 4: Train the model fitting to the pivotDataView
            Console.WriteLine("=============== Training the model ===============");
            ITransformer trainedModel = trainingPipeline.Fit(pivotDataView);

            //STEP 5: Evaluate the model and show accuracy stats
            Console.WriteLine("===== Evaluating Model's accuracy with Test data =====");
            var predictions = trainedModel.Transform(pivotDataView);
            var metrics = mlContext.Clustering.Evaluate(predictions, scoreColumnName: "Score", featureColumnName: "Features");

            ConsoleHelper.PrintClusteringMetrics(trainer.ToString()!, metrics);

            //STEP 6: Save/persist the trained model to a .ZIP file
            mlContext.Model.Save(trainedModel, pivotDataView.Schema, modelPath);

            Console.WriteLine("The model is saved to {0}", modelPath);
        }
        catch (Exception ex)
        {
            ConsoleHelper.ConsoleWriteException(ex.ToString());
        }

    }
}
