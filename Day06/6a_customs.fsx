#time "on"

// Read each group and merge into a single line.
let lines = [
    use sr = new System.IO.StreamReader(@"input.txt")
    let input = new System.Collections.Generic.List<string>()
    while not sr.EndOfStream do
        let l = sr.ReadLine()
        if l = "" then
            System.String.Join ("", input)
            input.Clear()
        else
            input.Add(l)

    // Add the tail of the file (assuming not empty)
    if input.Count > 0 then
        System.String.Join ("", input)
]

let countUnique inp =
    inp |> Seq.distinct |> Seq.length

// F# linter suggested sumBy to replace the 'map |> sum' pipe.
let distinct = lines |> Seq.sumBy countUnique

printfn "Total of distinct answers: %d" distinct