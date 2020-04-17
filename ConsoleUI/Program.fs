open System
open System.Collections.Generic
open CompletionTimeBenchmark

let mutable shouldStop = false

// workaround forward declaration shall be initialized in main()
let mutable mainMenu =
    Map.empty.
        Add(0, ("Exit", (fun () -> shouldStop <- true )))

let mutable CurrentMenu =  ref mainMenu

let hashSetMenu =
    Map.empty.
        Add(0, ("Back", (fun () -> CurrentMenu := mainMenu ) )).
        Add(1, ("Hashset<Tuple>", (fun () -> printfn "not implemented yet"  ) )).
        Add(2, ("Hashset<ValueTuple>", (fun () -> printfn "not implemented yet"  )));;

let minMaxMenu =
    Map.empty.
        Add(0, ("Back", (fun () -> CurrentMenu := mainMenu ) )).
        Add(1, ("Array Min Max with LINQ", (fun () -> printfn "%A " (CTBMinMax.DoBenchmarkMinMaxLinq 1_000_000)  ))).
        Add(2, ("Array Min Max manual", (fun () -> printfn "%A " (CTBMinMax.DoBenchmarkMinMaxManual 1_000_000) )));;

let GenerateMenu (map: Map<int,string * _>)=
    printfn "%s Enter a number for your choice of action: %s" Environment.NewLine Environment.NewLine
    Seq.iteri (fun index (item: KeyValuePair<int, string * _>) -> printfn "  %i: %s" item.Key (fst(item.Value))) map
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