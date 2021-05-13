module App

open Elmish
open Elmish.React
open Feliz
open Model
open View
open Controller

let init() =
  {
    TodoList           = List.empty
    NewTodoDescription = ""
  }


let update (msg: Msg) (state: State): State =
  match msg with
  | AddNewTodo ->
      state |> withNewTodo

  | DescriptionTextboxUpdated description ->
      { state with
          NewTodoDescription = description
      }

  | DeleteTodo todoId ->
      state |> withTodoRemoved <| todoId

  | ToggleCompleteStatus todoId ->
      state |> withCompleteStatusChanged <| todoId


let render (state: State) (dispatch: Msg -> unit) =
  Html.div [
    Html.div [
      prop.classes [Bulma.Container]
      prop.children [
        title
        counter state.TodoList
        renderAddSection state dispatch
        renderTodoList state.TodoList dispatch
      ]
    ]
  ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run