using System;
using System.Runtime.CompilerServices;

namespace Vtj.Gaming
{
    /// <summary>
    /// Arguments for the AnimationEnded event
    /// </summary>
    internal class AnimationEndedEventArgs : EventArgs
    {
        public string Key;

        public AnimationEndedEventArgs(string key)
            : base()
        {
            Key = key;
        }
    }
}
