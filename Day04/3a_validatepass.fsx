#time "on"

open System.IO
open System.Collections.Generic

// Read file, and merge the multi-line blocks into single lines
let lines = [
    use sr = new StreamReader(@"input.txt")
    let input = new List<string>()
    while not sr.EndOfStream do
        let l = sr.ReadLine()
        if l = "" then
            System.String.Join(" ", input)
            input.Clear()
        else
            input.Add(l)
]

// Input will be 'key:val key:val key:val'
let makedict (s: string) =
    s.Split(' ')
    |> Array.map ((fun part -> part.Split(':')) >> (fun kv -> kv.[0], kv.[1]))
    |> dict

let passports = lines |> List.map makedict

// Now validate passport properties ad count results
let validCount = 
    passports 
    |> List.where (fun pp -> pp.Keys.Count >= 7)
    |> List.where (fun pp -> 
                    pp.ContainsKey("byr") 
                    && pp.ContainsKey("iyr") 
                    && pp.ContainsKey("eyr") 
                    && pp.ContainsKey("hgt") 
                    && pp.ContainsKey("hcl") 
                    && pp.ContainsKey("ecl") 
                    && pp.ContainsKey("pid"))
    |> List.where (fun pp -> pp.Keys.Count = 7 || pp.Keys.Count = 8 && pp.ContainsKey("cid"))
    |> List.length

printfn "Valid passport count = %d" validCount