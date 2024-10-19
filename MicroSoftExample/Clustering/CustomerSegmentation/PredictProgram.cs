using CommonLib;
using CustomerSegmentation.Model;
using Microsoft.ML;

namespace CustomerSegmentation;

public static class PredictProgram
{
    public static void PredictProgramMain()
    {
        var assetsRelativePath = @"\PredictData";
        string assetsPath = PathFind.GetAbsolutePath(assetsRelativePath);

        var pivotCsv = Path.Combine(assetsPath, "inputs", "pivot.csv");
        var modelPath = Path.Combine(assetsPath, "inputs", "retailClustering.zip");
        var plotSvg = Path.Combine(assetsPath, "outputs", "customerSegmentation.svg");
        var plotCsv = Path.Combine(assetsPath, "outputs", "customerSegmentation.csv");

        try
        {
            MLContext mlContext = new MLContext();  //Seed set to any number so you have a deterministic results

            //Create the clusters: Create data files and plot a chart
            var clusteringModelScorer = new ClusteringModelScorer(mlContext, pivotCsv, plotSvg, plotCsv);
            clusteringModelScorer.LoadModel(modelPath);

            clusteringModelScorer.CreateCustomerClusters();
        }
        catch (Exception ex)
        {
            ConsoleHelper.ConsoleWriteException(ex.ToString());
        }
        ConsoleHelper.ConsolePressAnyKey();
    }
}
