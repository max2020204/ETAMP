using BenchmarkDotNet.Running;

namespace ETAMP.Encryption.Benchmark;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<EncryptionBenchmark>();
    }
}