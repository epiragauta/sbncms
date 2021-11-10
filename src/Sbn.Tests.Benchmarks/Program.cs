using BenchmarkDotNet.Running;

namespace Sbn.Tests.Benchmarks
{
    internal static class Program
    {
        private static void Main(string[] args) => new BenchmarkSwitcher(typeof(Program).Assembly).Run(args);
    }
}
