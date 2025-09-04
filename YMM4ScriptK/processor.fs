namespace YMM4ScriptK

open System
open Vortice.Direct2D1
open YukkuriMovieMaker.Commons
open YukkuriMovieMaker.Player.Video

type internal YMM4ScriptK_Processor(devices: IGraphicsDevicesAndContext, _owner: obj) =

    let effect = new Vortice.Direct2D1.Effects.Opacity(devices.DeviceContext)
    let outputImage: ID2D1Image = effect.Output

    member val Output: ID2D1Image = outputImage with get

    member this.SetInput(input: ID2D1Image option) =
        match input with
        | Some img -> effect.SetInput(0, img, true)
        | None -> effect.SetInput(0, null, true)

    member this.ClearInput() =
        effect.SetInput(0, null, true)

    member this.Update(effectDescription: EffectDescription) =
        effectDescription.DrawDescription

    interface IVideoEffectProcessor with
        member this.Output = this.Output
        member this.SetInput(input) = this.SetInput(Option.ofObj input)
        member this.ClearInput() = this.ClearInput()
        member this.Update(effectDescription) = this.Update(effectDescription)

    interface IDisposable with
        member this.Dispose() =
            effect.SetInput(0, null, true)
            this.Output.Dispose()
            effect.Dispose()
