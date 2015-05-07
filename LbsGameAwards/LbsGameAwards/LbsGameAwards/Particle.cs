using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Particle : GameObject
    {
        byte movmentType;
        byte spriteType;

        short animationOffset;

        Vector2 target;

        public Particle(Vector2 pos2, byte movmentType2, byte spriteType2, float ang, float speed2)
        {
            Pos = pos2;
            movmentType = movmentType2;
            spriteType = spriteType2;
            Angle = ang;
            Speed = speed2;
            AssignSprite();
        }

        public Particle(Vector2 pos2, byte movmentType2, byte spriteType2, Vector2 target2, float speed2)
        {
            Pos = pos2;
            movmentType = movmentType2;
            spriteType = spriteType2;
            target = target2;
            Speed = speed2;
            AssignSprite();
        }

        public void Update()
        {
            if(MaxFrame > 0)
            {
                Animate();
                AnimationCount += 1;

                SpriteCoords = new Point(Frame(CurrentFrame, Size.X)+animationOffset, SpriteCoords.Y);
            }

            switch(spriteType)
            {
                case 0:
                    if(CurrentFrame == MaxFrame - 1) destroy = true;
                    Rotation += 10f;
                    Z = 0.01f;
                    break;
            }
        }

        public void Movment()
        {
            switch(movmentType)
            {
                case 0:
                    AngleMath();
                    Pos += Vel;
                    break;
                case 1:
                    Pos = new Vector2(Lerp(Pos.X, target.X, Speed), Lerp(Pos.Y, target.Y, Speed));
                    break;
            }
        }

        public void AssignSprite()
        {
            switch(spriteType)
            {
                case 0:
                    SpriteCoords = new Point(364, 331);
                    SetSize(8);
                    MaxFrame = 5;
                    MaxAnimationCount = 4;
                    rotated = true;
                    break;
            }
            animationOffset = (short)(SpriteCoords.X + 1);
        }
    }
}
