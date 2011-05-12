using System;
using System.Runtime.CompilerServices;

namespace System.Html
{
    [IgnoreNamespace]
    [Imported]
    public sealed class AudioElement : Element
    {
        private AudioElement()
        {
        }

        [IntrinsicProperty]
        public float Volume
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        [IntrinsicProperty]
        public float CurrentTime
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        [IntrinsicProperty]
        public string Src
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        [IntrinsicProperty]
        public float Duration
        {
            get
            {
                return 0;
            }
        }

        [IntrinsicProperty]
        public bool Paused
        {
            get
            {
                return false;
            }
        }

        [IntrinsicProperty]
        public bool Ended
        {
            get
            {
                return false;
            }
        }

        public void Play() { }
        public void Pause() { }
        public void Load() { }
    }
}
