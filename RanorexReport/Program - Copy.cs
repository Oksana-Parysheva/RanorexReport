using RanorexReport;
using RanorexReport.AllureObjects;
using RanorexReport.Extensions;
using RanorexReport.RanorexLogData;
using System.Data;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

internal class ProgramMine
{
    static List<string> pathToCurrentTest = new List<string>();
    static List<long> durations = new List<long>();
    static string hostName = string.Empty;

    static string runName = "Build_36a";
    static int buildId = 7249;
    static string ranorexPath = "D:\\UVCS-to-allure\\rdp-reports\\by_builds\\" + runName;
    static string allureBasePath = @"D:\UVCS-to-allure\rdp-reports\allure3-version\MyTestTimeline";
    //static string allureHistoryReportToCopy = "";
    static string allureHistoryReportToCopy = "D:\\UVCS-to-allure\\rdp-reports\\allure3\\MyTest";

    static string ranorexZipFile = ranorexPath + "\\" + runName + ".zip";
    //static string ranorexZipFile = null;
    static string zipFile = string.Empty;
    static string pdfFile = string.Empty;
    static string allureResultsPath = Path.Combine(allureBasePath, "allure-results");
    static string allureReportPath = Path.Combine(allureBasePath, "allure-report");
    static string? buildVersion;

    private static void MainMine(string[] args)
    {
        Console.WriteLine("Generating Allure results...");
        GenerateAllureReports();
        File.Copy("categories.json", Path.Combine(allureResultsPath, "categories.json"), overwrite: true);

        if (!string.IsNullOrEmpty(buildVersion))
        {
            Directory.CreateDirectory(Path.Combine(allureBasePath, $"{buildVersion}_A3_Run_{buildId}"));
            CreateExecutorFile($"{buildVersion}_A3_Run_{buildId}", buildId);
        }
        else
        {
            CreateExecutorFile(runName, buildId);
        }

        //Console.WriteLine("Create history..." + $"npx allure history \"{allureBasePath}\"");
        //PowerShellRunner.RunCommand($"cd \"{allureBasePath}\"");
        //PowerShellRunner.RunCommand($"npx allure history \"allure-results\"");
        //Console.WriteLine("Generate Allure report Allure2..." + $"npx allure allure2 --output \"{allureReportPath}\" \"{allureResultsPath}\"");
        //PowerShellRunner.RunCommand($"npx allure allure2 --output \"{allureReportPath}\" \"{allureResultsPath}\"");
        //Console.WriteLine("Generate Allure report Awesome..." + $"npx allure awesome --output \"{allureReportPath}\" \"{allureResultsPath}\"");
        //PowerShellRunner.RunCommand($"npx allure awesome --output \"{allureReportPath}\" \"{allureResultsPath}\"");
        //Console.WriteLine("Generate Allure report Classic..." + $"npx allure classic --output \"{allureReportPath}\" \"{allureResultsPath}\"");
        //PowerShellRunner.RunCommand($"npx allure classic --output \"{allureReportPath}\" \"{allureResultsPath}\"");
        //Console.WriteLine("Generate Allure report Dashboard..." + $"npx allure dashboard --output \"{allureReportPath}\" \"{allureResultsPath}\"");
        //PowerShellRunner.RunCommand($"npx allure dashboard --output \"{allureReportPath}\" \"{allureResultsPath}\"");
        //Console.WriteLine("Create history..." + $"npx allure history \"{allureBasePath}\"");
        //PowerShellRunner.RunCommand($"npx allure history \"{allureResultsPath}\"");
        //PowerShellRunner.RunCommand($"cd \"{allureBasePath}\"");
        //PowerShellRunner.RunCommand($"npx allure history \"allure-results\"");

        //Console.WriteLine("Building Allure report..." + $"allure generate \"{allureResultsPath}\" --clean -o \"{allureReportPath}\"");
        //AllureHelper.CopyHistory(allureHistoryReportToCopy, allureResultsPath);
        //PowerShellRunner.RunCommand($"allure generate \"{allureResultsPath}\" --clean -o \"{allureReportPath}\"");
    }

    private static void CreateExecutorFile(string runFolderName, int buildNumber)
    {
        var templatePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var templateText = File.ReadAllText(Path.Combine(templatePath, "executor_template.json"));
        var newText = templateText.Replace("<buildNumber>", buildNumber.ToString()).Replace("<runFolder>", runFolderName).Replace("<buildVersion>", buildVersion);
        File.WriteAllText(Path.Combine(allureResultsPath, "executor.json"), newText);
    }

    private static void GenerateAllureReports()
    {
        var reports = GetReports();
        var testSuites = reports.Select(report => report.Activity.FindFirstWithTestSuiteName().Testsuitename).ToList().Distinct().ToList();
        Directory.CreateDirectory(allureResultsPath);
        foreach (var suiteName in testSuites)
        {
            var testReportsInOneSuite = reports
                .Where(r => r.Activity.FindFirstWithTestSuiteName()?.Testsuitename == suiteName)
                .ToList();

            List<AllureTestResult> testResults = new();

            // copy zip and rename
            if (!string.IsNullOrEmpty(ranorexZipFile))
            {
                zipFile = $"{Guid.NewGuid().ToString()}-attachment{Path.GetExtension(ranorexZipFile)}";
                var destPath = Path.Combine(allureResultsPath, zipFile);
                Directory.CreateDirectory(allureResultsPath);

                File.Copy(ranorexZipFile, destPath, overwrite: true);
            }

            uint duration = 0;

            foreach (var report in testReportsInOneSuite)
            {
                duration = report.Activity.Durationms;

                // copy pdf and rename
                if (!string.IsNullOrEmpty(report.PathToPdfFile))
                {
                    pdfFile = $"{Guid.NewGuid().ToString()}-attachment{Path.GetExtension(report.PathToPdfFile)}";
                    var destPath = Path.Combine(allureResultsPath, pdfFile);
                    Directory.CreateDirectory(allureResultsPath);

                    File.Copy(Path.Combine(ranorexPath, report.PathToPdfFile), destPath, overwrite: true);
                }

                report.Activity.FindFirstWithTestSuiteName().SetParentReferences();
                var testSuiteName = report.Activity.FindFirstWithTestSuiteName().Testsuitename;
                var testCases = report.Activity
                 .FindAll(a =>
                     (a.Type == "iteration-container" && a.TestentryActivityType.Equals("testcase", StringComparison.OrdinalIgnoreCase)) ||
                     (a.Type == "test-case" && a.ActivityExectype?.Equals("execute", StringComparison.OrdinalIgnoreCase) == true))
                 .ToList();

                if (testCases.Count == 0)
                {
                    Console.WriteLine($"No test-case nodes found in suite: {suiteName}");
                    continue;
                }
                
                buildVersion = GetHostAppBuildVersion(report.Activity);
                hostName = report.Activity.Host;
                CreateEnvironmentFile(report.Activity);
                //if (string.IsNullOrEmpty(buildVersion))
                //{
                //    CreateEnvironmentFile(report.Activity);
                //}
                //else
                //{
                //    CreateEnvironmentFile(report.Activity, buildVersion);
                //}

                foreach (var testCase in testCases)
                {
                    var hierarchy = testCase.GetHierarchy();
                    pathToCurrentTest = hierarchy;

                    if (testCase.Type.Equals("test-case") && testCase.ActivityExectype.Equals("execute", StringComparison.OrdinalIgnoreCase))
                    {
                        var allureTest = CreateAllureResultFiles(report, testCase);
                        testResults.Add(allureTest);
                    }
                    else
                    {
                        var allureTests = CreateAllureResultFilesFoIterations(report, testCase);
                        testResults.AddRange(allureTests);
                    }
                }
            }

            durations.Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            durations.Add(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + duration);
            var container = new AllureContainer
            {
                Uuid = Guid.NewGuid().ToString(),
                Name = suiteName,
                Children = testResults.Select(t => t.TestCaseId).ToList(),
                Start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + duration
            };

            // Write suite container JSON
            var suitePath = Path.Combine(allureResultsPath, $"{container.Uuid}-container.json");
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            File.WriteAllText(suitePath, JsonSerializer.Serialize(container, options));

            Console.WriteLine($"Allure suite created: {suiteName} ({testResults.Count} test cases)");
        }
    }

    public static string? GetHostAppBuildVersion(ReportActivity root)
    {
        // 1. Find the test-case
        var noteVersionTest = root.FindFirst(a =>
            a.Type == "test-case" &&
            string.Equals(a.DisplayName, "NoteHostAppVersion", StringComparison.OrdinalIgnoreCase));

        if (noteVersionTest == null)
            return null;

        // 2. Search all items recursively inside this test-case
        var items = noteVersionTest
            .FindAll(a => a.Item != null)
            .SelectMany(a => a.Item);

        foreach (var item in items)
        {
            if (item.Message?.Text == null)
                continue;

            // Join string[] → single string
            string message = string.Join(" ", item.Message.Text).Trim();

            // 3. Extract build version
            // Example: "Build Version: BLD_0036a"
            var match = Regex.Match(message, @"Build\s+Version:\s*(\S+)");
            if (match.Success)
                return match.Groups[1].Value;
        }

        return null;
    }

    public static Before GetBeforeTestSteps(ReportRanorex report)
    {
        var setupContainer = report.Activity.FindSetupBeforeTestCase();

        if (setupContainer is null)
            return null;

        var steps = BuildStepRecursive(setupContainer);
        durations.Add(steps.Start);
        durations.Add(steps.Stop);

        var beforeSetup = new Before
        {
            Name = steps.Name,
            Status = steps.Status,
            Stage = "finished",
            Steps = steps.Steps,
            Start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + setupContainer.Durationms,
        };

        return beforeSetup;
    }

    public static List<ReportRanorex> GetReports()
    {
        var extension = ".rxlog.data";
        if (!extension.StartsWith("."))
        {
            extension = "." + extension;
        }

        string[] files = Directory.GetFiles(ranorexPath, "*" + extension);

        Dictionary<string, List<ReportRanorex>> pdfReport = new Dictionary<string, List<ReportRanorex>>();
        List<ReportRanorex> reports = new List<ReportRanorex>();
        for (int i = 0; i < files.Count(); i++)
        {
            var file = files[i];
            var text = File.ReadAllText(file);
            var xmlResObject = file.FromXmlTo<ReportRanorex>();
            var pdfPath = file.Replace(extension, ".pdf");
            if (File.Exists(pdfPath))
            {
                xmlResObject.PathToPdfFile = Path.GetFileName(pdfPath);
            }
            xmlResObject.PathToLogDataFile = Path.GetFileName(file.Replace(".data", ""));
            reports.Add(xmlResObject);
        }

        return reports;
    }

    public static AllureTestResult CreateAllureResultFiles(ReportRanorex reportObject, ReportActivity testCase)
    {
        var suiteUuid = Guid.NewGuid().ToString();
        var testUuid = Guid.NewGuid().ToString();
        var attachmentUuid = Guid.NewGuid().ToString();

        var extension = ".pdf";
        string[] files = Directory.GetFiles(ranorexPath, $"*{extension}");

        // before
        var beforeSteps = GetBeforeTestSteps(reportObject);
        // after
        //var afterSteps = GetAfterTestSteps(reportObject);

        // Container for test
        // --- Create suite (container)
        var suite = new AllureContainer
        {
            Uuid = suiteUuid,
            Name = string.Join(".", pathToCurrentTest),
            Children = new List<string> { testUuid },
            Befores = beforeSteps is null ? new List<Before>() : new List<Before> { beforeSteps },
            //Afters = afterSteps
            Start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + testCase.Durationms,
        };

        var test = CreateAllureTest(reportObject.Activity, testCase, testUuid);

        if (!string.IsNullOrEmpty(zipFile))
        {
            test.Attachments.Add(new AllureAttachment()
            {
                Name = $"RanorexFullReport.zip",
                Source = zipFile,
                Type = "application/zip" // to download TODO: link to report
            }
            );
        }

        if (!string.IsNullOrEmpty(pdfFile))
        {
            test.Attachments.Add(new AllureAttachment()
            {
                Name = $"RanorexFullReport.pdf",
                Source = pdfFile,
                Type = "application/pdf" // to download TODO: link to report
            }
            );
        }

        // --- Serialize and write to JSON files
        var suitePath = Path.Combine(allureResultsPath, $"{suiteUuid}-container.json");
        var testPath = Path.Combine(allureResultsPath, $"{testUuid}-result.json");

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        File.WriteAllText(suitePath, JsonSerializer.Serialize(suite, options));
        File.WriteAllText(testPath, JsonSerializer.Serialize(test, options));

        return test;
    }

    public static List<AllureTestResult> CreateAllureResultFilesFoIterations(ReportRanorex reportObject, ReportActivity testCase)
    {
        List<AllureTestResult> allIterations = new List<AllureTestResult>();
        var iteratedTestCases = testCase.FindAll(p => p.Type.Equals("test-case")).ToList();
        foreach (var iteration in iteratedTestCases)
        {
            var suiteUuid = Guid.NewGuid().ToString();
            var testUuid = Guid.NewGuid().ToString();
            var attachmentUuid = Guid.NewGuid().ToString();

            var extension = ".pdf";
            string[] files = Directory.GetFiles(ranorexPath, $"*{extension}");

            // before
            var beforeSteps = GetBeforeTestSteps(reportObject);
            // after
            //var afterSteps = GetAfterTestSteps(reportObject);

            // Container for test
            // --- Create suite (container)
            var suite = new AllureContainer
            {
                Uuid = suiteUuid,
                Name = string.Join(".", pathToCurrentTest),
                Children = new List<string> { testUuid },
                Befores = beforeSteps is null ? new List<Before>() : new List<Before> { beforeSteps },
                Start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                Stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + iteration.Durationms,
                //Afters = afterSteps
            };

            var iterationParameters = GetIterationParameters(iteration);
            var test = CreateAllureTest(reportObject.Activity, iteration, testUuid, iterationParameters);

            if (!string.IsNullOrEmpty(zipFile))
            {
                test.Attachments.Add(new AllureAttachment()
                {
                    Name = $"RanorexFullReport.zip",
                    Source = zipFile,
                    Type = "application/zip" // to download TODO: link to report
                }
                );
            }

            if (!string.IsNullOrEmpty(pdfFile))
            {
                test.Attachments.Add(new AllureAttachment()
                {
                    Name = $"RanorexFullReport.pdf",
                    Source = pdfFile,
                    Type = "application/pdf" // to download TODO: link to report
                }
                );
            }

            // --- Serialize and write to JSON files
            var suitePath = Path.Combine(allureResultsPath, $"{suiteUuid}-container.json");
            var testPath = Path.Combine(allureResultsPath, $"{testUuid}-result.json");

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            File.WriteAllText(suitePath, JsonSerializer.Serialize(suite, options));
            File.WriteAllText(testPath, JsonSerializer.Serialize(test, options));

            allIterations.Add(test);
        }

        return allIterations;
    }

    private static Dictionary<string, string> GetIterationParameters(ReportActivity iterationCase)
    {
        var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (iterationCase?.DataRow?.Fields != null)
        {
            foreach (var f in iterationCase.DataRow.Fields)
            {
                var name = f.Name?.Trim() ?? string.Empty;
                var value = (f.Value ?? string.Empty).Trim();
                if (!string.IsNullOrEmpty(name))
                    parameters[name] = value;
            }
        }
        return parameters;
    }

    public static AllureTestResult CreateAllureTest(ReportActivity testsuite, ReportActivity testCase, string testUuid, Dictionary<string, string> iterParams)
    {
        string displayName = $"{testCase.DisplayName} [Iteration {testCase.Iteration}]";
        var historyUuid = (string.Join(".", pathToCurrentTest) + displayName).GetMD5HashCode();
        var steps = GetStepsRecursive(testCase);
        List<AllureParameter> parameters = [new AllureParameter { Name = "test_id", Value = testCase.Testcontainerid }];
        foreach (var param in iterParams)
        {
            parameters.Add(new AllureParameter { Name = param.Key, Value = param.Value });
        }
        string fullName = string.Join(".", pathToCurrentTest) + $"{ testCase.DisplayName} [Iteration.Count]";

        StatusDetails status = new StatusDetails();
        if (testCase.Result.Equals("Failed"))
        {
            var failedStep = steps.First(p => p.Status.Equals("failed"));
            var attachment = FindStepDetailsAttachment(failedStep);
            string decoded = WebUtility.HtmlDecode(File.ReadAllText(Path.Combine(allureResultsPath, attachment.Source)));
            string cleanedText = Regex.Replace(decoded, @"\s+", " ").Trim();
            status = new StatusDetails()
            {
                Message = cleanedText
            };
        }

        var labels = AllureHelper.BuildAllureLabels(pathToCurrentTest);
        if (!string.IsNullOrEmpty(buildVersion))
        {
            labels.Add(new AllureLabel { Name = "buildVersion", Value = buildVersion });
        }
        var test = new AllureTestResult
        {
            Uuid = testUuid,
            HistoryId = historyUuid,
            TestCaseId = historyUuid,
            Name = testCase.DisplayName,
            TestCaseName = testCase.DisplayName,
            TitlePath = new List<string> { $"{string.Join(".", pathToCurrentTest)}{displayName}" },
            FullName = fullName,
            Status = testCase.GetResultStatus(),
            StatusDetails = status,
            Stage = "finished",
            Labels = labels,
            Attachments = new List<AllureAttachment>(),
            Start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + testCase.Durationms,
            DescriptionHtml = AddDescriptionHtmlDataToAllure(testCase),
            Steps = steps,
            Parameters = parameters
        };

        if (testCase.Result.Equals("Failed"))
        {
            if (testCase.Videofile != null)
            {
                string sourcePath = Path.Combine(ranorexPath, testCase.Videofile);
                string fileName = $"{Guid.NewGuid().ToString()}-attachment{Path.GetExtension(sourcePath)}";
                string destPath = Path.Combine(allureResultsPath, fileName);

                File.Copy(sourcePath, destPath, overwrite: true);

                test.Attachments.Add(new AllureAttachment()
                {
                    Name = "Video artefact",
                    Source = fileName,
                    Type = "video/webm"
                });
            }
        }

        return test;
    }

    public static AllureTestResult CreateAllureTest(ReportActivity testsuite, ReportActivity testCase, string testUuid)
    {
        var historyUuid = string.Join(".", pathToCurrentTest).GetMD5HashCode();
        var steps = GetStepsRecursive(testCase);
        List<AllureParameter> parameters = [new AllureParameter { Name = "test_id", Value = testCase.Testcontainerid }];
        //testsuite.FindFirstWithTestSuiteName().Params.ToList().ForEach(p => parameters.Add(new AllureParameter { Name = p.Name, Value = p.Value.Trim() }));

        StatusDetails status = new StatusDetails();
        if (testCase.Result.Equals("Failed"))
        {
            var failedStep = steps.First(p => p.Status.Equals("failed"));
            var attachment = FindStepDetailsAttachment(failedStep);
            string decoded = WebUtility.HtmlDecode(File.ReadAllText(Path.Combine(allureResultsPath, attachment.Source)));
            string cleanedText = Regex.Replace(decoded, @"\s+", " ").Trim();
            status = new StatusDetails()
            {
                Message = cleanedText
            };
        }

        var labels = AllureHelper.BuildAllureLabels(pathToCurrentTest);
        labels.Add(new AllureLabel { Name = "host", Value = hostName });
        if (!string.IsNullOrEmpty(buildVersion))
        {
            labels.Add(new AllureLabel { Name = "buildVersion", Value = buildVersion });
        }

        var test = new AllureTestResult
        {
            Uuid = testUuid,
            HistoryId = historyUuid,
            TestCaseId = historyUuid,
            Name = testCase.DisplayName,
            TestCaseName = testCase.DisplayName,
            TitlePath = pathToCurrentTest,
            FullName = string.Join(".", pathToCurrentTest),
            Status = testCase.GetResultStatus(),
            StatusDetails = status,
            Stage = "finished",
            Labels = labels,
            Attachments = new List<AllureAttachment>(),
            Start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + testCase.Durationms,
            DescriptionHtml = AddDescriptionHtmlDataToAllure(testCase),
            Steps = steps,
            Parameters = parameters
        };

        if (testCase.Result.Equals("Failed"))
        {
            if (testCase.Videofile != null)
            {
                string sourcePath = Path.Combine(ranorexPath, testCase.Videofile);
                string fileName = $"{Guid.NewGuid().ToString()}-attachment{Path.GetExtension(sourcePath)}";
                string destPath = Path.Combine(allureResultsPath, fileName);

                File.Copy(sourcePath, destPath, overwrite: true);

                test.Attachments.Add(new AllureAttachment()
                {
                    Name = "Video artefact",
                    Source = fileName,
                    Type = "video/webm"
                });
            }
        }

        return test;
    }

    public static AllureAttachment? FindStepDetailsAttachment(Step step)
    {
        if (step == null)
            return null;

        // 1. Check current step attachments
        if (step.Attachments != null)
        {
            var match = step.Attachments
                .FirstOrDefault(a => a.Name == "Step Details");

            if (match != null)
                return match;
        }

        // 2. Recursively check child steps
        if (step.Steps != null)
        {
            foreach (var child in step.Steps)
            {
                var found = FindStepDetailsAttachment(child);
                if (found != null)
                    return found;
            }
        }

        // Nothing found
        return null;
    }

    public static string AddDescriptionHtmlDataToAllure(ReportActivity testCase)
        => BuildHtmlSummaryTable(testCase);

    private static void CreateEnvironmentFile(ReportActivity suite)
    {
        var filePath = Path.Combine(allureResultsPath, "environment.properties");

        List<string> lines = new List<string>
        {
            $"execution.time={suite.Timestamp ?? "-"}",
            $"computer.name={suite.Host ?? "-"}",
            $"operating.system={suite.Osversion ?? "-"}",
            $"screen.dimension={suite.Screenresolution ?? "-"}",
            $"run.configuration.name={suite.Runconfigname ?? "-"}",
            $"username={suite.User ?? "-"}",
            $"language={suite.Oslanguage ?? "-"}",
            $"duration={suite.Duration ?? "-"}"
        };

        suite.FindFirstWithTestSuiteName().Params.ToList().ForEach(p => lines.Add($"{p.Name}={p.Value.Trim().Replace(@"\", @"\\")}"));

        File.WriteAllLines(filePath, lines, new UTF8Encoding(false));
    }

    private static void CreateEnvironmentFile(ReportActivity suite, string applicationBuildVersion)
    {
        var filePath = Path.Combine(allureResultsPath, "environment.properties");

        List<string> lines = new List<string>
        {
            $"application.build.version ={applicationBuildVersion ?? "-"}",
            $"execution.time={suite.Timestamp ?? "-"}",
            $"computer.name={suite.Host ?? "-"}",
            $"operating.system={suite.Osversion ?? "-"}",
            $"screen.dimension={suite.Screenresolution ?? "-"}",
            $"run.configuration.name={suite.Runconfigname ?? "-"}",
            $"username={suite.User ?? "-"}",
            $"language={suite.Oslanguage ?? "-"}",
            $"duration={suite.Duration ?? "-"}"
        };

        suite.FindFirstWithTestSuiteName().Params.ToList().ForEach(p => lines.Add($"{p.Name}={p.Value.Trim().Replace(@"\", @"\\")}"));

        File.WriteAllLines(filePath, lines, new UTF8Encoding(false));
    }

    private static string BuildHtmlSummaryTable(ReportActivity testCase)
    {
        var sb = new StringBuilder();

        if (testCase.Detail != null)
            sb.AppendLine(Regex.Replace(WebUtility.HtmlDecode(testCase.Detail), @"\s+", " ").Trim());
        //else if (testCase.Errormessage != null)
        //    sb.AppendLine(Regex.Replace(WebUtility.HtmlDecode(testCase.Errormessage.Text.First()), @"\s+", " ").Trim());

        return sb.ToString();
    }

    public static List<Step> GetStepsRecursive(ReportActivity testCase)
    {
        List<Step> steps = new();

        foreach (var child in testCase.Activity)
        {
            steps.Add(BuildStepRecursive(child));
        }

        return steps;
    }

    private static Step BuildStepRecursive(ReportActivity activity)
    {
        // Build HTML table if items exist
        var attachments = new List<AllureAttachment>();

        if (activity.Result.Equals("Ignored") || activity.Result.Equals("Failed"))
        {
            if (activity.Detail != null)
            {
                string textContent = Regex.Replace(Regex.Replace(activity.Detail, "<.*?>", ""), @"\s+", " ").Trim();
                textContent = Regex.Replace(WebUtility.HtmlDecode(textContent), @"\s+", " ").Trim();
                var fileName = $"{Guid.NewGuid().ToString()}-attachment.html";
                File.WriteAllText(Path.Combine(allureResultsPath, fileName), textContent);

                attachments.Add(new AllureAttachment
                {
                    Name = "Step Details",
                    Source = fileName,
                    Type = "text/plain"
                });
            }
            else if (activity.Errormessage != null)
            {
                string textContent = Regex.Replace(Regex.Replace(activity.Errormessage.Text.First(), "<.*?>", ""), @"\s+", " ").Trim();
                textContent = Regex.Replace(WebUtility.HtmlDecode(textContent), @"\s+", " ").Trim();
                var fileName = $"{Guid.NewGuid().ToString()}-attachment.html";
                File.WriteAllText(Path.Combine(allureResultsPath, fileName), textContent);

                attachments.Add(new AllureAttachment
                {
                    Name = "Step Details",
                    Source = fileName,
                    Type = "text/plain"
                });
            }
        }

        if (activity.Item != null && activity.Item.Any())
        {
            var htmlUuid = Guid.NewGuid().ToString();
            var dataTable = ReportRanorexHelper.CreateDataTable(activity.Item.ToList());
            var htmlTable = dataTable.ToHtmlTable();
            var htmlFilePath = $"{htmlUuid}-attachment.html";
            File.WriteAllText(Path.Combine(allureResultsPath, htmlFilePath), htmlTable);

            attachments.Add(new AllureAttachment
            {
                Name = "Step table",
                Source = htmlFilePath,
                Type = "text/html"
            });

            foreach (var item in activity.Item)
            {
                if ((item.Level?.Equals("Error", StringComparison.OrdinalIgnoreCase) == true) ||
                    (!string.IsNullOrEmpty(item.Errimg) || !string.IsNullOrEmpty(item.Errthumb)))
                {
                    // Process both errimg and errthumb if available
                    foreach (var imagePath in new[] { item.Errimg})
                    {
                        if (string.IsNullOrEmpty(imagePath))
                            continue;

                        string sourcePath = Path.Combine(ranorexPath, imagePath);
                        if (!File.Exists(sourcePath))
                            continue;

                        htmlUuid = Guid.NewGuid().ToString();
                        string fileName = $"{htmlUuid}-attachment{Path.GetExtension(sourcePath)}";
                        string destPath = Path.Combine(allureResultsPath, fileName);

                        File.Copy(sourcePath, destPath, overwrite: true);

                        attachments.Add(new AllureAttachment
                        {
                            Name = Path.GetFileNameWithoutExtension(sourcePath),
                            Source = fileName,
                            Type = "image/jpeg"
                        });
                    }

                    if (item.Metainfo.Stacktrace != null)
                    {
                        var htmlUuidST = Guid.NewGuid().ToString();
                        var htmlFilePathST = $"{htmlUuid}-attachment.html";
                        File.WriteAllText(Path.Combine(allureResultsPath, htmlFilePathST), item.Metainfo.Stacktrace);

                        attachments.Add(new AllureAttachment
                        {
                            Name = "View Stacktrace",
                            Source = htmlFilePathST,
                            Type = "text/plain"
                        });
                    }
                }
            }
        }

        // Recursively process sub-activities (any depth)
        List<Step> subSteps = new();
        if (activity.Activity != null && activity.Activity.Any())
        {
            foreach (var subActivity in activity.Activity)
            {
                subSteps.Add(BuildStepRecursive(subActivity));
            }
        }

        // Determine name
        string name = string.IsNullOrEmpty(activity.DisplayName)
            ? (activity.Type?.Equals("setup-container", StringComparison.OrdinalIgnoreCase) == true
                ? "SETUP"
                : activity.Type?.Equals("teardown-container", StringComparison.OrdinalIgnoreCase) == true
                    ? "TEARDOWN"
                    : "STEP")
            : activity.DisplayName;

        List<AllureParameter> stepParameters = new List<AllureParameter>();
        foreach (var paramEls in activity.Params)
        {
            stepParameters.Add(new AllureParameter
            {
                Name = paramEls.Name,
                Value = paramEls.Value
            });
        }

        // Build step
        var step = new Step
        {
            Name = name,
            Status = activity.GetResultStatus(),
            Stage = "finished",
            Steps = subSteps,
            Start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            Stop = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + activity.Durationms,
            Attachments = attachments,
            Parameters = stepParameters
        };

        return step;
    }
}
