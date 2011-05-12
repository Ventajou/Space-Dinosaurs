using System.Runtime.CompilerServices;

namespace System.Debug
{
    [IgnoreNamespace]
    [Imported]
    [ScriptName("console")]
    public sealed class Console
    {
        public static void Log(object value) { }
        public static void Log(string value) { }
    }
}