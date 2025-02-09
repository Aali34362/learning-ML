﻿using Microsoft.ML;
using Microsoft.ML.Data;
using System.Diagnostics;
using System.Text;
using static Microsoft.ML.TrainCatalogBase;

namespace CommonLib;

public static partial class ConsoleHelper
{

    private const int Width = 114;

    public static void PrintRegressionMetrics(string name, RegressionMetrics metrics)
    {
        Console.WriteLine($"*************************************************");
        Console.WriteLine($"*       Metrics for {name} regression model      ");
        Console.WriteLine($"*------------------------------------------------");
        Console.WriteLine($"*       LossFn:        {metrics.LossFunction:0.##}");
        Console.WriteLine($"*       R2 Score:      {metrics.RSquared:0.##}");
        Console.WriteLine($"*       Absolute loss: {metrics.MeanAbsoluteError:#.##}");
        Console.WriteLine($"*       Squared loss:  {metrics.MeanSquaredError:#.##}");
        Console.WriteLine($"*       RMS loss:      {metrics.RootMeanSquaredError:#.##}");
        Console.WriteLine($"*************************************************");
    }

    public static void PrintBinaryClassificationMetrics(string name, BinaryClassificationMetrics metrics)
    {
        Console.WriteLine($"************************************************************");
        Console.WriteLine($"*       Metrics for {name} binary classification model      ");
        Console.WriteLine($"*-----------------------------------------------------------");
        Console.WriteLine($"*       Accuracy: {metrics.Accuracy:P2}");
        Console.WriteLine($"*       Area Under Curve:      {metrics.AreaUnderRocCurve:P2}");
        Console.WriteLine($"*       Area under Precision recall Curve:  {metrics.AreaUnderPrecisionRecallCurve:P2}");
        Console.WriteLine($"*       F1Score:  {metrics.F1Score:P2}");
        Console.WriteLine($"*       PositivePrecision:  {metrics.PositivePrecision:#.##}");
        Console.WriteLine($"*       PositiveRecall:  {metrics.PositiveRecall:#.##}");
        Console.WriteLine($"*       NegativePrecision:  {metrics.NegativePrecision:#.##}");
        Console.WriteLine($"*       NegativeRecall:  {metrics.NegativeRecall:P2}");
        Console.WriteLine($"************************************************************");
    }

    public static void PrintMulticlassClassificationMetrics(string name, MulticlassClassificationMetrics metrics)
    {
        Console.WriteLine($"************************************************************");
        Console.WriteLine($"*    Metrics for {name} multi-class classification model    ");
        Console.WriteLine($"*-----------------------------------------------------------");
        Console.WriteLine($"    MacroAccuracy = {metrics.MacroAccuracy:0.####}, a value from 0 and 1, where closer to 1.0 is better");
        Console.WriteLine($"    MicroAccuracy = {metrics.MicroAccuracy:0.####}, a value from 0 and 1, where closer to 1.0 is better");
        Console.WriteLine($"    LogLoss = {metrics.LogLoss:0.####}, the closer to 0, the better");
        Console.WriteLine($"    LogLoss for class 1 = {metrics.PerClassLogLoss[0]:0.####}, the closer to 0, the better");
        Console.WriteLine($"    LogLoss for class 2 = {metrics.PerClassLogLoss[1]:0.####}, the closer to 0, the better");
        Console.WriteLine($"    LogLoss for class 3 = {metrics.PerClassLogLoss[2]:0.####}, the closer to 0, the better");
        Console.WriteLine($"************************************************************");
    }

    public static void PrintRankingMetrics(string name, RankingMetrics metrics, uint optimizationMetricTruncationLevel)
    {
        Console.WriteLine($"************************************************************");
        Console.WriteLine($"*             Metrics for {name} ranking model              ");
        Console.WriteLine($"*-----------------------------------------------------------");
        Console.WriteLine($"    Normalized Discounted Cumulative Gain (NDCG@{optimizationMetricTruncationLevel}) = {metrics?.NormalizedDiscountedCumulativeGains?[(int)optimizationMetricTruncationLevel - 1] ?? double.NaN:0.####}, a value from 0 and 1, where closer to 1.0 is better");
        Console.WriteLine($"    Discounted Cumulative Gain (DCG@{optimizationMetricTruncationLevel}) = {metrics?.DiscountedCumulativeGains?[(int)optimizationMetricTruncationLevel - 1] ?? double.NaN:0.####}");
    }

    public static void ShowDataViewInConsole(MLContext mlContext, IDataView dataView, int numberOfRows = 4)
    {
        string msg = string.Format("Show data in DataView: Showing {0} rows with the columns", numberOfRows.ToString());
        ConsoleWriteHeader(msg);

        var preViewTransformedData = dataView.Preview(maxRows: numberOfRows);

        foreach (var row in preViewTransformedData.RowView)
        {
            var ColumnCollection = row.Values;
            string lineToPrint = "Row--> ";
            foreach (KeyValuePair<string, object> column in ColumnCollection)
            {
                lineToPrint += $"| {column.Key}:{column.Value}";
            }
            Console.WriteLine(lineToPrint + "\n");
        }
    }

    public static void PrintIterationMetrics(int iteration, string trainerName, BinaryClassificationMetrics metrics, double? runtimeInSeconds)
    {
        CreateRow($"{iteration,-4} {trainerName,-35} {metrics?.Accuracy ?? double.NaN,9:F4} {metrics?.AreaUnderRocCurve ?? double.NaN,8:F4} {metrics?.AreaUnderPrecisionRecallCurve ?? double.NaN,8:F4} {metrics?.F1Score ?? double.NaN,9:F4} {runtimeInSeconds.Value,9:F1}", Width);
    }

    public static void PrintIterationMetrics(int iteration, string trainerName, MulticlassClassificationMetrics metrics, double? runtimeInSeconds)
    {
        CreateRow($"{iteration,-4} {trainerName,-35} {metrics?.MicroAccuracy ?? double.NaN,14:F4} {metrics?.MacroAccuracy ?? double.NaN,14:F4} {runtimeInSeconds.Value,9:F1}", Width);
    }

    public static void PrintIterationMetrics(int iteration, string trainerName, RegressionMetrics metrics, double? runtimeInSeconds)
    {
        CreateRow($"{iteration,-4} {trainerName,-35} {metrics?.RSquared ?? double.NaN,8:F4} {metrics?.MeanAbsoluteError ?? double.NaN,13:F2} {metrics?.MeanSquaredError ?? double.NaN,12:F2} {metrics?.RootMeanSquaredError ?? double.NaN,8:F2} {runtimeInSeconds.Value,9:F1}", Width);
    }

    public static void PrintIterationMetrics(int iteration, string trainerName, RankingMetrics metrics, double? runtimeInSeconds)
    {
        CreateRow($"{iteration,-4} {trainerName,-15} {metrics?.NormalizedDiscountedCumulativeGains[0] ?? double.NaN,9:F4} {metrics?.NormalizedDiscountedCumulativeGains[2] ?? double.NaN,9:F4} {metrics?.NormalizedDiscountedCumulativeGains[9] ?? double.NaN,9:F4} {metrics?.DiscountedCumulativeGains[9] ?? double.NaN,9:F4} {runtimeInSeconds.Value,9:F1}", Width);
    }

    public static void PrintIterationException(Exception ex)
    {
        Console.WriteLine($"Exception during AutoML iteration: {ex}");
    }

    public static void PrintBinaryClassificationMetricsHeader()
    {
        CreateRow($"{"",-4} {"Trainer",-35} {"Accuracy",9} {"AUC",8} {"AUPRC",8} {"F1-score",9} {"Duration",9}", Width);
    }

    public static void PrintMulticlassClassificationMetricsHeader()
    {
        CreateRow($"{"",-4} {"Trainer",-35} {"MicroAccuracy",14} {"MacroAccuracy",14} {"Duration",9}", Width);
    }

    public static void PrintRegressionMetricsHeader()
    {
        CreateRow($"{"",-4} {"Trainer",-35} {"RSquared",8} {"Absolute-loss",13} {"Squared-loss",12} {"RMS-loss",8} {"Duration",9}", Width);
    }

    public static void PrintRankingMetricsHeader()
    {
        CreateRow($"{"",-4} {"Trainer",-15} {"NDCG@1",9} {"NDCG@3",9} {"NDCG@10",9} {"DCG@10",9} {"Duration",9}", Width);
    }

    private static void CreateRow(string message, int width)
    {
        Console.WriteLine("|" + message.PadRight(width - 2) + "|");
    }

    public static void ConsoleWriteHeader(params string[] lines)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" ");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
        var maxLength = lines.Select(x => x.Length).Max();
        Console.WriteLine(new string('#', maxLength));
        Console.ForegroundColor = defaultColor;
    }

    //public static void Print(ColumnInferenceResults results)
    //{
    //    Console.WriteLine("Inferred dataset columns --");
    //    new ColumnInferencePrinter(results).Print();
    //    Console.WriteLine();
    //}

    public static string BuildStringTable(IList<string[]> arrValues)
    {
        int[] maxColumnsWidth = GetMaxColumnsWidth(arrValues);
        var headerSpliter = new string('-', maxColumnsWidth.Sum(i => i + 3) - 1);

        var sb = new StringBuilder();
        for (int rowIndex = 0; rowIndex < arrValues.Count; rowIndex++)
        {
            if (rowIndex == 0)
            {
                sb.AppendFormat("  {0} ", headerSpliter);
                sb.AppendLine();
            }

            for (int colIndex = 0; colIndex < arrValues[0].Length; colIndex++)
            {
                // Print cell
                string cell = arrValues[rowIndex][colIndex];
                cell = cell.PadRight(maxColumnsWidth[colIndex]);
                sb.Append(" | ");
                sb.Append(cell);
            }

            // Print end of line
            sb.Append(" | ");
            sb.AppendLine();

            // Print splitter
            if (rowIndex == 0)
            {
                sb.AppendFormat(" |{0}| ", headerSpliter);
                sb.AppendLine();
            }

            if (rowIndex == arrValues.Count - 1)
            {
                sb.AppendFormat("  {0} ", headerSpliter);
            }
        }

        return sb.ToString();
    }

    private static int[] GetMaxColumnsWidth(IList<string[]> arrValues)
    {
        var maxColumnsWidth = new int[arrValues[0].Length];
        for (int colIndex = 0; colIndex < arrValues[0].Length; colIndex++)
        {
            for (int rowIndex = 0; rowIndex < arrValues.Count; rowIndex++)
            {
                int newLength = arrValues[rowIndex][colIndex].Length;
                int oldLength = maxColumnsWidth[colIndex];

                if (newLength > oldLength)
                {
                    maxColumnsWidth[colIndex] = newLength;
                }
            }
        }

        return maxColumnsWidth;
    }

    //class ColumnInferencePrinter
    //{
    //    private static readonly string[] TableHeaders = new[] { "Name", "Data Type", "Purpose" };

    //    private readonly ColumnInferenceResults _results;

    //    public ColumnInferencePrinter(ColumnInferenceResults results)
    //    {
    //        _results = results;
    //    }

    //    public void Print()
    //    {
    //        var tableRows = new List<string[]>();

    //        // Add headers
    //        tableRows.Add(TableHeaders);

    //        // Add column data
    //        var info = _results.ColumnInformation;
    //        AppendTableRow(tableRows, info.LabelColumnName, "Label");
    //        AppendTableRow(tableRows, info.ExampleWeightColumnName, "Weight");
    //        AppendTableRow(tableRows, info.SamplingKeyColumnName, "Sampling Key");
    //        AppendTableRows(tableRows, info.CategoricalColumnNames, "Categorical");
    //        AppendTableRows(tableRows, info.NumericColumnNames, "Numeric");
    //        AppendTableRows(tableRows, info.TextColumnNames, "Text");
    //        AppendTableRows(tableRows, info.IgnoredColumnNames, "Ignored");

    //        Console.WriteLine(ConsoleHelper.BuildStringTable(tableRows));
    //    }

    //    private void AppendTableRow(ICollection<string[]> tableRows,
    //        string columnName, string columnPurpose)
    //    {
    //        if (columnName == null)
    //        {
    //            return;
    //        }

    //        tableRows.Add(new[]
    //        {
    //            columnName,
    //            GetColumnDataType(columnName),
    //            columnPurpose
    //        });
    //    }

    //    private void AppendTableRows(ICollection<string[]> tableRows,
    //        IEnumerable<string> columnNames, string columnPurpose)
    //    {
    //        foreach (var columnName in columnNames)
    //        {
    //            AppendTableRow(tableRows, columnName, columnPurpose);
    //        }
    //    }

    //    private string GetColumnDataType(string columnName)
    //    {
    //        return _results.TextLoaderOptions.Columns.First(c => c.Name == columnName).DataKind.ToString();
    //    }
    //}

    public static void PrintPrediction(string prediction)
    {
        Console.WriteLine($"*************************************************");
        Console.WriteLine($"Predicted : {prediction}");
        Console.WriteLine($"*************************************************");
    }

    public static void PrintRegressionPredictionVersusObserved(string predictionCount, string observedCount)
    {
        Console.WriteLine($"-------------------------------------------------");
        Console.WriteLine($"Predicted : {predictionCount}");
        Console.WriteLine($"Actual:     {observedCount}");
        Console.WriteLine($"-------------------------------------------------");
    }


    public static void PrintBinaryClassificationMetrics(string name, CalibratedBinaryClassificationMetrics metrics)
    {
        Console.WriteLine($"************************************************************");
        Console.WriteLine($"*       Metrics for {name} binary classification model      ");
        Console.WriteLine($"*-----------------------------------------------------------");
        Console.WriteLine($"*       Accuracy: {metrics.Accuracy:P2}");
        Console.WriteLine($"*       Area Under Curve:      {metrics.AreaUnderRocCurve:P2}");
        Console.WriteLine($"*       Area under Precision recall Curve:  {metrics.AreaUnderPrecisionRecallCurve:P2}");
        Console.WriteLine($"*       F1Score:  {metrics.F1Score:P2}");
        Console.WriteLine($"*       LogLoss:  {metrics.LogLoss:#.##}");
        Console.WriteLine($"*       LogLossReduction:  {metrics.LogLossReduction:#.##}");
        Console.WriteLine($"*       PositivePrecision:  {metrics.PositivePrecision:#.##}");
        Console.WriteLine($"*       PositiveRecall:  {metrics.PositiveRecall:#.##}");
        Console.WriteLine($"*       NegativePrecision:  {metrics.NegativePrecision:#.##}");
        Console.WriteLine($"*       NegativeRecall:  {metrics.NegativeRecall:P2}");
        Console.WriteLine($"************************************************************");
    }

    public static void PrintAnomalyDetectionMetrics(string name, AnomalyDetectionMetrics metrics)
    {
        Console.WriteLine($"************************************************************");
        Console.WriteLine($"*       Metrics for {name} anomaly detection model      ");
        Console.WriteLine($"*-----------------------------------------------------------");
        Console.WriteLine($"*       Area Under ROC Curve:                       {metrics.AreaUnderRocCurve:P2}");
        Console.WriteLine($"*       Detection rate at false positive count: {metrics.DetectionRateAtFalsePositiveCount}");
        Console.WriteLine($"************************************************************");
    }

    public static void PrintMultiClassClassificationMetrics(string name, MulticlassClassificationMetrics metrics)
    {
        Console.WriteLine($"************************************************************");
        Console.WriteLine($"*    Metrics for {name} multi-class classification model   ");
        Console.WriteLine($"*-----------------------------------------------------------");
        Console.WriteLine($"    AccuracyMacro = {metrics.MacroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
        Console.WriteLine($"    AccuracyMicro = {metrics.MicroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
        Console.WriteLine($"    LogLoss = {metrics.LogLoss:0.####}, the closer to 0, the better");
        Console.WriteLine($"    LogLoss for class 1 = {metrics.PerClassLogLoss[0]:0.####}, the closer to 0, the better");
        Console.WriteLine($"    LogLoss for class 2 = {metrics.PerClassLogLoss[1]:0.####}, the closer to 0, the better");
        Console.WriteLine($"    LogLoss for class 3 = {metrics.PerClassLogLoss[2]:0.####}, the closer to 0, the better");
        Console.WriteLine($"************************************************************");
    }

    public static void PrintRegressionFoldsAverageMetrics(string algorithmName, IReadOnlyList<CrossValidationResult<RegressionMetrics>> crossValidationResults)
    {
        var L1 = crossValidationResults.Select(r => r.Metrics.MeanAbsoluteError);
        var L2 = crossValidationResults.Select(r => r.Metrics.MeanSquaredError);
        var RMS = crossValidationResults.Select(r => r.Metrics.RootMeanSquaredError);
        var lossFunction = crossValidationResults.Select(r => r.Metrics.LossFunction);
        var R2 = crossValidationResults.Select(r => r.Metrics.RSquared);

        Console.WriteLine($"*************************************************************************************************************");
        Console.WriteLine($"*       Metrics for {algorithmName} Regression model      ");
        Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
        Console.WriteLine($"*       Average L1 Loss:    {L1.Average():0.###} ");
        Console.WriteLine($"*       Average L2 Loss:    {L2.Average():0.###}  ");
        Console.WriteLine($"*       Average RMS:          {RMS.Average():0.###}  ");
        Console.WriteLine($"*       Average Loss Function: {lossFunction.Average():0.###}  ");
        Console.WriteLine($"*       Average R-squared: {R2.Average():0.###}  ");
        Console.WriteLine($"*************************************************************************************************************");
    }

    public static void PrintMulticlassClassificationFoldsAverageMetrics(
                                     string algorithmName,
                                   IReadOnlyList<CrossValidationResult<MulticlassClassificationMetrics>> crossValResults
                                                                       )
    {
        var metricsInMultipleFolds = crossValResults.Select(r => r.Metrics);

        var microAccuracyValues = metricsInMultipleFolds.Select(m => m.MicroAccuracy);
        var microAccuracyAverage = microAccuracyValues.Average();
        var microAccuraciesStdDeviation = CalculateStandardDeviation(microAccuracyValues);
        var microAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(microAccuracyValues);

        var macroAccuracyValues = metricsInMultipleFolds.Select(m => m.MacroAccuracy);
        var macroAccuracyAverage = macroAccuracyValues.Average();
        var macroAccuraciesStdDeviation = CalculateStandardDeviation(macroAccuracyValues);
        var macroAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(macroAccuracyValues);

        var logLossValues = metricsInMultipleFolds.Select(m => m.LogLoss);
        var logLossAverage = logLossValues.Average();
        var logLossStdDeviation = CalculateStandardDeviation(logLossValues);
        var logLossConfidenceInterval95 = CalculateConfidenceInterval95(logLossValues);

        var logLossReductionValues = metricsInMultipleFolds.Select(m => m.LogLossReduction);
        var logLossReductionAverage = logLossReductionValues.Average();
        var logLossReductionStdDeviation = CalculateStandardDeviation(logLossReductionValues);
        var logLossReductionConfidenceInterval95 = CalculateConfidenceInterval95(logLossReductionValues);

        Console.WriteLine($"*************************************************************************************************************");
        Console.WriteLine($"*       Metrics for {algorithmName} Multi-class Classification model      ");
        Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
        Console.WriteLine($"*       Average MicroAccuracy:    {microAccuracyAverage:0.###}  - Standard deviation: ({microAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({microAccuraciesConfidenceInterval95:#.###})");
        Console.WriteLine($"*       Average MacroAccuracy:    {macroAccuracyAverage:0.###}  - Standard deviation: ({macroAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({macroAccuraciesConfidenceInterval95:#.###})");
        Console.WriteLine($"*       Average LogLoss:          {logLossAverage:#.###}  - Standard deviation: ({logLossStdDeviation:#.###})  - Confidence Interval 95%: ({logLossConfidenceInterval95:#.###})");
        Console.WriteLine($"*       Average LogLossReduction: {logLossReductionAverage:#.###}  - Standard deviation: ({logLossReductionStdDeviation:#.###})  - Confidence Interval 95%: ({logLossReductionConfidenceInterval95:#.###})");
        Console.WriteLine($"*************************************************************************************************************");

    }

    public static double CalculateStandardDeviation (IEnumerable<double> values)
    {
        double average = values.Average();
        double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
        double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / (values.Count()-1));
        return standardDeviation;
    }

    public static double CalculateConfidenceInterval95(IEnumerable<double> values)
    {
        double confidenceInterval95 = 1.96 * CalculateStandardDeviation(values) / Math.Sqrt((values.Count()-1));
        return confidenceInterval95;
    }

    public static void PrintClusteringMetrics(string name, ClusteringMetrics metrics)
    {
        Console.WriteLine($"*************************************************");
        Console.WriteLine($"*       Metrics for {name} clustering model      ");
        Console.WriteLine($"*------------------------------------------------");
        Console.WriteLine($"*       Average Distance: {metrics.AverageDistance}");
        Console.WriteLine($"*       Davies Bouldin Index is: {metrics.DaviesBouldinIndex}");
        Console.WriteLine($"*************************************************");
    }

    [Conditional("DEBUG")]
    // This method using 'DebuggerExtensions.Preview()' should only be used when debugging/developing, not for release/production trainings
    public static void PeekDataViewInConsole(MLContext mlContext, IDataView dataView, IEstimator<ITransformer> pipeline, int numberOfRows = 4)
    {
        string msg = string.Format("Peek data in DataView: Showing {0} rows with the columns", numberOfRows.ToString());
        ConsoleWriteHeader(msg);

        //https://github.com/dotnet/machinelearning/blob/main/docs/code/MlNetCookBook.md#how-do-i-look-at-the-intermediate-data
        var transformer = pipeline.Fit(dataView);
        var transformedData = transformer.Transform(dataView);

        // 'transformedData' is a 'promise' of data, lazy-loading. call Preview
        //and iterate through the returned collection from preview.

        var preViewTransformedData = transformedData.Preview(maxRows: numberOfRows);

        foreach (var row in preViewTransformedData.RowView)
        {
            var ColumnCollection = row.Values;
            string lineToPrint = "Row--> ";
            foreach (KeyValuePair<string, object> column in ColumnCollection)
            {
                lineToPrint += $"| {column.Key}:{column.Value}";
            }
            Console.WriteLine(lineToPrint + "\n");
        }
    }

    [Conditional("DEBUG")]
    // This method using 'DebuggerExtensions.Preview()' should only be used when debugging/developing, not for release/production trainings
    public static void PeekVectorColumnDataInConsole(MLContext mlContext, string columnName, IDataView dataView, IEstimator<ITransformer> pipeline, int numberOfRows = 4)
    {
        string msg = string.Format("Peek data in DataView: : Show {0} rows with just the '{1}' column", numberOfRows, columnName );
        ConsoleWriteHeader(msg);

        var transformer = pipeline.Fit(dataView);
        var transformedData = transformer.Transform(dataView);

        // Extract the 'Features' column.
        var someColumnData = transformedData.GetColumn<float[]>(columnName)
                                                    .Take(numberOfRows).ToList();

        // print to console the peeked rows

        int currentRow = 0;
        someColumnData.ForEach(row => {
                                        currentRow++;
                                        String concatColumn = String.Empty;
                                        foreach (float f in row)
                                        {
                                            concatColumn += f.ToString();
                                        }

                                        Console.WriteLine();
                                        string rowMsg = string.Format("**** Row {0} with '{1}' field value ****", currentRow, columnName);
                                        Console.WriteLine(rowMsg);
                                        Console.WriteLine(concatColumn);
                                        Console.WriteLine();
                                      });
    }

    public static void ConsoleWriterSection(params string[] lines)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(" ");
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
        var maxLength = lines.Select(x => x.Length).Max();
        Console.WriteLine(new string('-', maxLength));
        Console.ForegroundColor = defaultColor;
    }

    public static void ConsolePressAnyKey()
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(" ");
        Console.WriteLine("Press any key to finish.");
        Console.ForegroundColor = defaultColor;
        Console.ReadKey();
    }

    public static void ConsoleWriteException(params string[] lines)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        const string exceptionTitle = "EXCEPTION";
        Console.WriteLine(" ");
        Console.WriteLine(exceptionTitle);
        Console.WriteLine(new string('#', exceptionTitle.Length));
        Console.ForegroundColor = defaultColor;
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }

    public static void ConsoleWriteWarning(params string[] lines)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        const string warningTitle = "WARNING";
        Console.WriteLine(" ");
        Console.WriteLine(warningTitle);
        Console.WriteLine(new string('#', warningTitle.Length));
        Console.ForegroundColor = defaultColor;
        foreach (var line in lines)
        {
            Console.WriteLine(line);
        }
    }

    // To evaluate the accuracy of the model's predicted rankings, prints out the Discounted Cumulative Gain and Normalized Discounted Cumulative Gain for search queries.
    public static void EvaluateMetrics(MLContext mlContext, IDataView predictions)
    {
        // Evaluate the metrics for the data using NDCG; by default, metrics for the up to 3 search results in the query are reported (e.g. NDCG@3).
        RankingMetrics metrics = mlContext.Ranking.Evaluate(predictions);

        Console.WriteLine($"DCG: {string.Join(", ", metrics.DiscountedCumulativeGains.Select((d, i) => $"@{i + 1}:{d:F4}").ToArray())}");

        Console.WriteLine($"NDCG: {string.Join(", ", metrics.NormalizedDiscountedCumulativeGains.Select((d, i) => $"@{i + 1}:{d:F4}").ToArray())}\n");
    }

    // Performs evaluation with the truncation level set up to 10 search results within a query.
    // This is a temporary workaround for this issue: https://github.com/dotnet/machinelearning/issues/2728.
    public static void EvaluateMetrics(MLContext mlContext, IDataView predictions, int truncationLevel)
    {
        if (truncationLevel < 1 || truncationLevel > 10)
        {
            throw new InvalidOperationException("Currently metrics are only supported for 1 to 10 truncation levels.");
        }

        //  Uses reflection to set the truncation level before calling evaluate.
        var mlAssembly = typeof(TextLoader).Assembly;
        var rankEvalType = mlAssembly.DefinedTypes.Where(t => t.Name.Contains("RankingEvaluator")).First();

        var evalArgsType = rankEvalType.GetNestedType("Arguments");
        var evalArgs = Activator.CreateInstance(rankEvalType.GetNestedType("Arguments"));

        var dcgLevel = evalArgsType.GetField("DcgTruncationLevel");
        dcgLevel.SetValue(evalArgs, truncationLevel);

        var ctor = rankEvalType.GetConstructors().First();
        var evaluator = ctor.Invoke(new object[] { mlContext, evalArgs });

        var evaluateMethod = rankEvalType.GetMethod("Evaluate");
        RankingMetrics metrics = (RankingMetrics)evaluateMethod.Invoke(evaluator, new object[] { predictions, "Label", "GroupId", "Score" });

        Console.WriteLine($"DCG: {string.Join(", ", metrics.DiscountedCumulativeGains.Select((d, i) => $"@{i + 1}:{d:F4}").ToArray())}");

        Console.WriteLine($"NDCG: {string.Join(", ", metrics.NormalizedDiscountedCumulativeGains.Select((d, i) => $"@{i + 1}:{d:F4}").ToArray())}\n");
    }
}
