namespace YMM4ScriptK

open System
open System.IO
open YukkuriMovieMaker.Plugin

module ScriptRegistry =
    let private allowed =
        set [".lua"; ".anim"; ".anm"; ".script"]

    let mutable private scripts: (string * string) list = []

    //実行ファイルからの相対.\user\plugin\YMM4ScriptK\scripts
    let private resolveRoot () =
        let baseDir = AppDomain.CurrentDomain.BaseDirectory
        Path.Combine(baseDir, "user", "plugin", "YMM4ScriptK", "scripts")

    // スキャン実行
    let refresh () =
        try
            let root = resolveRoot()
            Directory.CreateDirectory(root) |> ignore
            let files =
                Directory.EnumerateFiles(root, "*.*", SearchOption.AllDirectories)
                |> Seq.filter (fun p ->
                    let ext = Path.GetExtension(p).ToLowerInvariant()
                    allowed.Contains(ext))
                |> Seq.map (fun p -> Path.GetFileNameWithoutExtension(p), p)
                |> Seq.distinctBy fst
                |> Seq.sortBy fst
                |> Seq.toList
            scripts <- files
        with _ ->
            scripts <- []

    let getDisplayNames () =
        scripts |> List.map fst

    let tryGetFullPath (name: string) =
        scripts |> List.tryFind (fun (n,_) -> n = name) |> Option.map snd

// 起動時に一回だけrefreshするやーつ。
type internal ScriptStartupPlugin() =
    static do
        ScriptRegistry.refresh()

    interface IPlugin with
        member _.Name = "YMM4ScriptK.ScriptStartup"
