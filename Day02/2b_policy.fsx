#time "on"

open System.IO
open System.Text.RegularExpressions

type pwinfo = {
    pos1: int
    pos2: int
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
            | PwInfo [p1; p2; c; passwd] -> { 
                    pos1 = (p1 |> int) - 1
                    pos2 = (p2 |> int) - 1
                    ch = c |> char
                    passwd = passwd 
                }
            | _ -> None |> ignore
]

let matches = 
    lines 
        |> Seq.where(fun p -> 
                        (p.passwd.[p.pos1] = p.ch || p.passwd.[p.pos2] = p.ch)
                        && p.passwd.[p.pos1] <> p.passwd.[p.pos2]
                        )
        |> Seq.length


printfn "Matches = %d" matches

