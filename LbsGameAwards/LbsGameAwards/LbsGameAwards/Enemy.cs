using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Enemy : GameObject
    {
        sbyte hp;

        float attackDistance;
        float desierdAngle;
        float turnSpeed;
        float orginalSpeed;

        byte type;
        byte direction;

        bool attacking;
        bool invisible;
        bool followPlayer;

        short attackCount;
        short maxAttackCount;
        short hurtCount;
        short maxHurtCount;

        Color bloodColor;

        int tag;

        public Enemy(Vector2 pos2, byte type2)
        {
            followPlayer = true;
            Pos = pos2;
            type = type2;
            AssignType();
            orginalSpeed = Speed;
            OrginalColor = color;
            Z = 1.0f;
            maxHurtCount = 8;
            tag = Globals.CurrentEnemyTag+1;
            Globals.CurrentEnemyTag = tag;
            bloodColor = Color.LightGreen;
        }

        public void CheckHealth()
        {
            Random random = new Random();
            if(hp <= 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Game1.explosions.Add(new Explosion(Pos+new Vector2(random.Next(-Size.X/2, Size.X/2), random.Next(-Size.Y/2, Size.Y/2)), 32, bloodColor));
                }
                destroy = true;
            }
            foreach (Projectile p in Game1.projectiles)
            {
                if (p.HitBox().Intersects(HitBox()))
                {
                    if (hurtCount <= 0)
                    {
                        hp -= (sbyte)p.Damege;
                        hurtCount = 1;
                    }
                    p.destroy = true;
                }
            }
        }

        public void AttackUpdate()
        {
            switch(type)
            {
                case 0:
                    AngleMath();
                    rotateOnRad = false;
                    foreach (Player p in Game1.players)
                    {
                        desierdAngle = AimAt(p.Pos, false);
                        if (DistanceTo(p.Pos) > attackDistance) followPlayer = true;
                        else
                        {
                            Angle = desierdAngle;
                            followPlayer = false;
                        }
                    }
                    TurnTwoards();
                    break;
            }
        }

        public void TurnTwoards()
        {
            if (followPlayer)
            {
                foreach (Player p in Game1.players)
                {
                    desierdAngle = AimAt(p.Pos, false);
                    if (p.Pos.Y == Pos.Y)
                    {
                        if (p.Pos.X < Pos.X) Angle = 180;
                        else Angle = 0;
                    }
                }

                if (Angle > desierdAngle) Angle -= turnSpeed;
                if (Angle < desierdAngle) Angle += turnSpeed;

                if (Angle < 0 && desierdAngle > 0 || Angle > 0 && desierdAngle < 0) Angle *= -1;

                Pos += Vel;
            }
        }

        public void Update()
        {
            Z = GetCenter.Y / 1000;
            AttackUpdate();
            CheckHealth();
            HurtUpdate();
            Animate();
            AnimationCount += 1;
            SpriteCoords = new Point(Frame(CurrentFrame), SpriteCoords.Y);

            foreach(Enemy e in Game1.enemies)
            {
                if(e.HitBox().Intersects(HitBox()) && e.type == type)
                {
                    if(e.tag > tag)
                    {

                    }
                }
            }
        }

        public void HurtUpdate()
        {
            if(hurtCount >= 1)
            {
                hurtCount += 1;
                color = Color.Red;
                invisible = true;
            }
            if (hurtCount >= maxHurtCount)
            {
                color = OrginalColor;
                invisible = false;
                hurtCount = 0;
            }
        }

        public void AssignType()
        {
            Random random = new Random();
            switch(type)
            {
                case 0:
                    maxAttackCount = 16;
                    attackDistance = 32;
                    hp = 2;
                    MaxAnimationCount = 8;
                    MaxFrame = 4;
                    Angle = random.Next(-180, 180);
                    Speed = 1.5f;
                    turnSpeed = 2f;
                    SpriteCoords = new Point(1, 331);
                    SetSize(32);
                    break;
            }
        }
    }
}
