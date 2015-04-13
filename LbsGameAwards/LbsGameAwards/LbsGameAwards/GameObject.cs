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
            if(rotated)
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

        public Color color { get; set; }

        public Color OrginalColor { get; set; }

        public bool rotateOnRad;
        public bool rotated;
        public bool destroy;

        public float Lerp(float s, float e, float t)
        {
            return s + t * (e - s);
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
            VelX = ((float)Math.Cos(Angle) * Speed);
            VelY = ((float)Math.Sin(Angle) * Speed);
        }

        public float AimAt(Vector2 target)
        {
            return (float)Math.Atan2(target.Y - Pos.Y, target.X - Pos.X);
        }

        public float DistanceTo(Vector2 target)
        {
            return (float)Math.Sqrt((Pos.X - target.X) * (Pos.X - target.X) + (Pos.Y - target.Y) * (Pos.Y - target.Y));
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            Rectangle soruceRectangle = new Rectangle(SpriteCoords.X, SpriteCoords.Y, Size.X, Size.Y);
            Vector2 origin = Vector2.Zero;
            if (rotated) origin = new Vector2(Size.X / 2, Size.Y / 2);
            if(!rotateOnRad) spriteBatch.Draw(spritesheet, Pos, soruceRectangle, color, Rotation, origin, Scale, SpriteEffects.None, Z);
            else spriteBatch.Draw(spritesheet, Pos, soruceRectangle, color, (Rotation * (float)Math.PI/180), origin, Scale, SpriteEffects.None, Z);
        }
    }
}
