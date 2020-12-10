let s = "FBFF"

let rec thing (x:int) =
    if x > 100 then
        x
    else
        thing (x * 2)

thing 3
       