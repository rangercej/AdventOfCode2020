#time "on"

// Read each group into an array and add to the list.
let lines = [
    use sr = new System.IO.StreamReader(@"input.txt")
    let input = new System.Collections.Generic.List<char[]>()
    while not sr.EndOfStream do
        let l = sr.ReadLine()
        if l = "" then
            input |> Array.ofSeq
            input.Clear()
        else
            input.Add(l.ToCharArray())

    // Add the tail of the file (assuming not empty)
    if input.Count > 0 then
        input |> Array.ofSeq
]

// For each string in the array, turn it into a set of chars so we
// have an array of sets. Then call intersectMany across them all.
let findIntersect (answers:array<char[]>) =
    answers 
        |> Array.map Set.ofArray 
        |> Set.intersectMany
    
// Find the intersects for each group, count, and sum for the final answer.
let total = 
    lines 
        |> List.map (findIntersect >> Set.count)
        |> Seq.sum

printfn "Total answers where all people in group answered yes to the same question = %d" total