using CommonLib;
using ImageClassification.ModelScorer;

public class Program
{
    static void Main(string[] args)
    {
        string assetsRelativePath = @"../../../assets";
        string assetsPath = PathFind.GetAbsolutePath(assetsRelativePath);

        var tagsTsv = Path.Combine(assetsPath, "inputs", "images", "tags.tsv");
        var imagesFolder = Path.Combine(assetsPath, "inputs", "images");
        var inceptionPb = Path.Combine(assetsPath, "inputs", "inception", "tensorflow_inception_graph.pb");
        var labelsTxt = Path.Combine(assetsPath, "inputs", "inception", "imagenet_comp_graph_label_strings.txt");

        try
        {
            var modelScorer = new TFModelScorer(tagsTsv, imagesFolder, inceptionPb, labelsTxt);
            modelScorer.Score();

        }
        catch (Exception ex)
        {
            ConsoleHelper.ConsoleWriteException(ex.ToString());
        }

        ConsoleHelper.ConsolePressAnyKey();
    }
}
