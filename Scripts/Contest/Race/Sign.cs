using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Race
{
    [ScriptNamespace("vtsdr")]
    class Sign : RoadObject
    {
        public Sign(long distance, float x, Scene scene)
            : base(distance, x, scene)
        { }


        protected override List<AnimationSequence> EnsureSequences(Scene scene)
        {
            List<AnimationSequence> sequences = new List<AnimationSequence>();
            AnimationSequence sequence = new AnimationSequence();
            ImageElement image = scene.LoadImage("Images/Race/obstacles/sign.png", false);
            sequence.AddSprite(image, 0, 240);
            sequences.Add(sequence);

            return sequences;
        }
    }
}
