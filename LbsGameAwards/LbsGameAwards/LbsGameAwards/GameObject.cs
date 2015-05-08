using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LbsGameAwards
{
    abstract class GameObject
    {
        public Vector2 Pos { get; set; }

        public Vector2 GetCenter { get { return new Vector2(Pos.X+Size.X/2, Pos.Y+Size.Y/2); } }
        public Vector2 Vel { get { return new Vector2(VelX, VelY); } }

        public Rectangle HitBox()
        {
            if(!rotated)
            {
                return new Rectangle((int)Pos.X, (int)Pos.Y, Size.X, Size.Y);
            }
            else
            {
                return new Rectangle((int)Pos.X - Size.X / 2, (int)Pos.Y - Size.Y / 2, Size.X, Size.Y);
            }
        }

        public Point SpriteCoords { get; set; }
        public Point Size { get; private set; }

        public float Angle2 { get; set; }
        public float Angle { get; set; }
        public float Speed { get; set; }
        
        public float VelX {get; set;}
        public float VelY {get; set;}

        public float Rotation { get; set; }
        public float Scale = 1.0f;
        public float Z { get; set; }

        public short CurrentFrame { get; set; }
        public short MinFrame { get; set; }
        public short MaxFrame { get; set; }
        public short AnimationCount { get; set; }
        public short MaxAnimationCount { get; set; }

        public Color color = Color.White;

        public Color OrginalColor { get; set; }

        public bool rotateOnRad;
        public bool rotated;
        public bool destroy;

        public SpriteEffects spriteEffects = SpriteEffects.None;

        public void Animate()
        {
            if(AnimationCount >= MaxAnimationCount)
            {
                CurrentFrame += 1;
                AnimationCount = 0;
            }
            if(CurrentFrame >= MaxFrame)
            {
                CurrentFrame = MinFrame;
            }
        }

        public bool OnScreen()
        {
            return Pos.X < 640 && Pos.X > 0 && Pos.Y < 640 && Pos.Y > 0;
        }

        // I wanted this for some reason at 2 am, can't remember why
        public int DeFrame(int spriteCoord)
        {
            return ((spriteCoord - 1) / 16) / 2;
        }

        public int Frame(int cell)
        {
            return cell * 32 + 1 + cell;
        }

        public int Frame(int cell, int size)
        {
            return cell * size + 1 + cell;
        }

        public float Lerp(float s, float e, float t)
        {
            return s + t * (e - s);
        }

        public float ZOrder()
        {
            if(!rotated) return (GetCenter.Y > 0) ? Z = GetCenter.Y / 1000 : 0.01f;
            else return (Pos.Y > 0) ? Z = GetCenter.Y / 1000 : 0.01f;
        }

        public void SetSize(int width, int height)
        {
            Size = new Point(width, height);
        }

        public void SetSize(int size)
        {
            Size = new Point(size, size);
        }

        public void AngleMath()
        {
            if (!rotateOnRad) Angle2 = (Angle * (float)Math.PI / 180);
            if (rotateOnRad) Angle2 = Angle;
            VelX = ((float)Math.Cos(Angle2) * Speed);
            VelY = ((float)Math.Sin(Angle2) * Speed);
        }

        public float AimAt(Vector2 target, bool rad)
        {
            if(rad)
                return (float)Math.Atan2(target.Y - Pos.Y, target.X - Pos.X);
            else
                return (float)Math.Atan2(target.Y - Pos.Y, target.X - Pos.X) * 180 / (float)Math.PI;
        }

        public float DistanceTo(Vector2 target)
        {
            return (float)Math.Sqrt((Pos.X - target.X) * (Pos.X - target.X) + (Pos.Y - target.Y) * (Pos.Y - target.Y));
        }

        public void DrawSprite(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            Rectangle soruceRectangle = new Rectangle(SpriteCoords.X, SpriteCoords.Y, Size.X, Size.Y);
            Vector2 origin = Vector2.Zero;
            if (rotated) origin = new Vector2(Size.X / 2, Size.Y / 2);
            if(rotateOnRad) spriteBatch.Draw(spritesheet, Pos, soruceRectangle, color, Rotation, origin, Scale, spriteEffects, Z);
            else spriteBatch.Draw(spritesheet, Pos, soruceRectangle, color, (Rotation * (float)Math.PI / 180), origin, Scale, spriteEffects, Z);
        }
    }
}
