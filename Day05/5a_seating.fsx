#time "on"

let lines = [
    use sr = new System.IO.StreamReader(@"input.txt")
    while not sr.EndOfStream do
        sr.ReadLine()
]

let rec findNumber (s:list<char>, lower:int, upper:int, fwd:char, bck:char) =
    if s.Length = 0 then
        (lower + upper) / 2
    else
        let dir = s.[0]
        let searchSize = 1 + upper - lower 
        let halfWay = searchSize / 2
        if dir = fwd then
            findNumber (s |> List.tail, lower, upper - halfWay, fwd, bck)
        elif dir = bck then
            findNumber ((s |> List.tail), lower + halfWay, upper, fwd, bck)
        else
            -1
        
let getRowMap ptr =
    let seatPtrs = ptr |> Seq.take 7 |> Seq.toList
    findNumber (seatPtrs, 0, 127, 'F', 'B')

let getSeatMap ptr =
    let seatPtrs = ptr |> Seq.skip 7 |> Seq.take 3 |> Seq.toList
    findNumber (seatPtrs, 0, 7, 'L', 'R')

let seatLoc = lines |> List.map (fun s -> getRowMap s, getSeatMap s)
let seatId = seatLoc |> List.map(fun x -> (fst(x) * 8) + snd(x)) |> List.max

printfn "Highest seat ID = %d" seatId