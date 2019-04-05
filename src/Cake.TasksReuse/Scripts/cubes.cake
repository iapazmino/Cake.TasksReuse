//Here we load an addin we built

Task("Create-Cube")
    .Does<BuildRunContext>(build =>
        {
            Information($"Tasks here will do use the addin and need params form {build.BuildName}!");
        }
    );