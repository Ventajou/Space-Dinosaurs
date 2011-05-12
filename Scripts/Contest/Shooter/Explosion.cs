// Explosion.cs
//

using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Html;

namespace Vtj.Contest.Shooter
{
    internal class Explosion : GameObject
    {
        public Explosion()
            : base()
        {
            ShooterLevel level = ShooterLevel.Current;
            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                level.LoadImage("images/shooter/boss/0001.png", false),
                level.LoadImage("images/shooter/boss/0002.png", false),
                level.LoadImage("images/shooter/boss/0003.png", false),
                level.LoadImage("images/shooter/boss/0004.png", false),
                level.LoadImage("images/shooter/boss/0005.png", false),
                level.LoadImage("images/shooter/boss/0006.png", false),
                level.LoadImage("images/shooter/boss/0007.png", false),
                level.LoadImage("images/shooter/boss/0008.png", false),
                level.LoadImage("images/shooter/boss/0009.png", false),
            }, 50, 50);
            sequence.Delay = 50;
            sequence.Loop = true;
            AnimationSequences["Default"] = sequence;

            StartAnimation("Default");
        }
    }
}
