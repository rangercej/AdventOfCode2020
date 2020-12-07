open System.IO

let lines = seq {
    use sr = new StreamReader(@"input.txt")
    while not sr.EndOfStream do
        yield sr.ReadLine() |> int
}

let prod = Seq.allPairs lines lines

let sums = seq {
    for p in prod do
        [ fst(p); snd(p); fst(p) + snd(p) ]
} 

let result = sums |> Seq.find (fun x -> x.[2] = 2020) 

let out = result.[0] * result.[1]

printfn "Product = %A" out