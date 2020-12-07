#time "on"

open System.IO

// Read the map. # = tree, . = open space. Map is cylindrical.
let lines = [
    use sr = new StreamReader(@"input.txt")
    while not sr.EndOfStream do
        sr.ReadLine()
]

let mapWidth = lines.[0].Length

// The slope we follow is 3 across, one down, so create X and Y co-ords, wrapped round the map
let y = [ 0 .. (lines.Length - 1) ]
let x = [ 0 .. (lines.Length) - 1 ] |> List.map(fun n -> (n * 3) % mapWidth)

let coords = List.zip x y

let treeCount =
    coords |> List.where(fun c -> lines.[snd(c)].[fst(c)] = '#') |> List.length

printfn "Tree count = %d" treeCount
