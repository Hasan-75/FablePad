module View

open Feliz
open Model
open Zanaptak.TypedCssClasses
open Helper

type Bulma = CssClasses<"https://cdn.jsdelivr.net/npm/bulma@0.9.2/css/bulma.min.css", Naming.PascalCase>

let title =
    Html.div [
        prop.classes [ Bulma.Title; Bulma.Is1; Bulma.Columns; Bulma.IsCentered ]
        prop.children [
            Html.div [
                prop.classes [ Bulma.M5 ]
                prop.children [
                    Html.h1 "Fable Pad"
                ]
            ]
        ]
    ]

let counter (todoList: Todo list) : ReactElement =
    Html.div [
        prop.classes [ Bulma.Title; Bulma.Is6; Bulma.Columns ]
        prop.children [
            Html.div [
                prop.classes [ Bulma.M5 ]
                prop.children [
                    Html.p ( sprintf "Total Task %i" todoList.Length )
                    Html.p (
                        todoList
                        |> countRemainingTask
                        |>sprintf "Remaining Task %i"
                    )
                ]
            ]
        ]
    ]



let generateButton (text: string) (msg: Msg) (dispatch: Msg -> unit) (classes: string list) : ReactElement =
  Html.button [
    prop.classes (classes @ [Bulma.Button])
    prop.style [
      style.margin 10
    ]
    prop.children [
      Html.p text
    ]
    prop.onClick( fun _ -> msg |> dispatch )
  ]

let createColumn (child: ReactElement) : ReactElement =
    Html.div [
      prop.classes [ Bulma.Column; Bulma.IsHalf]
      prop.children [
        child
      ]
    ]


let statusColor (todo: Todo) : string =
  if todo.IsComplete then color.green
  else color.red

let statusTag (todo: Todo) : ReactElement =
    let classes = if todo.IsComplete then [ Bulma.IsSuccess ] else [Bulma.IsDanger]
    let text = if todo.IsComplete then "Done" else "Incomplete"
    Html.span [
      prop.style [ style.floatStyle.right ]
      prop.classes ( [ "tag" ] @ classes )
      prop.children [
          Html.text text
      ]
    ]



let renderSingleTodoItem (todo: Todo) (dispatch: Msg -> unit) : ReactElement =
  Html.div [
      prop.classes [ Bulma.Columns; Bulma.IsCentered ]
      prop.children [
          Html.div [
                prop.classes [Bulma.Card]
                prop.style [
                  style.padding 10
                  style.border (3, borderStyle.dashed, todo |> statusColor)
                ]
                prop.children [
                  Html.div [
                      prop.children [
                          Html.p [
                              prop.children [ Html.text todo.Description ]
                              prop.classes [Bulma.Title; Bulma.Is3]
                          ]
                          statusTag todo
                    ]
                  ]
                  Html.div [
                      prop.classes [ Bulma.IsRight ]
                      prop.children [
                          generateButton "Delete" (todo.TodoId |> DeleteTodo) dispatch <| List.Empty
                          generateButton "Mark Done" (todo.TodoId |> ToggleCompleteStatus) dispatch <| List.empty
                      ]
                  ]
                ]
              ]
              |> createColumn
            ]
  ]

let renderTodoList (todoList: Todo list) (dispatch: Msg -> unit) : ReactElement =
  Html.div [
      prop.classes [  ]
      prop.children [
          Html.div [
              prop.classes [  ]
              prop.children [
                  Html.div [
                      prop.classes [ ]
                      prop.children [
                          Html.ul[
                            prop.children [ for todo in todoList do ( renderSingleTodoItem todo dispatch ) ]
                          ]
                      ]
                  ]
              ]
          ]
      ]
  ]


let renderNewTodoTextBox (description: string) (dispatch: Msg -> unit) : ReactElement =
  Html.input[
    prop.classes [ Bulma.Input ]
    prop.type' "text"
    prop.placeholder "Description"
    prop.value description
    prop.onTextChange( DescriptionTextboxUpdated >> dispatch )
    prop.onKeyUp (
        fun evt ->
            if evt.keyCode = 13.0
            then AddNewTodo |> dispatch
    )
  ]

let renderAddButton (dispatch: Msg -> unit) : ReactElement =
    Html.button [
        prop.classes [ Bulma.Button; Bulma.IsSuccess ]
        prop.children [
          Html.p "Add New"
        ]
        prop.onClick( fun _ -> AddNewTodo |> dispatch )
    ]



let renderAddSection (state: State) (dispatch: Msg -> unit) : ReactElement =
    Html.div [
        prop.classes [ Bulma.Columns; Bulma.IsCentered]
        prop.children [
            Html.div [
                prop.classes [ Bulma.Field; Bulma.HasAddons; Bulma.IsHalf ]
                prop.children [
                    Html.div [
                        prop.classes [ Bulma.Control; Bulma.IsExpanded]
                        prop.children [renderNewTodoTextBox state.NewTodoDescription dispatch ]
                    ]
                    Html.div [
                        prop.classes [ Bulma.Control; Bulma.HasIconsRight ]
                        prop.children [
                            renderAddButton dispatch
                        ]
                    ]
                ]
            ]
            |> createColumn
        ]
    ]

