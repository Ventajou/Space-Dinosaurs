using System;
using System.Collections.Generic;
using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;

namespace Vtj.Gaming
{
    internal delegate void AnimationCompletedEventHandler(object source, AnimationEndedEventArgs e);

    [ScriptNamespace("vtg")]
    internal class GameObject : IDisposable
    {
        #region Private Members
        private static List<GameObject> _visibleObjects;
        private DateTime _lastUpdateTick;

#if DEBUG
        private bool _disposed = false;
#endif
        #endregion

        #region Protected Members
        protected bool Started = false;
        #endregion

        #region Public Members
        public static List<GameObject> Objects;

        public Dictionary<string, AnimationSequence> AnimationSequences = new Dictionary<string, AnimationSequence>();

        public int CurrentFrame;
        public string CurrentAnimation;
        public int Delay;

        public Vector3D Location;
        public float Scale = 1;
        public bool Visible = true;
        #endregion

        #region Events
        public event AnimationCompletedEventHandler AnimationCompleted;
        #endregion

        public GameObject()
        {
            if (Objects == null) Objects = new List<GameObject>();
            Location = new Vector3D(0, 0, 0);
            Objects.Add(this);
        }

        public static GameObject Create(ImageElement defaultImage, float handleX, float handleY)
        {
            GameObject gameObject = new GameObject();
            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprite(defaultImage, handleX, handleY);
            gameObject.AnimationSequences["Default"] = sequence;
            gameObject.StartAnimation("Default");
            return gameObject;
        }

        public static void Remove(GameObject gameObject)
        {
            if (_visibleObjects.Contains(gameObject)) _visibleObjects.Remove(gameObject);
            if (Objects.Contains(gameObject)) Objects.Remove(gameObject);
            gameObject.Dispose();
#if DEBUG
            gameObject._disposed = true;
#endif
        }

        public void SetSprite(string animation, int index)
        {
#if DEBUG
            if (!AnimationSequences.ContainsKey(animation)) throw Exception.Create("The animation [" + animation + "] does not exist.", null);
            if (AnimationSequences[animation].Sprites.Count <= index) throw Exception.Create("The animation [" + animation + "] does not contain a sprite at index " + index + ".", null);
#endif
            CurrentAnimation = animation;
            CurrentFrame = index;
        }

        public Sprite GetCurrentSprite()
        {
            return AnimationSequences[CurrentAnimation].Sprites[CurrentFrame];
        }

        public void StartAnimation(string animation)
        {
#if DEBUG
            if (!AnimationSequences.ContainsKey(animation)) throw Exception.Create("The animation [" + animation + "] does not exist.", null);
#endif
            if (CurrentAnimation == animation) return;

            CurrentAnimation = animation;
            CurrentFrame = 0;
            Delay = AnimationSequences[animation].Delay;
            Started = true;
            _lastUpdateTick = DateTime.Now;
        }

        public void ResumeAnimation()
        {
            _lastUpdateTick = DateTime.Now;
            Started = true;
        }

        public void StopAnimation()
        {
            Started = false;
        }

        public virtual void Update()
        {
#if DEBUG
            if (_disposed) throw Exception.Create("Attempting to update an object that was disposed.", null);
#endif
            if (!Started || Delay == 0) return;

            DateTime now = DateTime.Now;
            AnimationSequence sequence = AnimationSequences[CurrentAnimation];

            if (now - _lastUpdateTick >= Delay)
            {
                _lastUpdateTick = now;

                if (!sequence.Loop && CurrentFrame == sequence.Sprites.Count - 1)
                {
                    StopAnimation();
                    if (AnimationCompleted != null) AnimationCompleted(this, new AnimationEndedEventArgs(CurrentAnimation));
                }
                else if (sequence.Sprites.Count > 1) CurrentFrame = (CurrentFrame + 1) % sequence.Sprites.Count;
            }
        }

        public void Draw(CanvasContext2D context)
        {
            if (!Visible || Script.IsNullOrUndefined(CurrentAnimation)) return;

            Sprite sprite = AnimationSequences[CurrentAnimation].Sprites[CurrentFrame];

            if (Scale == 1)
            {
                context.DrawImage(sprite.Image, Location.X - sprite.HandleX, Location.Y - sprite.HandleY);
            }
            else
            {
                context.DrawImage(sprite.Image,
                    Location.X - sprite.HandleX * Scale,
                    Location.Y - sprite.HandleY * Scale,
                    sprite.Image.NaturalWidth * Scale,
                    sprite.Image.NaturalHeight * Scale);
            }
        }

        public bool Intersect(float x, float y, float w, float h)
        {
            Sprite sprite = AnimationSequences[CurrentAnimation].Sprites[CurrentFrame];

            return !(((x > Location.X + sprite.Image.NaturalWidth - sprite.HandleX) ||
                    (x + w < Location.X - sprite.HandleX)) ||
                    ((y > Location.Y + sprite.Image.NaturalHeight - sprite.HandleY) ||
                    (y + h < Location.Y - sprite.HandleY)));
        }

        public bool Collides(GameObject target)
        {
            Sprite sprite = AnimationSequences[CurrentAnimation].Sprites[CurrentFrame];

            float x1 = Location.X - sprite.HandleX * Scale;
            float x2 = x1 + sprite.Image.NaturalWidth * Scale;
            float y1 = Location.Y - sprite.HandleY * Scale;
            float y2 = y1 + sprite.Image.NaturalHeight * Scale;

            Sprite tsprite = target.GetCurrentSprite();
            float tx1 = target.Location.X - tsprite.HandleX * Scale;
            float tx2 = tx1 + tsprite.Image.NaturalWidth * Scale;
            float ty1 = target.Location.Y - tsprite.HandleY * Scale;
            float ty2 = ty1 + tsprite.Image.NaturalHeight * Scale;

            if ((tx2 < x1) || (tx1 > x2) || (ty2 < y1) || (ty1 > y2)) return false;

            return Scene.Collision(sprite.Image.Src, tsprite.Image.Src, tx1 - x1, ty1 - y1);
        }

        public static int Compare(GameObject x, GameObject y)
        {
            if (x.Location.Z > y.Location.Z) return -1;
            if (x.Location.Z < y.Location.Z) return 1;
            return 0;
        }

        public static void DrawObjects(CanvasContext2D context)
        {
            _visibleObjects = new List<GameObject>();

            foreach (GameObject gameObject in Objects)
            {
                if (!gameObject.Visible) continue;
                _visibleObjects.Add(gameObject);
            }

            _visibleObjects.Sort(Compare);

            foreach (GameObject gameObject in _visibleObjects) gameObject.Draw(context);
        }

        public static void Clear()
        {
            Objects = new List<GameObject>();
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
        }

        #endregion
    }
}
