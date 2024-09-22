using Dumpify;
using Microsoft.ML;
using MLNetCrashCourse.Model;

namespace MLNetCrashCourse.ModelChoice;

public static class SdcaSet
{
    public static void SdcaSetMain()
    {
        var context = new MLContext();

        #region File Location
        string fileName = "housing.csv";
        FileInfo f = new FileInfo(fileName);
        string fullName = f.FullName;
        var fileDir = f.Directory!.Parent!.Parent!.Parent;
        #endregion

        var data = context.Data.LoadFromTextFile<HousingData>(fileDir + "\\housing.csv", hasHeader: true, separatorChar: ',');

        var split = context.Data.TrainTestSplit(data, testFraction: 0.2);

        var features = split.TrainSet.Schema
            .Select(col => col.Name)
            .Where(colName => colName != "Label" && colName != "OceanProximity")
            .ToArray();

        // Define the pipeline
        var pipeLine = context.Transforms.Text.FeaturizeText("Text", "OceanProximity")
            .Append(context.Transforms.Concatenate("Features", features))
            .Append(context.Transforms.Concatenate("Feature", "Features", "Text"))
            .Append(context.Regression.Trainers.Sdca()); // Try SDCA regression as well

        var model = pipeLine.Fit(split.TrainSet);

        // Evaluate the model
        var predictions = model.Transform(split.TestSet);
        var metrics = context.Regression.Evaluate(predictions);

        $"Sdca -> R^2 - {metrics.RSquared}".Dump();

    }
}
/*
Suggestions:
Feature Selection:
You are manually excluding "Label" and "OceanProximity" columns. 
Consider automating this by detecting all non-numeric columns for exclusion.

Model Choice:
You're using LbfgsPoissonRegression(), which is good for count-based data. 
For continuous target values, you might also try Sdca() (Stochastic Dual Coordinate Ascent) regression for potentially better results.

Enhancing Pipeline:
You might want to add normalization or scaling of features for better performance depending on the dataset.
 */