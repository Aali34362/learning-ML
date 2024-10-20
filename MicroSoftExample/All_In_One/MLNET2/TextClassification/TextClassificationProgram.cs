//Load sample data
using TextClassification;

public static class TextClassificationProgram
{
    public static void TextClassificationMain()
    {
        var sampleData = new ReviewSentiment.ModelInput()
        {
            Col0 = @"Crust is not good.",
        };

        //Load model and predict output
        var result = ReviewSentiment.Predict(sampleData);

        // Print sentiment
        Console.WriteLine($"Sentiment: {(result.PredictedLabel == 0 ? "Negative" : "Positive")}");
    }
}