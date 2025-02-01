#region

using BenchmarkDotNet.Running;

#endregion

namespace ETAMP.Sign.Benchmark;

class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<ETAMPSignBenchmark>();
    }
}