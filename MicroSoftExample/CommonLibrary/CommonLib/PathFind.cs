using System.Reflection;

namespace CommonLib;

public static class PathFind
{
    static FileInfo _dataRoot = new FileInfo(Assembly.GetExecutingAssembly().Location);
    public static string GetAbsolutePath(string relativePath)
    {
        string assemblyFolderPath = _dataRoot.Directory!.Parent!.Parent!.Parent!.FullName;
        return $"{assemblyFolderPath}{relativePath}";
    }
    public static string GetAssetsPath(params string[] paths)
    {
        if (paths == null || paths.Length == 0)
            return null!;

        return Path.Combine(paths.Prepend(_dataRoot.Directory!.Parent!.Parent!.Parent!.FullName).ToArray());
    }
    public static string DeleteAssets(params string[] paths)
    {
        var location = GetAssetsPath(paths);

        if (!string.IsNullOrWhiteSpace(location) && File.Exists(location))
            File.Delete(location);
        return location;
    }
    ////public static string GetAbsolutePath(string relativePath)
    ////{
    ////    FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
    ////    string assemblyFolderPath = _dataRoot.Directory!.Parent!.Parent!.Parent!.FullName;
    ////    return $"{assemblyFolderPath}{relativePath}";
    ////}
}
