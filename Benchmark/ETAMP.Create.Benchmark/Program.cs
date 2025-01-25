#region

using BenchmarkDotNet.Running;

#endregion

namespace ETAMP.Create.Benchmark;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<ETAMPBenchmark>();
    }
}