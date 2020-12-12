#time "on"

open System.Text.RegularExpressions

let lines = [
    use sr = new System.IO.StreamReader(@"input.txt")
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
    let m = Regex.Match(input, "^\d+ (\S+ \S+) bags?$")
    List.tail [ for g in m.Groups -> g.Value ]

let processContainedDef (s:string) =
    match s with
        | InnerBagColour [col] -> col
        | _ -> ""
    
// Input should be a CSV string of "{val} {adjective} {colour} bags", so split
// on commas and process each bag definition.
let processInner (def:string) = 
    def.Split(',')
        |> Array.map ((fun s -> s.Trim()) >> processContainedDef)
        |> Array.filter (fun s -> s <> "")

let processLine (s:string) =
    match s with
        | InnerOuterBags [outerBag; innerDef] -> outerBag, processInner innerDef
        | _ -> "", [||]

// Line format is something like:
//   {adjective} {colour} "bags" contain {val} {adjective} {colour} "bags" [, ...]
// Process this into a simple array of outside colour -> inside colours
let bags = lines |> List.map processLine

// Now we have something that resembles bags that contain bags, can we turn it
// inside out to say that a bag of colour {x} is contained with in colour {y}.
// So we map over all bags, getting the array that contains the inner bags, and
// map over that to create a list of tuples that are (inner, outer).
let inverseBags =
    bags 
        |> List.map (fun def -> (snd(def) 
                                |> Array.map (fun inner -> inner,fst(def))))
        |> List.filter (fun arr -> arr.Length > 0)
        // At this point we have a list of arrays of tuples. We need to flatten it.
        |> Array.concat
        |> List.ofArray

// Hurrah! Now we can search for how many ways a "shiny gold" bag can be stored.
let rec findBags (bagColour:string list, currentBags:string list) =
    let bagsContainingColour = inverseBags |> List.where (fun ele -> bagColour |> List.contains (fst(ele)))
    if bagsContainingColour.Length = 0 then
        currentBags
    else
        let outerBags = bagsContainingColour |> List.map(fun x -> snd(x))
        let newCurrent = currentBags @ outerBags
        findBags (outerBags, newCurrent)

let possibleBags = findBags (["shiny gold"], [])

let bagCount = possibleBags |> List.distinct |> List.length

printfn "A shiny gold bag could be in any of %d bags" bagCount
