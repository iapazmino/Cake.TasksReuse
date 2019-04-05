var dirsToDelete = GetDirectories("**/bin") + GetDirectories("**/obj") + GetDirectories("build");

var recursiveAndForce = new DeleteDirectorySettings { Recursive = true, Force = true };

Task("Clean")
    .IsDependentOn("Clean-BuildDirectory")
    .DoesForEach(
        (GetDirectories("**/bin") + GetDirectories("**/obj") + GetDirectories(dirBuildOutput)), dir =>
        {
            if(DirectoryExists(dir))
            {
                DeleteDirectory(dir, recursiveAndForce);
            }
        }
    );

Task("Clean-BuildDirectory")
    .Does<BuildRunContext>(build =>
        {
            if(DirectoryExists(build.BuildArtifactsDir))
            {
                DeleteDirectory(build.BuildArtifactsDir, recursiveAndForce);
            }
        }
    );
