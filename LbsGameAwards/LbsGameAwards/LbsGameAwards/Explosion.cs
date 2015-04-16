using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Explosion : GameObject 
    {
        short animationOffset;

        byte size;

        public Explosion(Vector2 pos2, byte size2, Color color2)
        {
            Z = 0.11f;
            Pos = pos2;
            size = size2;
            SetSize(size);
            color = color2;
            MaxAnimationCount = 4;
            AssignSprite();
            CurrentFrame = MinFrame;
        }

        public void Update()
        {
            Animate();
            AnimationCount += 1;
            destroy = (CurrentFrame >= MaxFrame - 1) ? true : destroy;
            SpriteCoords = new Point(Frame(CurrentFrame), SpriteCoords.Y);
        }

        public void AssignSprite()
        {
            switch(size)
            {
                case 32:
                    MinFrame = 7;
                    MaxFrame = 14;
                    SpriteCoords = new Point(Frame(MinFrame), 166);
                    break;
            }
        }
    }
}
