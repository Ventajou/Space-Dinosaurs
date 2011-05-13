using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Race
{
    internal class Pole : RoadObject
    {
        public Pole(long distance, float x, Scene scene)
            : base(distance, x, scene)
        { }

        protected override List<AnimationSequence> EnsureSequences(Scene scene)
        {
            List<AnimationSequence> sequences = new List<AnimationSequence>();
            AnimationSequence sequence = new AnimationSequence();
            ImageElement image = scene.LoadImage("Images/Race/obstacles/pole.png", false);
            sequence.AddSprite(image, 10, 80);
            sequences.Add(sequence);

            return sequences;
        }
    }
}
