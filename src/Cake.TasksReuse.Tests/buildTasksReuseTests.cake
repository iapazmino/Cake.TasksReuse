//This test is how a script that re-uses this package should look like

#addin nuget:?package=Cake.TasksReuse&version=1.0.0

using Cake.TasksReuse;

var target = Argument("target", "Default");

Setup<BuildRunContext>(ctx =>
    {
        return ParseContext("Resources/config.json");
    }
);

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Create-Cube")
    .IsDependentOn("Package-Model");