using ImageClassification.ImageDataStructures;

namespace ImageClassification_TensorFlow.ModelScorer;

public static partial class ConsoleHelper
{
    public static void ConsoleWrite(this ImageNetDataProbability self)
    {
        var defaultForeground = Console.ForegroundColor;
        var labelColor = ConsoleColor.Magenta;
        var probColor = ConsoleColor.Blue;
        var exactLabel = ConsoleColor.Green;
        var failLabel = ConsoleColor.Red;

        Console.Write("ImagePath: ");
        Console.ForegroundColor = labelColor;
        Console.Write($"{Path.GetFileName(self.ImagePath)}");
        Console.ForegroundColor = defaultForeground;
        Console.Write(" labeled as ");
        Console.ForegroundColor = labelColor;
        Console.Write(self.Label);
        Console.ForegroundColor = defaultForeground;
        Console.Write(" predicted as ");
        if (self.Label.Equals(self.PredictedLabel))
        {
            Console.ForegroundColor = exactLabel;
            Console.Write($"{self.PredictedLabel}");
        }
        else
        {
            Console.ForegroundColor = failLabel;
            Console.Write($"{self.PredictedLabel}");
        }
        Console.ForegroundColor = defaultForeground;
        Console.Write(" with probability ");
        Console.ForegroundColor = probColor;
        Console.Write(self.Probability);
        Console.ForegroundColor = defaultForeground;
        Console.WriteLine("");
    }
}
