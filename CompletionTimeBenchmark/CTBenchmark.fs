namespace CompletionTimeBenchmark
open System


module CTBenchmark =
    let testIterations = 10

    let RunBenchmark workload =
        Console.Write "["

        let results = [| for i in 1 .. testIterations -> printf " | "; CTBStopWatch.MeasureInMs workload; |]

        Console.WriteLine(" | ]");
        Array.average results

