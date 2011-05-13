using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Runtime.CompilerServices;

namespace Vtj.Contest.Shooter
{
    /// <summary>
    /// Base class for any projectile shot by the player's ship
    /// </summary>
    internal abstract class PlasmaBall : GameObject
    {
        protected IMotion Motion;

        public bool Hit = false;
        public bool Destroyed = false;

        public PlasmaBall(Vector2D location, IMotion motion)
            : base()
        {
            Location = new Vector3D(location.X, location.Y, ShooterLevel.WeaponsZ);
            Motion = motion;

            ShooterLevel.Current.Plasma.AddPlasmaBall(this);
        }

        public override void Update()
        {
            ShooterLevel level = ShooterLevel.Current;

            if (!Hit)
            {
                foreach (GameObject building in ShooterLevel.Current.Buildings.VisibleBuildings)
                {
                    if (!Collides(building)) continue;
                    Hit = true;
                    return;
                }

                if (level.Status == ShooterStatus.Running && Collides(level.Meteor.Ship))
                {
                    level.Crash();
                    Hit = true;
                    return;
                }
            }

            Location.X -= level.DeltaTime * level.BaseSpeed;
            Motion.Update(this, level.DeltaTime);

            base.Update();
        }
    }
}
