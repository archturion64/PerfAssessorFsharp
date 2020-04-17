namespace CompletionTimeBenchmark

open System
open System.Linq


module CTBMinMax =
    let GenerateArray size = 
        let rnd = Random()
        Array.init size (fun _ -> rnd.Next (0, size))

    let BenchmarkMinMaxLinq size = 
        let array = GenerateArray size;
        let max = array.Max();
        let min = array.Min();
        printf "%d %d " min max

    let BenchmarkMinMaxManual size = 
        let array = GenerateArray size;
        let mutable max = 0
        let mutable min = 0
        for i in 0 .. array.Length - 1 do
            if max < (array.[i]) then 
                max <- array.[i]
            if min > (array.[i]) then 
                min <- array.[i]
        printf " %d %d " min max

    let DoBenchmarkMinMaxLinq size =
        let functionToBench = fun () -> BenchmarkMinMaxLinq size
        CTBenchmark.RunBenchmark <| functionToBench

    let DoBenchmarkMinMaxManual size =
        let functionToBench = fun () -> BenchmarkMinMaxManual size
        CTBenchmark.RunBenchmark <| functionToBench
