#time "on"

open System.IO
open System.Collections.Generic
open System.Text.RegularExpressions

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

let validateYear (s:string, min:int, max:int) =
    if (Regex.IsMatch(s, "^\d+$")) then
        let n = s |> int
        n >= min && n <= max
    else
        false

let validateHeight (s:string) =
    if (Regex.IsMatch(s, "^\d+cm$")) then
        let hgt = s.Substring(0, s.Length - 2) |> int
        hgt >= 150 && hgt <= 193
    elif (Regex.IsMatch(s, "^\d+in$")) then
        let hgt = s.Substring(0, s.Length - 2) |> int
        hgt >= 59 && hgt <= 76
    else
        false

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
    |> List.where (fun pp ->
                    validateYear (pp.["byr"], 1920, 2002)
                    && validateYear (pp.["iyr"], 2010, 2020)
                    && validateYear (pp.["eyr"], 2020, 2030)
                    && validateHeight pp.["hgt"]
                    && Regex.IsMatch(pp.["hcl"], "^#[0-9a-f]{6}$")
                    && (List.contains pp.["ecl"] [ "amb";"blu";"brn";"gry";"grn";"hzl";"oth" ])
                    && Regex.IsMatch(pp.["pid"], "^\d{9}$")
    )
    |> List.length

printfn "Valid passport count = %d" validCount