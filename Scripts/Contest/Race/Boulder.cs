using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Race
{
    [ScriptNamespace("vtsdr")]
    internal class Boulder : RoadObject
    {
        public Boulder(long distance, float x, Scene scene)
            : base(distance, x, scene)
        { }

        protected override List<AnimationSequence> EnsureSequences(Scene scene)
        {
            List<AnimationSequence> sequences = new List<AnimationSequence>();
            AnimationSequence sequence = new AnimationSequence();
            ImageElement image = scene.LoadImage("Images/Race/obstacles/r1.png", false);
            sequence.AddSprite(image, 47, 78);
            sequences.Add(sequence);

            sequence = new AnimationSequence();
            image = scene.LoadImage("Images/Race/obstacles/r2.png", false);
            sequence.AddSprite(image, 47, 78);
            sequences.Add(sequence);

            sequence = new AnimationSequence();
            image = scene.LoadImage("Images/Race/obstacles/r3.png", false);
            sequence.AddSprite(image, 39, 77);
            sequences.Add(sequence);

            sequence = new AnimationSequence();
            image = scene.LoadImage("Images/Race/obstacles/r4.png", false);
            sequence.AddSprite(image, 50, 79);
            sequences.Add(sequence);

            sequence = new AnimationSequence();
            image = scene.LoadImage("Images/Race/obstacles/r5.png", false);
            sequence.AddSprite(image, 46, 74);
            sequences.Add(sequence);

            sequence = new AnimationSequence();
            image = scene.LoadImage("Images/Race/obstacles/r6.png", false);
            sequence.AddSprite(image, 47, 84);
            sequences.Add(sequence);

            return sequences;
        }
    }
}
