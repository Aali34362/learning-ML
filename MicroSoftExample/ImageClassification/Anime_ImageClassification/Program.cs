﻿
// This file was auto-generated by ML.NET Model Builder. 

using Anime_ImageClassification;
using System.IO;

// Create single instance of sample data from first line of dataset for model input
var imageBytes = File.ReadAllBytes(@"D:\Programming\Asp\AspCore\v8\learning-ML\learning-ML\MicroSoftExample\ImageClassification\ConAppImagePredictRec\images\ASL\ASL.full.1129363.jpg");
MLModel1.ModelInput sampleData = new MLModel1.ModelInput()
{
    ImageSource = imageBytes,
};

// Make a single prediction on the sample data and print results.
var sortedScoresWithLabel = MLModel1.PredictAllLabels(sampleData);
Console.WriteLine($"{"Class",-40}{"Score",-20}");
Console.WriteLine($"{"-----",-40}{"-----",-20}");

foreach (var score in sortedScoresWithLabel)
{
    Console.WriteLine($"{score.Key,-40}{score.Value,-20}");
}
