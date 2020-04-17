namespace CompletionTimeBenchmark

open System
open System.Linq


module CTBMinMax =
    let GenerateArray size (rnd:Random) = 
        Array.init size (fun _ -> rnd.Next (0, size))

    let BenchmarkMinMaxLinq size = 
        let rnd = Random()
        let array = GenerateArray size rnd;
        let max = array.Max();
        let min = array.Min();
        printf "%d %d " min max

    let BenchmarkMinMaxManual size = 
        let rnd = Random()
        let array = GenerateArray size rnd;
        let mutable max = 0
        let mutable min = 0
        for i in 0 .. array.Length - 1 do
            if max < (array.[i]) then 
                max <- array.[i]
            if min > (array.[i]) then 
                min <- array.[i]
        printf " %d %d " min max

    let DoBenchmarkMinMaxLinq size : float =
        let functionToBench = fun () -> BenchmarkMinMaxLinq size
        CTBenchmark.RunBenchmark <| functionToBench

    let DoBenchmarkMinMaxManual size : float =
        let functionToBench = fun () -> BenchmarkMinMaxManual size
        CTBenchmark.RunBenchmark <| functionToBench
