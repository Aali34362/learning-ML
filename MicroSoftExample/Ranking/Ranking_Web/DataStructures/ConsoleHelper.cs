using WebRanking.DataStructures;

namespace Ranking_Web.DataStructures;

public static partial class ConsoleHelper
{
    // Prints out the the individual scores used to determine the relative ranking.
    public static void PrintScores(IEnumerable<SearchResultPrediction> predictions)
    {
        foreach (var prediction in predictions)
        {
            Console.WriteLine($"GroupId: {prediction.GroupId}, Score: {prediction.Score}");
        }
    }
}
