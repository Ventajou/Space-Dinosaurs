using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Race
{
    internal class Tree : RoadObject
    {
        public Tree(long distance, float x, Scene scene)
            : base(distance, x, scene)
        { }

        protected override List<AnimationSequence> EnsureSequences(Scene scene)
        {
            List<AnimationSequence> sequences = new List<AnimationSequence>();
            AnimationSequence sequence = new AnimationSequence();
            ImageElement image = scene.LoadImage("Images/Race/obstacles/tree.png", false);
            sequence.AddSprite(image, 124, 384);
            sequences.Add(sequence);

            return sequences;
        }
    }
}
