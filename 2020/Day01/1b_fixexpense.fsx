open System.IO

let lines = [
    use sr = new StreamReader(@"input.txt")
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

let result = sums |> Seq.find (fun x -> x.[3] = 2020) 

let out = result.[0] * result.[1] * result.[2]

printfn "Product = %A" out