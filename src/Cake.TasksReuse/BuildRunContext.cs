﻿namespace Cake.TasksReuse
{
    public class BuildRunContext
    {
        public string BuildName { get; set; }
        public string BuildArtifactsDir { get; set; }
        public string PackageSpecPath { get; set; }
        public string PackageVersion { get; set; }

        public BuildRunContext()
        {
            BuildArtifactsDir = "dir";
        }
    }
}