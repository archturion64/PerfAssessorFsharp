open System
open System.Collections.Generic
open CompletionTimeBenchmark

let mutable shouldStop = false

let containerSize = 1_000_000

// workaround forward declaration shall be initialized in main()
let mutable mainMenu =
    Map.empty.
        Add(0, ("Exit", (fun () -> shouldStop <- true )))

let mutable CurrentMenu =  ref mainMenu

let ReportResult testName test containerSize =
    printfn "Bench %s complete in: %f ms" testName (test containerSize)
 
let RunAllEntryesAsync (map: Map<int,string * (unit -> unit)>) = 
    map
    |> Seq.filter (fun x -> x.Key > 0 && x.Key < Seq.length map - 1) // skip first and last manue entry
    |> Seq.map (fun (x: KeyValuePair<int,string * (unit -> unit)>) -> async {snd(x.Value)()})
    |> Async.Parallel
    |> Async.RunSynchronously |> ignore
    printfn " ms workflow started"

let hashSetMenu =
    Map.empty.
        Add(0, ("Back", (fun () -> CurrentMenu := mainMenu ) )).
        Add(1, ("Hashset<Tuple>", (fun () -> ReportResult "Hashset<Tuple>" CTBHashSet.DoBenchmarkHashSetTuple containerSize  ) )).
        Add(2, ("Hashset<ValueTuple>", (fun () -> ReportResult  "Hashset<ValueTuple>" CTBHashSet.DoBenchmarkHashSetValueTuple containerSize ))).
        Add(3, ("Run all async", (fun () ->  RunAllEntryesAsync  !CurrentMenu)));;

let minMaxMenu =
    Map.empty.
        Add(0, ("Back", (fun () -> CurrentMenu := mainMenu ) )).
        Add(1, ("Array Min Max with LINQ", (fun () -> ReportResult "Array Min Max with LINQ" CTBMinMax.DoBenchmarkMinMaxLinq containerSize ))).
        Add(2, ("Array Min Max manual", (fun () -> ReportResult "Array Min Max manual" CTBMinMax.DoBenchmarkMinMaxManual containerSize ))).
        Add(3, ("Run all async", (fun () ->  RunAllEntryesAsync  !CurrentMenu)));;

let GenerateMenu (map: Map<int,string * _>)=
    printfn "%s Enter a number for your choice of action: %s" Environment.NewLine Environment.NewLine
    Seq.iter (fun (item: KeyValuePair<int, string * _>) -> printfn "  %i: %s" item.Key (fst(item.Value))) map
    printfn "%s" Environment.NewLine

let TriggerSelection (selection :int) (map : Map<int,string * (unit-> unit)>) = 
    let found = map.TryFind selection
    match found with
        | Some (x : string * (unit -> unit)) ->  (snd(x))()
        | None -> printfn "Invalid input"

let ReactOnInput (map: Map<int,string * (unit -> unit)>) = 
    match Console.ReadLine () |> System.Int32.TryParse with
        | true, out ->  TriggerSelection out map
        | false, _ -> printfn "Invalid input"

[<EntryPoint>]
let main argv =

    // workaround for cyclic dependancy of data containers
    mainMenu <- mainMenu.Add(1, ("HashSet", (fun () -> CurrentMenu := hashSetMenu)))
    mainMenu <- mainMenu.Add(2, ("MinMax", (fun () -> CurrentMenu := minMaxMenu )))

    CurrentMenu := mainMenu

    while not shouldStop do
        !CurrentMenu |> GenerateMenu
        ReactOnInput !CurrentMenu
    0 // return an integer exit code