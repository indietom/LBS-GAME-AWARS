using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Projectile : GameObject
    {
        byte movmentType;
        byte spriteType;
        public byte Damege { get; set; }

        short lifeTime;
        short maxLifeTime;

        public bool enemy;
        public bool explosive;

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
            }
        }

        public void Update()
        {
            if(MaxFrame > 0)
            {
                Animate();
                AnimationCount += 1;
            }

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
