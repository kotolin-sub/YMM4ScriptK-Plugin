module lua

open System
open System.Runtime.InteropServices

module LuaJIT =
    [<Literal>]
    let private Lua51Dll = "lua51.dll"

    [<DllImport(Lua51Dll, CallingConvention = CallingConvention.Cdecl)>]
    extern IntPtr luaL_newstate()

    [<DllImport(Lua51Dll, CallingConvention = CallingConvention.Cdecl)>]
    extern void luaL_openlibs(IntPtr luaState)

    [<DllImport(Lua51Dll, CallingConvention = CallingConvention.Cdecl)>]
    extern int luaL_loadstring(IntPtr luaState, string chunk)

    [<DllImport(Lua51Dll, CallingConvention = CallingConvention.Cdecl)>]
    extern int lua_pcall(IntPtr luaState, int nargs, int nresults, int errfunc)

    [<DllImport(Lua51Dll, CallingConvention = CallingConvention.Cdecl)>]
    extern double lua_tonumber(IntPtr luaState, int index)

    [<DllImport(Lua51Dll, CallingConvention = CallingConvention.Cdecl)>]
    extern int lua_gettop(IntPtr luaState)

    [<DllImport(Lua51Dll, CallingConvention = CallingConvention.Cdecl)>]
    extern void lua_settop(IntPtr luaState, int index)
