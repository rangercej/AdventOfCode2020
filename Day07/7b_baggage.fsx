#time "on"

open System.Text.RegularExpressions

type BagDef = {
    OuterBagColour: string;
    InnerBagCount: int;
    InnerBagColour: string
}

let lines = [
    use sr = new System.IO.StreamReader(@"test.txt")
    while not sr.EndOfStream do
        sr.ReadLine()
]

let (|InnerOuterBags|) input =
    // Line format is something like:
    //   {adjective} {colour} "bags" contain {val} {adjective} {colour} "bags" [, ...]
    let m = Regex.Match(input, "^(\S+ \S+) bags contain (.*)\.$")
    List.tail [ for g in m.Groups -> g.Value ]

let (|InnerBagColour|) input =
    // Extract the modifier+colour from the string "{val} {adjective} {colour} bags".
    // Note that 'bags' is singular for one bag, so optional s.
    let m = Regex.Match(input, "^(\d+) (\S+ \S+) bags?$")
    List.tail [ for g in m.Groups -> g.Value ]

let processContainedDef (s:string, outerCol:string) =
    match s with
        | InnerBagColour [count;col] -> { InnerBagCount = count |> int; InnerBagColour = col; OuterBagColour = outerCol }
        | _ -> { InnerBagCount = 0; InnerBagColour = ""; OuterBagColour = ""}
    
// Input should be a CSV string of "{val} {adjective} {colour} bags", so split
// on commas and process each bag definition.
let processInner (innerDef:string, outerCol:string) = 
    innerDef.Split(',')
        |> Array.map (fun s -> s.Trim())
        |> Array.map (fun s -> processContainedDef (s, outerCol))
        |> Array.filter (fun s -> s.InnerBagColour <> "")

let processLine (s:string) =
    match s with
    | InnerOuterBags [outerBag; innerDef] -> processInner (innerDef, outerBag)
    | _ -> [||]

// Line format is something like:
//   {adjective} {colour} "bags" contain {val} {adjective} {colour} "bags" [, ...]
// Process this into a structure breaking out inner, outer and quantity.
let bags = lines |> List.map processLine |> Array.concat |> List.ofArray

// Hurrah! Now we can search for how many bags a shiny gold bag can contain
let rec findBags (bagColour:list<string>, currentBags:list<BagDef>) =
    let bagsContainedInColour = bags |> List.where (fun ele -> bagColour |> List.contains (ele.OuterBagColour))
    if bagsContainedInColour.Length = 0 then
        currentBags
    else
        let nextOuterBags = bagsContainedInColour |> List.map(fun x -> x.InnerBagColour)
        let nextBags = currentBags @ bagsContainedInColour
        findBags (nextOuterBags, nextBags)

let possibleBags = findBags (["shiny gold"], [])

let bagCount = possibleBags |> List.map(fun x -> x.InnerBagCount) |> List.sum

//printfn "A shiny gold bag could be in any of %d bags" bagCount

// 293 too low