open System.IO
open System.Text.RegularExpressions

type pwinfo = {
    minc: int
    maxc: int
    ch: char
    passwd: string
}

// Cribbed and tweaked from MSDN. Need to better understand Active Patterns.
let (|PwInfo|) input =
    // Line format is min-max char: passwd
    let m = Regex.Match(input, "^(\d+)-(\d+) ([a-z]): (.*)$")
    List.tail [ for g in m.Groups -> g.Value ]

let lines = [
    use sr = new StreamReader(@"input.txt")
    while not sr.EndOfStream do
        let line = sr.ReadLine()
        // So Active Patterns mean we can use a regex and split a string into seperate vars
        match line with
            | PwInfo [minc; maxc; c; passwd] -> { 
                    minc = minc |> int
                    maxc = maxc |> int
                    ch = c |> char
                    passwd = passwd 
                }
            | _ -> None |> ignore
]

let counts = seq {
    for p in lines do
        let cnt = 
            // Oh nice, a string in F# can be naturally decomposed to a sequence of chars
            p.passwd 
            |> Seq.filter (fun c -> c = p.ch) 
            |> Seq.length
        yield (p, cnt)
}

let checks =  
    counts 
    |> Seq.filter (fun p -> snd(p) <= fst(p).maxc && snd(p) >= fst(p).minc) 
    |> Seq.length

printfn "Matches = %d" checks