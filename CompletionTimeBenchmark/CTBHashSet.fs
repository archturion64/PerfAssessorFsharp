namespace CompletionTimeBenchmark

open System
open System.Linq
open System.Collections.Generic

module CTBHashSet =
    let GenerateHashSetTuple size (rng:Random) =
        let mySet = new HashSet<(string * float)>()
        for i:int in 0 .. size - 1 do
            mySet.Add( (i.ToString() , rng.NextDouble())) |> ignore
        mySet
    
    // workaround for missing ValueTuple in the standard
    type Val = struct
        val first : string
        val second : float
        new (f: string, s: float) = { first = f; second = s; }
    end

    let GenerateHashSetValueTuple size (rng:Random) =
        let mySet  = new HashSet<Val>()
        for i:int in 0 .. size - 1 do
            mySet.Add( Val(i.ToString(), rng.NextDouble()) ) |> ignore
        mySet

    let BenchmarkHashSetTuple size = 
        let rnd = Random()
        let mutable sum = 0.0
        let mySet = GenerateHashSetTuple size rnd;
        printf "%f" (Seq.sumBy (function item -> snd(item)) mySet)

    let BenchmarkHashSetValueTuple size = 
        let rnd = Random()
        let mutable sum = 0.0
        let mySet = GenerateHashSetValueTuple size rnd;
        printf "%f" (Seq.sumBy (function (item:Val) -> item.second) mySet)

    let DoBenchmarkHashSetTuple size : float=
        let functionToBench = fun () -> BenchmarkHashSetTuple size
        CTBenchmark.RunBenchmark <| functionToBench

    let DoBenchmarkHashSetValueTuple size : float =
        let functionToBench = fun () -> BenchmarkHashSetValueTuple size
        CTBenchmark.RunBenchmark <| functionToBench