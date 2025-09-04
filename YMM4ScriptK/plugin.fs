namespace YMM4ScriptK

open System.Collections.Generic
open System.ComponentModel.DataAnnotations
open YukkuriMovieMaker.Commons
open YukkuriMovieMaker.Exo
open YukkuriMovieMaker.Player.Video
open YukkuriMovieMaker.Plugin.Effects

[<VideoEffect("YMMScriptK", [|"いろいろ"|], [||])>]
type public YMM4ScriptK() =
    inherit VideoEffectBase()

    override this.Label = "YMM4ScriptK"

    [<Display(GroupName = "スクリプト", Name = "スクリプト")>]
    [<ScriptSelector>]
    member val SelectedScript: string = "" with get, set

    override this.CreateExoVideoFilters(keyFrameIndex: int, exoOutputDescription: ExoOutputDescription) =
        [] :> IEnumerable<string>

    override this.CreateVideoEffect(devices: IGraphicsDevicesAndContext) =
        new YMM4ScriptK_Processor(devices, this) :> IVideoEffectProcessor

    override this.GetAnimatables() =
        ([] : IAnimatable list) :> IEnumerable<IAnimatable>
