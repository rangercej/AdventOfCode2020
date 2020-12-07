open System.IO

let lines = [
    use sr = new StreamReader(@"C:\Users\chris\source\repos\AdventOfCode\2020\1a\input.txt")
    while not sr.EndOfStream do
        sr.ReadLine() |> int
]

let prod = seq {
    for x in lines do
        for y in lines do
            for z in lines do
                yield [x;y;z]
}

let sums = seq {
    for p in prod do
        [ p.[0]; p.[1]; p.[2]; p.[0] + p.[1] + p.[2] ]
} 

let result = sums |> Seq.where (fun x -> x.[3] = 2020) 

result |> Seq.map (fun x -> x.[0] * x.[1] * x.[2]) |> Seq.toList