using System;
using System.Collections.Generic;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Shooter
{
    internal class CloudSystem : GameSystem
    {
        private const int CloudCount = 5;

        private CloudRow[] _rows = new CloudRow[CloudCount];
        private ShooterLevel _level;

        public override void Init(Scene level)
        {
            _level = (ShooterLevel)level;

            for (int i = 0; i < CloudCount; i++)
            {
                CloudRow row = (CloudRow)new object();

                row.Image = _level.LoadImage("images/shooter/cloud" + (i + 1) +".png", false);
                row.OffsetX = 0;
                row.RelativeSpeed = 1.2f - (i / 10);
                row.Objects = new List<GameObject>();
                _rows[i] = row;
            }
        }

        public override void Load()
        {
            float offset = 0;
            for (int i = 0; i < CloudCount; i++)
            {
                CloudRow row = _rows[i];

                row.OffsetY = offset - 20;
                offset += row.Image.NaturalHeight - 20;
            }
        }

        public override void Update(CanvasContext2D context)
        {
            for (int i = CloudCount - 1; i >= 0; i--)
            {
                CloudRow row = _rows[i];
                row.OffsetX -= _level.BaseSpeed * row.RelativeSpeed * _level.DeltaTime;
                while (row.OffsetX + row.Image.NaturalWidth <= 0) row.OffsetX += row.Image.NaturalWidth;

                int objectIndex = 0;
                float offset = Math.Floor(row.OffsetX);
                while (offset < 800)
                {
                    GameObject gameObject;
                    if (row.Objects.Count <= objectIndex)
                    {
                        row.Objects.Add(gameObject = GameObject.Create(row.Image, 0, 0));
                        gameObject.Location.Z = i * 10;
                        gameObject.Location.Y = row.OffsetY;
                    }
                    else
                        gameObject = row.Objects[objectIndex];

                    gameObject.Location.X = offset;
                    gameObject.Visible = true;
                    offset += row.Image.NaturalWidth;
                    objectIndex++;
                }

                for (int j = objectIndex; j < row.Objects.Count; j++)
                {
                    row.Objects[j].Visible = false;
                }
            }
        }

        public override void Dispose()
        {
            _level = null;
            _rows = null;
        }
    }
}
