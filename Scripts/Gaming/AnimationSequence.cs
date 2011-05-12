using System.Collections.Generic;
using System.Html;
using System.Runtime.CompilerServices;

namespace Vtj.Gaming
{
    /// <summary>
    /// Groups a set of sprites in a sequence that can be animated
    /// </summary>
    [ScriptNamespace("vtg")]
    internal class AnimationSequence
    {
        public List<Sprite> Sprites = new List<Sprite>();
        public int Delay;
        public bool Loop = true;

        public void AddSprite(ImageElement image, float handleX, float handleY)
        {
            Sprite sprite = new Sprite();
            sprite.Image = image;
            sprite.HandleX = handleX;
            sprite.HandleY = handleY;
            Sprites.Add(sprite);
        }

        public void AddSprites(ImageElement[] images, float handleX, float handleY)
        {
            foreach (ImageElement image in images)
            {
                Sprite sprite = new Sprite();
                sprite.Image = image;
                sprite.HandleX = handleX;
                sprite.HandleY = handleY;
                Sprites.Add(sprite);
            }
        }
    }
}
