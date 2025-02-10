using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;

namespace ETAMP.Compress.Benchmark;

internal class Program
{
    private static void Main(string[] args)
    {
        var config = new MultiRuntimeConfig()
            .AddLogger(ConsoleLogger.Default)
            .AddColumnProvider(DefaultColumnProviders.Instance);
        BenchmarkRunner.Run<CompressBench>(config);
    }
}

public class MultiRuntimeConfig : ManualConfig
{
    public MultiRuntimeConfig()
    {
        AddJob(Job.Default.WithRuntime(CoreRuntime.Core80).WithId(".NET 8"));
        AddJob(Job.Default.WithRuntime(CoreRuntime.Core90).WithId(".NET 9"));
    }
}