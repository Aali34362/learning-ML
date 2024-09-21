using Dumpify;
using Microsoft.ML;
using MLNetCrashCourse;

var context = new MLContext();

#region File Location
string fileName = "housing.csv";
FileInfo f = new FileInfo(fileName);
string fullName = f.FullName;
var fileDir = f.Directory!.Parent!.Parent!.Parent;
string dir = System.IO.Directory.GetCurrentDirectory();
#endregion

var data = context.Data.LoadFromTextFile<HousingData>(fileDir + "\\housing.csv", hasHeader: true, separatorChar: ',');

var split = context.Data.TrainTestSplit(data, testFraction: 0.2);

var features = split.TrainSet.Schema
    .Select(col => col.Name)
    .Where(colName => colName != "Label" && colName != "OceanProximity")
    .ToArray();

var pipeLine = context.Transforms.Text.FeaturizeText("Text", "OceanProximity")
    .Append(context.Transforms.Concatenate("Features", features))
    .Append(context.Transforms.Concatenate("Feature", "Features", "Text"))
    .Append(context.Regression.Trainers.LbfgsPoissonRegression());

var model = pipeLine.Fit(split.TrainSet);

var predictions = model.Transform(split.TestSet);

var metrics = context.Regression.Evaluate(predictions);

$"R^2 - {metrics.RSquared}".Dump();

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
