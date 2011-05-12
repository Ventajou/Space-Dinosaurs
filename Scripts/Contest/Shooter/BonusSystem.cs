using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Html.Media.Graphics;

namespace Vtj.Contest.Shooter
{
    internal class BonusSystem : GameSystem
    {
        private List<Bonus> _bonuses;
        private string[] _bonusTypes = new string[] 
            {
                BonusTypes.PowerUp,
                BonusTypes.PowerUp,
                BonusTypes.SpeedUp,
                BonusTypes.SpeedUp,
                BonusTypes.SpeedUp,
                BonusTypes.SpeedUp,
                BonusTypes.SpeedUp,
                BonusTypes.TripleShot,
                BonusTypes.TripleShot,
                BonusTypes.TripleShot,
                BonusTypes.Missile,
                BonusTypes.Missile,
                BonusTypes.DoubleShot,
                BonusTypes.DoubleShot,
                BonusTypes.DoubleShot,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty
            };

        public override void Init(Scene level)
        {
            _bonuses = new List<Bonus>();
        }

        public override void Load()
        {
            Bonus.PreloadImages();
        }

        public override void Update(CanvasContext2D context)
        {
            List<Bonus> garbage = new List<Bonus>();
            foreach (Bonus bonus in _bonuses)
            {
                bonus.Update();

                if (bonus.Collides(ShooterLevel.Current.Meteor.Ship))
                {
                    ShooterLevel.Current.ApplyBonus(bonus.BonusType);
                    garbage.Add(bonus);
                }

                if (!bonus.Intersect(0, -100, 800, 700)) garbage.Add(bonus);
            }

            foreach (Bonus junk in garbage)
            {
                GameObject.Remove(junk);
                _bonuses.Remove(junk);
            }
        }

        public void CreateBonus(Vector2D location)
        {
            string bonusType = _bonusTypes[Math.Floor(Math.Random() * _bonusTypes.Length)];
            if (string.IsNullOrEmpty(bonusType)) return;
            _bonuses.Add(new Bonus(location, bonusType));
        }
    }
}
