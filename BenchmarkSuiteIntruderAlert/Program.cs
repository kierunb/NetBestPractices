using BenchmarkDotNet.Running;

namespace BenchmarkSuiteIntruderAlert;

internal class Program
{
    static void Main(string[] args)
    {
        var _ = BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}
