using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Race
{
    internal abstract class RoadObject
    {
        public long Distance;
        public float X;
        public GameObject GameObject;

        protected Scene _scene;

        public RoadObject(long distance, float x, Scene scene)
        {
            Distance = distance;
            X = x;
            _scene = scene;

            List<AnimationSequence> sequences = EnsureSequences(scene);

            this.GameObject = new GameObject();
            this.GameObject.AnimationSequences["Default"] = sequences[Math.Floor(Math.Random() * sequences.Count)];
            this.GameObject.StartAnimation("Default");
        }

        protected abstract List<AnimationSequence> EnsureSequences(Scene scene);
    }
}
