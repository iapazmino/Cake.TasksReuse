 
Task("Package-Model")
    .IsDependentOn("Copy-Package-Content")
    .Does<BuildRunContext>(build =>
        {
            Information($"Packaging for {build.BuildName}...");
            EnsureDirectoryExists(build.BuildArtifactsDir);
            NuGetPack(
                build.PackageSpecPath,
                new NuGetPackSettings {
                    Version = build.PackageVersion,
                    BasePath = build.BuildArtifactsDir,
                    OutputDirectory = build.BuildArtifactsDir
                }
            );
        }
    );

Task("Copy-Package-Content")
    .Does<BuildRunContext>(build =>
        {
            Information($"Copying files from places to {build.BuildArtifactsDir}.");
        }
    );