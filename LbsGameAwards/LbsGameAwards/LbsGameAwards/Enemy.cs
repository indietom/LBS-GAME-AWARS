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

        byte type;
        byte direction;

        bool attacking;
        bool invisible;
        bool followPlayer;

        short attackCount;
        short maxAttackCount;
        short hurtCount;
        short maxHurtCount;

        public Enemy(Vector2 pos2, byte type2)
        {
            followPlayer = true;
            Pos = pos2;
            type = type2;
            AssignType();
            OrginalColor = color;
            Z = 1.0f;
            maxHurtCount = 8;
        }

        public void CheckHealth()
        {
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

                    if(followPlayer)
                    {
                        foreach (Player p in Game1.players)
                        {
                            desierdAngle = AimAt(p.Pos, false);
                        }

                        if (Angle > desierdAngle) Angle -= 2f;
                        if (Angle < desierdAngle) Angle += 2f;

                        if(Angle < 0 && desierdAngle > 0 || Angle > 0 && desierdAngle < 0) Angle *= -1;

                        Pos += Vel;
                    }
                    break;
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
            switch(type)
            {
                case 0:
                    maxAttackCount = 16;
                    attackDistance = 16;
                    hp = 2;
                    MaxAnimationCount = 4;
                    MaxFrame = 4;
                    Speed = 1f;
                    SpriteCoords = new Point(1, 331);
                    SetSize(32);
                    break;
            }
        }
    }
}
