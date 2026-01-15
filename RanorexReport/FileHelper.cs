namespace RanorexReport
{
    public static class FileHelper
    {
        public static void CopyFolder(string targetFolderPath, string sourceFolderPath)
        {
            Directory.CreateDirectory(targetFolderPath);

            foreach (var file in Directory.GetFiles(sourceFolderPath, "*.*", SearchOption.AllDirectories))
            {
                var relative = file.Substring(sourceFolderPath.Length).TrimStart(Path.DirectorySeparatorChar);
                var dest = Path.Combine(targetFolderPath, relative);
                Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
                File.Copy(file, dest, true);
            }
        }
    }
}
