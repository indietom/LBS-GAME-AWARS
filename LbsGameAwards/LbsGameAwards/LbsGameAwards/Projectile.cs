using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Projectile : GameObject
    {
        float maxHeight;

        byte movmentType;
        byte spriteType;
        byte explosionSize;
        public byte Damege { get; set; }

        short lifeTime;
        short maxLifeTime;

        public bool enemy;
        public bool explosive;
        bool falling;

        public Projectile(Vector2 pos2, float angle2, float speed2, byte damege2, byte movmentType2, byte spriteType2, bool enemy2)
        {
            Pos = pos2;
            Angle = angle2;
            Speed = speed2;
            Damege = damege2;
            movmentType = movmentType2;
            spriteType = spriteType2;
            enemy = enemy2; 
            AssignSpriteType();
        }

        public Projectile(Vector2 pos2, float angle2, float speed2, byte damege2, byte movmentType2, byte spriteType2, bool enemy2, float z2)
        {
            Pos = pos2;
            Angle = angle2;
            Speed = speed2;
            Damege = damege2;
            movmentType = movmentType2;
            spriteType = spriteType2;
            enemy = enemy2;
            AssignSpriteType();
            Z = z2;
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
                    if (falling)
                    {
                        Scale = Lerp(Scale, 0.5f, 0.07f);
                        if(Scale <= 0.6f)
                        {
                            Game1.explosions.Add(new Explosion(Pos + new Vector2(-16, -16), 32, Color.Orange, true, enemy));
                            destroy = true;
                        }
                    }
                    else
                    {
                        Scale = Lerp(Scale, maxHeight, 0.02f);
                        if (Scale >= Lerp(Scale, maxHeight, 0.02f)/1.1)
                            falling = true;
                    }
                    Speed = Lerp(Speed, 0, 0.1f);
                    AngleMath();
                    Pos += Vel;
                    break;
            }
        }

        public void Update()
        {
            if(MaxFrame > 0)
            {
                Animate();
                AnimationCount += 1;
            }

            if (spriteType != 1) Z = ZOrder();

            Movment();
        }

        public void AssignSpriteType()
        {
            Z = 0.5f;
            if (!enemy)
            {
                switch(spriteType)
                {
                    case 0:
                        SetSize(8);
                        SpriteCoords = new Point(166, 1);
                        Z = (Angle == -90) ? 0.05f : Z;
                        break;
                    case 1:
                        SetSize(8);
                        SpriteCoords = new Point(166, 10);
                        maxHeight = 10.0f;
                        Z = 0.9f;
                        rotated = true;
                        break;
                }
            }
            else
            {
                switch (spriteType)
                {
                    case 0:
                        SetSize(8);
                        SpriteCoords = new Point(175, 1);
                        break;
                }
            }
        }
    }
}
