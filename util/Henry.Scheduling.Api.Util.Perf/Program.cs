using BenchmarkDotNet.Running;
using Henry.Scheduling.Api.Util.Perf.Application;

namespace Henry.Scheduling.Api.Util.Perf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SlotGeneratorBenchmarker>();
            Console.WriteLine("Hello, World!");
        }
    }
}
