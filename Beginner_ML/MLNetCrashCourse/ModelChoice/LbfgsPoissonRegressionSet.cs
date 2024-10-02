using Dumpify;
using Microsoft.ML;
using MLNetCrashCourse.Model;

namespace MLNetCrashCourse.ModelChoice;

public static class LbfgsPoissonRegressionSet
{
    public static void LbfgsPoissonRegressionMain()
    {
        //MLContext Initialization:
        //This initializes the machine learning context, providing access to data loading, processing, and model training APIs.        
        var context = new MLContext();

        #region File Location
        string fileName = "housing.csv";
        FileInfo f = new FileInfo(fileName);
        string fullName = f.FullName;
        var fileDir = f.Directory!.Parent!.Parent!.Parent;
        string dir = System.IO.Directory.GetCurrentDirectory();
        //string rootPath = AppDomain.CurrentDomain.BaseDirectory;
        #endregion

        var data = context.Data.LoadFromTextFile<HousingData>(fileDir + "\\housing.csv", hasHeader: true, separatorChar: ',');

        //Train-Test Split:
        //The data is split into training and testing sets, with 20% allocated for testing.
        var split = context.Data.TrainTestSplit(data, testFraction: 0.2);
        var features = split.TrainSet.Schema
            .Select(col => col.Name)
            .Where(colName => colName != "Label" && colName != "OceanProximity")
            .ToArray();

        // Define the pipeline -> Pipeline Construction:
        //Text Featurization:
        //converts the categorical text data (e.g., "OceanProximity") into numerical features.
        //Concatenation:
        //It combines all feature columns (both numerical and text-derived ones) into a single "Features" column for the model to use.
        //Model:
        //You're using LbfgsPoissonRegression() as the regression algorithm.
        var pipeLine = context.Transforms.Text.FeaturizeText("Text", "OceanProximity")
            .Append(context.Transforms.Concatenate("Features", features))
            .Append(context.Transforms.Concatenate("Feature", "Features", "Text"))
            .Append(context.Regression.Trainers.LbfgsPoissonRegression());

        //Model Training and Evaluation:
        //trains the model on the training set.
        var model = pipeLine.Fit(split.TrainSet);

        // Evaluate the model
        var predictions = model.Transform(split.TestSet);
        //evaluates the model on the test set and outputs the R2 score to check the model's performance.
        var metrics = context.Regression.Evaluate(predictions);

        $"LbfgsPoissonRegression -> R^2 - {metrics.RSquared}".Dump();

        //Load sample data
        var sampleData = new MLModel1.ModelInput()
        {
            Longitude = -122.22F,
            Latitude = 37.86F,
            Housing_median_age = 21F,
            Total_rooms = 7099F,
            Total_bedrooms = 1106F,
            Population = 2401F,
            Households = 1138F,
            Median_income = 8.3014F,
            Ocean_proximity = @"NEAR BAY",
        };

        //Load model and predict output
        var result = MLModel1.Predict(sampleData);
        $"Predict - Features : {result.Features.Count()}, Longitude:{result.Longitude}, Latitude:{result.Latitude}, Ocean_proximity:{result.Ocean_proximity.Count()} Median_income:{result.Median_income}".Dump();
    }
}
