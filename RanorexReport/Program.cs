//using RanorexReport.Core;

//internal class Program
//{
//    static void Main(string[] args)
//    {
//        // Temporary fallback (optional)
//        RanorexAllureGenerator.Generate(
//            ranorexArtifactsPath: @"D:\UVCS-to-allure\RxReport_Failed",
//            allureOutputPath: @"D:\UVCS-to-allure\RxReport_Failed",
//            buildId: "726538",
//            runName: "LocalRun",
//            zipName: null
//        );
//    }
//}


using RanorexReport.Core;
using System;
using System.CommandLine;

var root = new RootCommand("Ranorex → Allure converter");

var create = new Command("create", "Generate allure-results");

var buildId = new Option<int>("--buildId");
var runName = new Option<string>("--runName");
var zipName = new Option<string>("--zipName");

var ranorexPath = new Argument<string>("ranorexArtifactsPath");
var outputPath = new Argument<string>("allureOutputPath");

create.AddOption(buildId);
create.AddOption(runName);
create.AddOption(zipName);
create.AddArgument(ranorexPath);
create.AddArgument(outputPath);

create.SetHandler((rp, op, bId, rName, zName) =>
{
    RanorexAllureGenerator.Generate(
        ranorexArtifactsPath: rp,
        allureOutputPath: op,
        buildId: bId,
        runName: rName,
        zipName: zName
    );

}, ranorexPath, outputPath, buildId, runName, zipName);

root.AddCommand(create);

return root.Invoke(args);
