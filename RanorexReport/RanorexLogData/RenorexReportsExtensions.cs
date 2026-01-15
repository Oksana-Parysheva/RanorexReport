using Allure.Net.Commons;

namespace RanorexReport.RanorexLogData
{
    public static class RenorexReportsExtensions
    {
        public static void SetParentReferences(this ReportActivity activity, ReportActivity parent = null)
        {
            if (activity == null)
                return;

            activity.Parent = parent;

            if (activity.Activity != null)
            {
                foreach (var child in activity.Activity)
                {
                    SetParentReferences(child, activity);
                }
            }
        }

        public static ReportActivity FindFirstWithTestSuiteName(this ReportActivity activity)
        {
            if (activity == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(activity.Testsuitename))
            {
                return activity;
            }

            if (activity.Activity != null)
            {
                foreach (var child in activity.Activity)
                {
                    var result = child.FindFirstWithTestSuiteName();
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public static ReportActivity FindFirst(this ReportActivity activity, Func<ReportActivity, bool> predicate)
        {
            if (activity == null)
            {
                return null;
            }

            if (predicate(activity))
            {
                return activity;
            }

            if (activity.Activity != null)
            {
                foreach (var child in activity.Activity)
                {
                    var result = child.FindFirst(predicate);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        public static ReportActivity? FindSetupBeforeTestCase(this ReportActivity root)
        {
            if (root?.Activity == null)
            {
                return null;
            }

            foreach (var activity in root.Activity)
            {
                // If this node is test-suite or smart-folder, look deeper
                if (activity.Type is "test-suite" or "smart-folder")
                {
                    var found = activity.FindSetupBeforeTestCase();
                    if (found != null)
                    {
                        return found;
                    }
                }

                if (activity.Type.Equals("setup-container"))
                {
                    return activity;
                }

                // Search children for a "test-case"
                var testCaseIndex = activity.Activity?
                    .Select((a, i) => new { a, i })
                    .FirstOrDefault(x => x.a.Type.Equals("test-case"));

                if (testCaseIndex != null)
                {
                    // Find previous setup-container before test-case
                    var setup = activity.Activity?
                        .Take(testCaseIndex.i)
                        .LastOrDefault(a => a.Type.Equals("setup-container"));

                    if (setup != null)
                    {
                        return setup;
                    }
                }
            }

            return null;
        }

        public static List<ReportActivity> GetActivitiesBeforeFirstTestCase(this ReportActivity root)
        {
            var path = new List<ReportActivity>();
            if (root == null)
            {
                return path;
            }

            FindActivitiesBeforeTestCaseRecursive(root, path);
            return path;
        }

        private static bool FindActivitiesBeforeTestCaseRecursive(ReportActivity activity, List<ReportActivity> path)
        {
            if (activity == null)
            {
                return false;
            }

            // Add current activity to the path
            path.Add(activity);

            if (activity.Activity == null || activity.Activity.Count == 0)
            {
                return false;
            }

            // If this activity directly contains a "test-case", stop here
            if (activity.Activity.Any(a => a.Type.Equals("test-case")))
            {
                return true;
            }

            // Recurse deeper
            foreach (var child in activity.Activity)
            {
                if (FindActivitiesBeforeTestCaseRecursive(child, path))
                    return true;
            }

            // If this branch does not lead to a test-case, backtrack
            path.RemoveAt(path.Count - 1);
            return false;
        }

        public static string GetResultStatus(this ReportActivity activity)
            => activity.Result?.Equals("Success", StringComparison.OrdinalIgnoreCase) == true
                ? "passed"
                : (activity.Result?.Equals("Ignored", StringComparison.OrdinalIgnoreCase) == true
                    ? "skipped"
                    : "failed");

        public static List<ReportActivity> FindAll(this ReportActivity activity, Func<ReportActivity, bool> predicate)
        {
            List<ReportActivity> matches = new();
            if (predicate(activity))
                matches.Add(activity);

            if (activity.Activity != null)
            {
                foreach (var child in activity.Activity)
                    matches.AddRange(child.FindAll(predicate));
            }
            return matches;
        }

        public static List<string> GetHierarchy(this ReportActivity testCase)
        {
            var list = new List<string>();
            var current = testCase;

            while (current != null)
            {
                string name = string.Empty;
                if (!current.Type.Equals("iteration-container"))
                {
                    name =
                        current.DisplayName ??
                        current.Headertext ??
                        current.Testsuitename ??
                        current.Testcontainername ??
                        current.Type;
                }

                list.Add(name);
                current = current.Parent;
            }

            list.Reverse(); // root → test-case
            return list;
        }
    }
}
