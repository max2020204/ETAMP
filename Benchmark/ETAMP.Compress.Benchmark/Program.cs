using BenchmarkDotNet.Running;

namespace ETAMP.Compress.Benchmark;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<CompressBench>();
    }
}