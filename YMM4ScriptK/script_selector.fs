namespace YMM4ScriptK

open System
open System.Windows
open System.Windows.Controls
open System.Windows.Controls.Primitives
open System.Windows.Data
open YukkuriMovieMaker.Commons
open YukkuriMovieMaker.Views.Converters

type internal ScriptSelectorControl() as this =
    inherit UserControl()

    static let valueProperty =
        DependencyProperty.Register(
            "Value",
            typeof<string>,
            typeof<ScriptSelectorControl>,
            FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
            )
        )

    let combo = new ComboBox(Height = 26.0, MinWidth = 120.0)
    let beginEditEvent = new Event<EventHandler, EventArgs>()
    let endEditEvent = new Event<EventHandler, EventArgs>()

    do
        this.Content <- combo

        let b = new Binding("Value")
        b.Source <- this
        b.Mode <- BindingMode.TwoWay
        combo.SetBinding(Selector.SelectedItemProperty, b) |> ignore

        combo.SelectionChanged.Add(fun _ ->
            beginEditEvent.Trigger(this, EventArgs.Empty)
            endEditEvent.Trigger(this, EventArgs.Empty)
        )

    static member val ValueProperty: DependencyProperty = valueProperty

    member this.Value
        with get() = this.GetValue(ScriptSelectorControl.ValueProperty) :?> string
        and set(v: string) = this.SetValue(ScriptSelectorControl.ValueProperty, box v)

    member this.ItemsSource
        with get() = combo.ItemsSource
        and set(v) = combo.ItemsSource <- v

    [<CLIEvent>]
    member _.BeginEdit = beginEditEvent.Publish

    [<CLIEvent>]
    member _.EndEdit = endEditEvent.Publish

    interface IPropertyEditorControl with
        [<CLIEvent>]
        member _.BeginEdit = beginEditEvent.Publish

        [<CLIEvent>]
        member _.EndEdit = endEditEvent.Publish

type internal ScriptSelectorAttribute() =
    inherit PropertyEditorAttribute2()

    override _.Create() : FrameworkElement =
        let c = new ScriptSelectorControl()
        c.ItemsSource <- ScriptRegistry.getDisplayNames()
        c :> FrameworkElement

    override _.SetBindings(control: FrameworkElement, itemProperties: ItemProperty[]) =
        let editor = control :?> ScriptSelectorControl
        editor.ItemsSource <- ScriptRegistry.getDisplayNames()
        editor.SetBinding(ScriptSelectorControl.ValueProperty, ItemPropertiesBinding.Create(itemProperties))
        |> ignore

    override _.ClearBindings(control: FrameworkElement) =
        let editor = control :?> ScriptSelectorControl
        BindingOperations.ClearBinding(editor, ScriptSelectorControl.ValueProperty)
