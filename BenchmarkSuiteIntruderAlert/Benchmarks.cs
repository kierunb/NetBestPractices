using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.VSDiagnostics;

namespace BenchmarkSuiteIntruderAlert;
// For more information on the VS BenchmarkDotNet Diagnosers see https://learn.microsoft.com/visualstudio/profiling/profiling-with-benchmark-dotnet
[CPUUsageDiagnoser]
[MemoryDiagnoser]
[SimpleJob(iterationCount: 20)]
public class Benchmarks
{

    [Benchmark]
    public void Starter()
    {
        var simulation = new IntruderAlertStarter.Simulation();
        simulation.Start();

    }

    [Benchmark]
    public void Optimized()
    {
        var simulation = new IntruderAlertOptimized.Simulation();
        simulation.Start();

    }
    
}
