using System.Data;

namespace RanorexReport.RanorexLogData
{
    public static class ReportRanorexHelper
    {
        public static DataTable CreateDataTable(List<ActivityItem> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("Level", typeof(string));
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("Message", typeof(string));

            for (int i = 0; i < items.Count; i++)
            {
                dt.Rows.Add(items[i].Time, items[i].Level, items[i].Category, string.Join(string.Empty, items[i].Message.Text == null ? string.Empty : string.Join("", items[i].Message.Text ?? Array.Empty<string>())));
            }

            return dt;
        }

        public static List<string> GetPathToTestCase(ReportRanorex root)
        {
            var pathToCurrentTest = new List<string>();
            var path = root.Activity.GetActivitiesBeforeFirstTestCase()
                .Where(p => p.Type.Equals("test-suite") | p.Type.Equals("smart-folder")).ToList();
            path.ForEach(p =>
            {
                pathToCurrentTest.Add(p.Testsuitename is null ? p.DisplayName : p.Testsuitename);
            });
            var testCase = root.Activity.FindFirst(a => a.Type.Equals("test-case")).DisplayName;
            pathToCurrentTest.Add(testCase);

            return pathToCurrentTest;
        }

        public static List<string> GetPathToTestCase(ReportRanorex report, ReportActivity testCase)
        {
            List<string> path = new();
            ReportActivity current = testCase;
            while (current != null)
            {
                if (!string.IsNullOrEmpty(current.DisplayName))
                    path.Insert(0, current.DisplayName);
                current = current.Parent; // Make sure you set Parent when building hierarchy
            }
            return path;
        }
    }
}
