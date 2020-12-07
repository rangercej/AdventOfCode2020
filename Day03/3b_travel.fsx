#time "on"

open System.IO

// Read the map. # = tree, . = open space. Map is cylindrical.
let lines = [
    use sr = new StreamReader(@"input.txt")
    while not sr.EndOfStream do
        sr.ReadLine()
]

let mapWidth = lines.[0].Length

let slopes = [(1,1); (3,1); (5,1); (7,1); (1,2)]

// Count trees on each slope
let trees = [
    for slope in slopes do
        let right = fst(slope)
        let down = snd(slope)

        let y = [ 0 .. down .. (lines.Length - 1) ]
        
        // We want as many {x} co-ordinates as there are {y} co-ordinates.
        let x = [ 0 .. (y.Length) - 1 ] |> List.map(fun n -> (n * right) % mapWidth)

        let coords = List.zip x y
        coords |> List.where(fun c -> lines.[snd(c)].[fst(c)] = '#') |> List.length |> int64
]

let treeProduct = List.fold (fun acc elem -> acc * elem) 1L trees

printfn "Tree product = %d" treeProduct
