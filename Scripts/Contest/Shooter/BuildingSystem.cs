using System;
using System.Collections.Generic;
using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Shooter
{
    internal class BuildingSystem : GameSystem
    {
        private List<ImageElement> _images;
        private List<GameObject> _buildings;
        private int _startPosition;
        private int _endPosition;
        private int _density;
        private ShooterLevel _level;

        private int[] _platformHeights = new int[] { 5, 12, 37, 52, 32};

        public List<GameObject> VisibleBuildings = new List<GameObject>();

        public BuildingSystem(int startPosition, int endPosition, int density)
        {
            _startPosition = startPosition;
            _endPosition = endPosition;
            _density = density;
        }

        public override void Init(Scene level)
        {
            _level = (ShooterLevel)level;

            _images = new List<ImageElement>();
            for (int i = 1; i < 6; i++)
                _images.Add(level.LoadImage("images/shooter/buildings/" + i + ".png", true));
        }

        public override void Load()
        {
            _buildings = new List<GameObject>();
            Generate();
        }

        public override void Update(CanvasContext2D context)
        {
            List<GameObject> garbage = new List<GameObject>();
            VisibleBuildings = new List<GameObject>();

            foreach (GameObject building in _buildings)
            {
                building.Location.X -= _level.BaseSpeed * _level.DeltaTime;

                if (building.Location.X > 800) continue;

                if (building.Location.X + building.GetCurrentSprite().Image.NaturalWidth < 0) garbage.Add(building);
                else VisibleBuildings.Add(building);
            }

            foreach (GameObject junk in garbage)
            {
                _buildings.Remove(junk);
                GameObject.Remove(junk);
            }
        }

        public override void Dispose()
        {
            VisibleBuildings = null;
            _buildings = null;
            _level = null;
        }

        private void Generate()
        {
            int position = _startPosition;

            while (position < _endPosition)
            {
                if (Math.Random() * 100 < _density)
                {
                    ImageElement image = _images[Math.Floor(Math.Random() * _images.Count)];
                    GameObject gameObject = GameObject.Create(image, 0, image.NaturalHeight);
                    gameObject.Location = new Vector3D(position, 590, ShooterLevel.BuildingsZ);
                    position += image.NaturalWidth;
                    _buildings.Add(gameObject);
                }

                position += Math.Random() * 40;
            }
        }

        public float MaxAltitude(int start, int finish)
        {
            float altitude = 590;

            foreach (GameObject building in _buildings)
            {
                Sprite sprite = building.GetCurrentSprite();

                if (building.Location.X + sprite.Image.Width < start) continue;

                if (building.Location.X > finish) return altitude;

                altitude = Math.Min(altitude, 590 - sprite.Image.NaturalHeight);
            }

            return altitude;
        }

        public float GunEmplacement(int start, int finish)
        {
            float altitude = 590;

            foreach (GameObject building in _buildings)
            {
                Sprite sprite = building.GetCurrentSprite();

                if (building.Location.X + sprite.Image.Width < start) continue;

                if (building.Location.X > finish) return altitude;

                altitude = Math.Min(altitude, 590 - sprite.Image.NaturalHeight + _platformHeights[_images.IndexOf(sprite.Image)]);
            }

            return altitude;
        }
    }
}
