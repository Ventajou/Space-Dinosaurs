using System;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;

namespace Vtj.Gaming
{
    /// <summary>
    /// Provides a framework for game subsystems to be easily reused among scenes
    /// </summary>
    internal abstract class GameSystem : IDisposable
    {
        /// <summary>
        /// Called when the scene is being initialized.
        /// </summary>
        /// <param name="level">The game scene that this instance is attached to</param>
        public abstract void Init(Scene level);

        /// <summary>
        /// Called when all resources have been loaded.
        /// </summary>
        public virtual void Load() { }

        /// <summary>
        /// Called at each iterarion.
        /// </summary>
        /// <param name="context"></param>
        public abstract void Update(CanvasContext2D context);

        /// <summary>
        /// Override to clean up any resource used.
        /// </summary>
        public virtual void Dispose() { }

        /// <summary>
        /// Called when the game is paused.
        /// </summary>
        public virtual void Pause() { }

        /// <summary>
        /// Called when the game is resumed from pause.
        /// </summary>
        public virtual void Resume() { }
    }
}
