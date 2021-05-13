module Controller

open Model

let withNewTodo (state: State) : State =
  let newTodo =
    {
      TodoId      = TodoId(System.Guid.NewGuid())
      Description = state.NewTodoDescription
      IsComplete  = false
    }

  {
    state with
      TodoList = [ newTodo ] @ state.TodoList
      NewTodoDescription = ""
  }

let withTodoRemoved (state: State) (todoId: TodoId) : State =
  let newList =
    state.TodoList
    |> List.filter (fun x -> x.TodoId <> todoId)
  { state with TodoList = newList }

let withCompleteStatusChanged (state: State) (todoId: TodoId) : State =
  let updatedList =
    state.TodoList
    |> List.map (
        fun todo ->
          if todo.TodoId = todoId
          then { todo with IsComplete = not todo.IsComplete }
          else todo
      )
  { state with TodoList = updatedList }
