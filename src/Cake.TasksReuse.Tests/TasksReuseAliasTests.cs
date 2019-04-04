using System;
using Xunit;

namespace Cake.TasksReuse.Tests
{
    public class TasksReuseAliasTests
    {
        FakeCakeContext Cake { get; }

        public TasksReuseAliasTests()
        {
            Cake = FakeCakeContext.CreateFakeContext();
        }

        [Fact]
        public void ShouldVerifyFilePathIsSet()
        {
            var nullOrEmpty = Assert.Throws<ArgumentException>(() => Cake.Context.ParseContext(""));
            Assert.Equal("The path to the JSON config file is missing.", nullOrEmpty.Message);
        }

        [Fact]
        public void ShouldParseConfigurationFile()
        {
            var buildCtx = Cake.Context.ParseContext("Resources/config.json");
            Assert.Equal("TasksReuseBuild", buildCtx.BuildName);
            Assert.Equal("build-tasks", buildCtx.BuildArtifactsDir);
        }
    }
}
