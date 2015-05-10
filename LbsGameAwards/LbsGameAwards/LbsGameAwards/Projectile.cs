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

        short smokeCount;

        byte movmentType;
        byte spriteType;
        byte explosionSize;
        public byte Damege { get; set; }
        byte orginalDamege;

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
            orginalDamege = Damege;
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
            orginalDamege = Damege;
        }

        public void OnImpact()
        {
            if (explosive)
            {
                if (rotated) Game1.explosions.Add(new Explosion(new Vector2(Pos.X - explosionSize / 2, Pos.Y - explosionSize / 2), explosionSize, Color.Orange));
                else Game1.explosions.Add(new Explosion(Pos, explosionSize, Color.Orange));
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
                case 2:
                    AngleMath();
                    Pos += Vel;
                    Speed = Lerp(Speed, 5, Speed/10);
                    smokeCount += 1;
                    if (smokeCount >= 8)
                    {
                        Game1.particles.Add(new Particle(Pos, 0, 0, -Angle, 0.01f));
                        smokeCount = 0;
                    }
                    break;
            }
        }

        public void Update()
        {
            if(MaxFrame > 0)
            {
                Animate();
                AnimationCount += 1;
                SpriteCoords = new Point(Frame(CurrentFrame, Size.X), SpriteCoords.Y);
            }

            if (spriteType != 1) Z = ZOrder();

            foreach(Door d in Game1.doors)
            {
                if(d.HitBox().Intersects(HitBox()))
                {
                    OnImpact();
                    destroy = true;
                }
            }

            if(Game1.currentRoom.tileIntersection(HitBox(), 4))
            {
                destroy = true;
                OnImpact();
            }

            if (!enemy)
            {
                switch (spriteType)
                {
                    case 2:
                        if (CurrentFrame >= MaxFrame - 1) destroy = true;
                        if (CurrentFrame >= MaxFrame - 4) Damege = 0;
                        break;
                    case 3:
                        Rotation = Angle;
                        break;
                }
            }
            else
            {
                switch(spriteType)
                {
                    case 1:
                        Rotation += 5f;
                        break;
                }
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
                    case 1:
                        SetSize(8);
                        SpriteCoords = new Point(166, 10);
                        maxHeight = 10.0f;
                        Z = 0.9f;
                        rotated = true;
                        break;
                    case 2:
                        SetSize(32);
                        MinFrame = 5;
                        MaxFrame = (short)(MinFrame + 9);
                        CurrentFrame = MinFrame;
                        SpriteCoords = new Point(Frame(MinFrame), 67);
                        MaxAnimationCount = 4;
                        break;
                    case 3:
                        SetSize(16, 8);
                        SpriteCoords = new Point(199, 1);
                        rotated = true;
                        explosive = true;
                        explosionSize = 32;
                        //Console.WriteLine("e");
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
                    case 1:
                        SetSize(4);
                        SpriteCoords = new Point(175, 10);
                        rotated = true;
                        break;
                }
            }
        }
    }
}
