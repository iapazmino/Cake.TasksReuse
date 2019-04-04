using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing;
using NSubstitute;

namespace Cake.TasksReuse.Tests
{
    /// <summary>
    /// Fake cake context.
    /// See https://raw.githubusercontent.com/cake-contrib/Cake.FileHelpers/master/Cake.FileHelpers.Tests/Fakes/FakeCakeContext.cs
    /// </summary>
    public class FakeCakeContext
    {
        public ICakeContext Context { get; }
        public FakeLog Log { get; }
        public DirectoryPath WorkingDirectory { get; }

        public FakeCakeContext()
        {
            var fileSystem = new FileSystem();
            Log = new FakeLog();
            var runtime = new CakeRuntime();
            var platform = new FakePlatform(PlatformFamily.Windows);
            var environment = new CakeEnvironment(platform, runtime, Log);
            var globber = new Globber(fileSystem, environment);

            var args = Substitute.For<ICakeArguments>();
            var registry = new WindowsRegistry();

            var dataService = Substitute.For<ICakeDataService>();
            var toolRepository = new ToolRepository(environment);
            var toolResolutionStrategy = new ToolResolutionStrategy(fileSystem, environment, globber, new FakeConfiguration());
            var tools = new ToolLocator(environment, toolRepository, toolResolutionStrategy);
            var processRunner = new ProcessRunner(fileSystem, environment, Log, tools, new FakeConfiguration());
            Context = new CakeContext(fileSystem, environment, globber, Log, args, processRunner, registry, tools, dataService);

            WorkingDirectory = new DirectoryPath(
                System.IO.Path.GetFullPath(AppContext.BaseDirectory));
            Context.Environment.WorkingDirectory = WorkingDirectory;
        }

        public static FakeCakeContext CreateFakeContext(string workingDirectory = "./test-file-system")
        {
            var cakeCtx = new FakeCakeContext();
            var testFSPath = new DirectoryPath(workingDirectory);
            var testFSDir = cakeCtx.Context.FileSystem.GetDirectory(testFSPath);

            if(testFSDir.Exists)
            {
                testFSDir.Delete(true);
            }

            testFSDir.Create();

            return cakeCtx;
        }
    }
}
