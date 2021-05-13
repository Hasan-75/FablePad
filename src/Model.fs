module Model

type TodoId = TodoId of System.Guid

type Todo =
  {
    TodoId:      TodoId
    Description: string
    IsComplete:  bool
  }

type State =
  {
    TodoList:           Todo list
    NewTodoDescription: string
  }

type Msg =
  | AddNewTodo
  | DescriptionTextboxUpdated of string
  | DeleteTodo of TodoId
  | ToggleCompleteStatus of TodoId