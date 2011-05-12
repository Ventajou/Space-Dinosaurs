using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Shooter
{
    [ScriptNamespace("vtsds")]
    internal abstract class Dino : GameObject
    {
        protected IMotion Motion;
        protected int Life;

        public bool Gone = false;
        public bool Dead = false;
        public bool Locked = false;

        protected const string DefaultAnimation = "Default";
        protected const string ExplosionAnimation = "Explosion";

        public event EventHandler KilledEvent;

        public Dino(IMotion motion)
        {
            Motion = motion;
        }

        public override void Update()
        {
            ShooterLevel level = ShooterLevel.Current;
            Location.X -= level.BaseSpeed * level.DeltaTime;
            
            if (!(Visible = Intersect(0, 0, 800, 600)))
            {
                if (Location.X < 0) Gone = true;
                return;
            }

            if (Motion != null) Motion.Update(this, level.DeltaTime);

            base.Update();

            if (!Started && Dead) Gone = true;
        }

        public void Hit(int strength)
        {
            if (Dead) return;

            Life -= strength;
            if (Life <= 0)
            {
                Dead = true;
                StartAnimation(ExplosionAnimation);
            }
        }

        public override void Dispose()
        {
            KilledEvent = null;
            base.Dispose();
        }
    }
}
