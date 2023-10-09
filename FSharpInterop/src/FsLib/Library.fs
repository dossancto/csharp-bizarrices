namespace FsLib

module Say =
    let Hello name =
        printfn "Hello %s" name

module Continha = 
  let Abs n = if n < 0 then -n else n

  let rec Factorial n = 
    match n with 
    | n when abs n = 1 -> n
    | n when n < 0 -> abs n * Factorial(n + 1)
    | n -> n * Factorial(n - 1)

