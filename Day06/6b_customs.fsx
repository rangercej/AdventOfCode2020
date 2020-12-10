#time "on"

// Read each group and merge into a single line.
let lines = [
    use sr = new System.IO.StreamReader(@"input.txt")
    let input = new System.Collections.Generic.List<string>()
    while not sr.EndOfStream do
        let l = sr.ReadLine()
        if l = "" then
            System.String.Join(";", input)
            input.Clear()
        else
            input.Add(l)

    // Add the tail of the file (assuming not empty)
    if input.Count > 0 then
        System.String.Join(";", input)
]

// Each group is a single line, with each person's answers sepearated by ';'. So
// split to get an array of each person's answers, then cast each element of the
// array to a set of individual characters, then call intersectMany.
let findIntersect (s:string) =
    s.Split(';') 
        |> Array.map (fun x -> x.ToCharArray() |> Set.ofArray) 
        |> Set.intersectMany
    
// Find the intersects for each group, count, and sum for the final answer.
let total = 
    lines 
        |> List.map (findIntersect >> Set.count)
        |> Seq.sum

printfn "Total answers where all people in group answered yes to the same question = %d" total