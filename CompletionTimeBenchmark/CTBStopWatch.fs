namespace CompletionTimeBenchmark

module CTBStopWatch = 
    let MeasureInMs workload = 
        let stopWatch = System.Diagnostics.Stopwatch.StartNew()
        workload()
        stopWatch.Stop()
        stopWatch.Elapsed.TotalMilliseconds
        