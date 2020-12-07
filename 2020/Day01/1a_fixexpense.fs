open System.IO

let lines = seq {
    use sr = new StreamReader(@"C:\Users\chris\source\repos\AdventOfCode\2020\1a\input.txt")
    while not sr.EndOfStream do
        yield sr.ReadLine() |> int
}

let prod = Seq.allPairs lines lines

let sums = seq {
    for p in prod do
        [ fst(p); snd(p); fst(p) + snd(p) ]
} 

let result = sums |> Seq.where (fun x -> x.[2] = 2020) 

result |> Seq.map (fun x -> x.[0] * x.[1]) |> Seq.toList