#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=OpenCover&version=4.7.922"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var packageVersion = Argument("packageVersion", "1.0.0");

var dirBuildOutput = "build";

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore-Build-And-Pack")
    .IsDependentOn("Test");

Task("CI")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore-Build-And-Pack")
    .IsDependentOn("Test-With-Coverage");
    
var recursiveAndForce = new DeleteDirectorySettings { Recursive = true, Force = true };
Task("Clean")
    .DoesForEach(
        (GetDirectories("**/bin") + GetDirectories("**/obj") + GetDirectories(dirBuildOutput)), dir =>
        {
            if(DirectoryExists(dir))
            {
                DeleteDirectory(dir, recursiveAndForce);
            }
        }
    );
    
Task("Restore-Build-And-Pack")
    .Does(() =>
        {
            MSBuild(
                "Cake.TasksReuse.sln",
                settings => settings
                    .UseToolVersion(MSBuildToolVersion.VS2017)
                    .SetConfiguration(configuration)
                    .WithRestore()
                    .WithProperty("PackageVersion", packageVersion)
            );
        }
    );
    
var testsDllPath = $"Cake.TasksReuse.Tests/bin/{configuration}/netstandard2.0/Cake.TasksReuse.Tests.dll";
var xunitSettings = new XUnit2Settings { OutputDirectory = dirBuildOutput, XmlReport = true, ShadowCopy = false };
Task("Test")
    .Does(() =>
        {
            EnsureDirectoryExists(dirBuildOutput);
            XUnit2(testsDllPath, xunitSettings);
        }
    );

var openCoverSettings = new OpenCoverSettings().WithFilter("+[Cake.TasksReuse]*").WithFilter("-[Cake.TasksReuse.Tests]*");
var openCoverReportPath = new FilePath($"{dirBuildOutput}/Cake.TasksReuse.Tests.Coverage.dll.xml");
Task("Test-With-Coverage")
    .Does(() =>
        {
            EnsureDirectoryExists(dirBuildOutput);
            OpenCover(
                tool => { tool.XUnit2(testsDllPath, xunitSettings); },
                openCoverReportPath,
                openCoverSettings
            );
        }
    );
    
RunTarget(target);