using System.Text;
using System.Text.RegularExpressions;

namespace RanorexReport.AllureObjects
{
    public static class AllureHelper
    {
        public static void RenameDefectsToObservations(string allureReportPath)
        {
            var oldName = "Product defects";
            var newName = "Product observations";
            List<string> pathsToFilesToUpdate =
            [
                Path.Combine(allureReportPath, @"data\categories.json"),
            Path.Combine(allureReportPath, @"data\categories.csv"),
            Path.Combine(allureReportPath, @"history\categories-trend.json"),
            Path.Combine(allureReportPath, @"widgets\categories.json"),
            Path.Combine(allureReportPath, @"widgets\categories-trend.json"),
        ];

            foreach (var file in pathsToFilesToUpdate)
            {
                var fileText = File.ReadAllText(file);
                fileText = fileText.Replace(oldName, newName);

                File.WriteAllText(file, fileText);
            }
        }

        public static void FixIndexHtml(string allureReportPath)
        {
            string filePath = Path.Combine(allureReportPath, "index.html");
            string html = File.ReadAllText(filePath);

            // Regex to match base64 strings inside `d('...','<base64>')`
            var pattern = @"(?:data:[^,]+,|data\/attachments\/[^']+','|\bdata\/[^']+','?)((?:[A-Za-z0-9+\/]{4})*(?:[A-Za-z0-9+\/]{4}|[A-Za-z0-9+\/]{3}=|[A-Za-z0-9+\/]{2}={2}))";
            var regex = new Regex(pattern, RegexOptions.Compiled);

            var modified = html;
            int replacements = 0;

            modified = regex.Replace(modified, match =>
            {
                string base64 = match.Groups[1].Value;
                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(base64));

                if (decoded.Contains("defects"))
                {
                    decoded = decoded.Replace("defects", "observations");
                    replacements++;
                }

                string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(decoded));
                return match.Value.Replace(base64, encoded);
            });

            File.WriteAllText(filePath, modified, Encoding.UTF8);
        }

        public static void CopyHistory(string oldReportPath, string newResultsPath)
        {
            string sourceHistory = Path.Combine(oldReportPath, "history");
            string targetHistory = Path.Combine(newResultsPath, "history");

            if (!Directory.Exists(sourceHistory))
            {
                Console.WriteLine("No previous Allure history found to copy.");
                return;
            }

            FileHelper.CopyFolder(targetHistory, sourceHistory);
            Console.WriteLine("Allure history copied successfully.");
        }

        public static (string parentSuite, string suite, string subSuite, string testMethod, string package)
            MapAllureLabels(List<string> hierarchy)
        {
            // Always last element is test method
            string testMethod = hierarchy.Last();
            var folders = hierarchy.Take(hierarchy.Count - 1).ToList();

            string parentSuite = folders.Count > 0 ? folders[0] : "";
            string suite = folders.Count > 1 ? folders[1] : "";
            string package = string.Join(" / ", [parentSuite, suite]);


            if (folders.Any(p=> string.IsNullOrEmpty(p)))
            {
                folders.RemoveAll(string.IsNullOrEmpty);
            }
            string subSuite = folders.Count > 2
                ? string.IsNullOrEmpty(folders.Skip(2).First()) ? string.Empty : string.Join(" -> ", folders.Skip(2))
                : "";

            return (parentSuite, suite, subSuite, testMethod, package);
        }

        public static List<AllureLabel> BuildAllureLabels(List<string> hierarchy)
        {
            var labels = new List<AllureLabel>();

            // Extract mapped values
            var (parentSuite, suite, subSuite, testMethod, package) = MapAllureLabels(hierarchy);

            // Always add parentSuite
            if (!string.IsNullOrWhiteSpace(parentSuite))
                labels.Add(new AllureLabel { Name = "parentSuite", Value = parentSuite });

            // Always add suite
            if (!string.IsNullOrWhiteSpace(suite))
                labels.Add(new AllureLabel { Name = "suite", Value = suite });

            // Add subSuite only if exists
            if (!string.IsNullOrWhiteSpace(subSuite))
                labels.Add(new AllureLabel { Name = "subSuite", Value = subSuite });

            // Always add testMethod
            if (!string.IsNullOrWhiteSpace(testMethod))
                labels.Add(new AllureLabel { Name = "testMethod", Value = testMethod });

            labels.Add(new AllureLabel { Name = "package", Value = package });

            return labels;
        }
    }
}
