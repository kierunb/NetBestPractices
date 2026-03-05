using BenchmarkDotNet.Attributes;
using Microsoft.VSDiagnostics;
using System;

namespace BenchmarkSuiteIntruderAlert;

[CPUUsageDiagnoser]
[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 1, iterationCount: 10)]
public class CsvBench
{
    private const string Line = "42,John Doe,acme@example.com,19995";
    private static readonly string[]? _ = null; // placeholder to avoid warnings

    [Benchmark(Baseline = true)]
    public (int id, string name, string email, decimal price) Split()
    {
        var parts = Line.Split(','); // allocates array + substrings
        return (int.Parse(parts[0]), parts[1], parts[2], decimal.Parse(parts[3]));
    }

    [Benchmark]
    public (int id, string name, string email, decimal price) SpanParse()
    {
        ReadOnlySpan<char> s = Line.AsSpan();
        var id = int.Parse(Next(ref s, ','));
        var name = Next(ref s, ',').ToString();
        var email = Next(ref s, ',').ToString();
        var price = decimal.Parse(s);
        return (id, name, email, price);

        static ReadOnlySpan<char> Next(ref ReadOnlySpan<char> span, char sep)
        {
            var idx = span.IndexOf(sep);
            if (idx < 0)
            {
                var last = span; span = ReadOnlySpan<char>.Empty; return last;
            }
            var token = span.Slice(0, idx);
            span = span.Slice(idx + 1);
            return token;
        }
    }
}
