module Helper

open Model

let countRemainingTask (todoList: Todo list) : int =
    todoList
    |> List.filter (fun x -> not x.IsComplete)
    |> List.length