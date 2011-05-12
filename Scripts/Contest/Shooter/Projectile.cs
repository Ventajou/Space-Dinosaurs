using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Runtime.CompilerServices;

namespace Vtj.Contest.Shooter
{
    /// <summary>
    /// Base class for any projectile shot by the player's ship
    /// </summary>
    [ScriptNamespace("vtsds")]
    internal abstract class Projectile : GameObject
    {
        protected IMotion Motion;

        protected abstract int Strength { get; }

        public bool Hit = false;
        public bool Destroyed = false;

        public Projectile(Vector2D location, IMotion motion)
            : base()
        {
            Location = new Vector3D(location.X, location.Y, ShooterLevel.WeaponsZ);
            Motion = motion;
        }

        public override void Update()
        {
            if (!Hit)
            {
                foreach (GameObject building in ShooterLevel.Current.Buildings.VisibleBuildings)
                {
                    if (!Collides(building)) continue;
                    Hit = true;
                    return;
                }

                foreach (Dino dino in ShooterLevel.Current.Dinos.VisibleDinos)
                {
                    if (dino.Dead || !Collides(dino)) continue;
                    dino.Hit(Strength);
                    Hit = true;
                    return;
                }
            }

            ShooterLevel level = ShooterLevel.Current;
            Location.X -= level.DeltaTime * level.BaseSpeed;
            Motion.Update(this, level.DeltaTime);

            base.Update();
        }

        public virtual void Removed()
        { }
    }
}
