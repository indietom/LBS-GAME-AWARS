﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LbsGameAwards
{
    class Enemy : GameObject
    {
        public sbyte Hp { get; set; }

        int worth;

        byte type;
        byte direction;

        float attackDistance;
        float desierdAngle;
        float turnSpeed;
        float orginalSpeed;
        float shootAngle;

        bool attacking;
        bool invisible;
        bool followPlayer;

        short attackCount;
        short maxAttackCount;
        short hurtCount;
        short maxHurtCount;
        short changeDirectionCount;
        short animationOffset;
        short moveToPosCount = 1;

        Color bloodColor;

        Rectangle turretHitBox;

        Vector2 turretPos;

        Vector2 target;
        Vector2 tankTarget;
        Vector2 newTankTarget;

        int tag;

        public Enemy(Vector2 pos2, byte type2)
        {
            followPlayer = true;
            Pos = pos2;
            type = type2;
            bloodColor = Color.LightGreen;
            maxHurtCount = 8;
            tag = Globals.CurrentEnemyTag + 1;
            Globals.CurrentEnemyTag = tag;
            AssignType();
            orginalSpeed = Speed;
            OrginalColor = color;
            worth = (type * 100) + 100;
        }

        public void CheckHealth()
        {
            Random random = new Random();
            if(Hp <= 0)
            {
                Game1.players[0].Score += worth;
                bool metal = (bloodColor == Color.LightGreen) ? false : true;
                for (int i = 0; i < 50; i++)
                {
                    Vector2 randomOffset = Vector2.Zero;
                    if (rotated)
                        randomOffset = new Vector2(random.Next(-Size.X / 2, Size.X / 2), random.Next(-Size.Y / 2, Size.Y / 2));
                    else
                        randomOffset = new Vector2(random.Next(-Size.X, Size.X), random.Next(-Size.Y, Size.Y));

                    Game1.gibs.Add(new Gib(Pos + randomOffset, random.Next(360), random.Next(16) / 5, random.Next(20, 30) / 12, (byte)random.Next(5), metal));
                }

                byte tmpExplosionSize = (Size.X >= 32) ? (byte)Size.X : (byte)32;
                for (int i = 0; i < 10; i++)
                {
                    if (rotated) Game1.explosions.Add(new Explosion(Pos + new Vector2(random.Next(-Size.X, Size.X / 2), random.Next(-Size.Y, Size.Y / 2)), tmpExplosionSize, bloodColor));
                    else Game1.explosions.Add(new Explosion(Pos + new Vector2(random.Next(-Size.X / 2, Size.X / 2), random.Next(-Size.Y / 2, Size.Y / 2)), tmpExplosionSize, bloodColor));
                }
                destroy = true;
            }
            foreach (Projectile p in Game1.projectiles)
            {
                if (type == 1 && Hp > 5)
                {
                    if (p.HitBox().Intersects(turretHitBox) && !p.enemy && p.Damege > 0)
                    {
                        p.OnImpact();
                        if (hurtCount <= 0)
                        {
                            Hp -= (sbyte)p.Damege;
                            hurtCount = 1;
                        }
                        p.destroy = true;
                    }
                }
                else
                {
                    if (p.HitBox().Intersects(HitBox()) && !p.enemy && type != 2 && p.Damege > 0)
                    {
                        if (type == 3) Speed = -5;
                        if (hurtCount <= 0)
                        {
                            Hp -= (sbyte)p.Damege;
                            hurtCount = 1;
                        }
                        p.OnImpact();
                        p.destroy = true;
                    }
                }
            }
        }

        public void AttackUpdate()
        {
            Random random = new Random();

            foreach(Player p in Game1.players)
            {
                if (attackCount >= maxAttackCount && type != 1 && !p.invisible)
                {
                    if (DistanceTo(p.Pos) <= attackDistance && type != 4) p.dead = true;
                    attackCount = 0;
                }
                attacking = ((DistanceTo(p.Pos) <= attackDistance));
            }

            if (type != 1)
            {
                if (attacking)
                    attackCount += 1;
                else
                    attackCount = 0;
            }

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
                    Speed = Lerp(Speed, orginalSpeed, 0.1f);
                    TurnTwoards();
                    break;
                case 1:
                    if (Hp > 5) attackCount += 1;
                    else attackCount = 0;

                    foreach(Player p in Game1.players)
                    {
                        target = new Vector2(Lerp(target.X, p.GetCenter.X, 0.05f), Lerp(target.Y, p.GetCenter.Y, 0.05f));
                        if (p.HitBox().Intersects(HitBox()) && !p.invisible)
                            p.dead = true;
                    }

                    shootAngle = AimAt(target, false);

                    if (attackCount == maxAttackCount || attackCount == maxAttackCount + 8 || attackCount == maxAttackCount + 16)
                    {
                        Game1.projectiles.Add(new Projectile(turretPos + new Vector2(-4, -3.5f), shootAngle+random.Next(-16, 17), 5, 1, 0, 0, true, Z+0.001f));
                    }
                    attackCount = (attackCount >= (short)(maxAttackCount + 32)) ? (short)0 : attackCount;
                    break;
                case 2:
                    foreach(Player p in Game1.players)
                    {
                        if(p.HitBox().Intersects(HitBox()))
                        {
                            Game1.explosions.Add(new Explosion(GetCenter + new Vector2(-32, -32), 64, Color.Orange, true, true));
                            destroy = true;
                        }
                    }
                    break;
                case 3:
                    foreach (Player p in Game1.players)
                    {
                        target = new Vector2(Lerp(target.X, p.Pos.X, 0.08f), Lerp(target.Y, p.Pos.Y, 0.08f));
                        if(!attacking) Speed = Lerp(Speed, orginalSpeed, 0.04f);
                        else Speed = 0;
                    }
                    Angle = AimAt(target, false);
                    AngleMath();
                    Pos += Vel;
                    break;
                case 4:
                    AngleMath();
                    shootAngle = AimAt(target, false);
                    Angle = shootAngle;
                    Pos += Vel;
                    foreach(Player p in Game1.players)
                    {
                        target = new Vector2(Lerp(target.X, p.Pos.X, 0.07f), Lerp(target.Y, p.Pos.Y, 0.07f));
                    }
                    Rotation = shootAngle;
                    if(attacking)
                    {
                        if(attackCount == 32 || attackCount == 48)
                        {
                            for(int i = -1; i < 2; i++)
                            {
                                Game1.projectiles.Add(new Projectile(Pos, shootAngle + (i * -16), 3, 0, 0, 1, true, Z-0.01f));
                            }
                        }
                        AnimationCount = 0;
                        CurrentFrame = MinFrame;
                        Speed = Lerp(Speed, 0, 0.05f);
                    }
                    else
                    {
                        Speed = Lerp(Speed, orginalSpeed, 0.05f);
                    }
      
                    //Speed = Lerp(Speed, orginalSpeed, 0.1f);
                    //Console.WriteLine(Speed);
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
            Random random = new Random();

            if (Game1.players[0].dead)
            {
                attackCount = 0;
            }

            if (type != 2) ZOrder();
            AttackUpdate();
            CheckHealth();
            HurtUpdate();
            if (!Game1.players[0].dead) Animate();
            AnimationCount += 1;
            if (MaxFrame > 0) SpriteCoords = new Point(Frame(CurrentFrame, Size.X) + animationOffset, SpriteCoords.Y);

            if (type == 1 && moveToPosCount >= 1)
            {
                foreach(Door d in Game1.doors)
                {
                    if (new Rectangle((int)d.Pos.X, (int)d.Pos.Y, 32, 32).Intersects(HitBox()))
                    {
                        foreach(Door d2 in Game1.doors)
                        {
                            if (d2.Tag == d.Tag) d2.open = true;
                        }
                    }
                }
            }

            switch(type)
            {
                case 1:
                    turretHitBox = new Rectangle((int)turretPos.X-16, (int)turretPos.Y-16, 32, 32);
                    turretPos = Pos;

                    if(moveToPosCount <= 0) Rotation = AimAt(tankTarget, false);
                    else Rotation = AimAt(new Vector2(320, 240), false);

                    if (moveToPosCount >= 1) moveToPosCount += 1;

                    if (moveToPosCount >= 128*3 && OnScreen()) moveToPosCount = 0;

                    tankTarget = new Vector2(Lerp(tankTarget.X, newTankTarget.X, 0.007f), Lerp(tankTarget.Y, newTankTarget.Y, 0.007f));

                    changeDirectionCount += 1;
                    if(changeDirectionCount >= 128)
                    {
                        newTankTarget = new Vector2(random.Next(640 - 128), random.Next(480 - 128));
                        if (DistanceTo(newTankTarget) >= 128)
                            changeDirectionCount = 0;
                    }
                    
                    if(moveToPosCount <= 0)
                    {
                        if(Game1.currentRoom.tileIntersection(HitBox(), 4))
                        {
                            changeDirectionCount = 128;
                        }
                        foreach(Door d in Game1.doors)
                        {
                            if(!d.open && d.HitBox().Intersects(HitBox()))
                            {
                                changeDirectionCount = 128;
                            }
                        }
                    }

                    AngleMath();
                    Angle = Rotation;
                    Pos += Vel;
                    break;
            }

            if(type != 1)
            foreach(Enemy e in Game1.enemies)
            {
                if(e.HitBox().Intersects(HitBox()) && e.type == type)
                {
                    if(e.tag < tag && e.tag != tag)
                    {
                        //Pos -= Vel;
                        Speed -= 0.1f;
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

        public void Draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            if(type == 1)
            {
                if(Hp > 5) spriteBatch.Draw(spritesheet, turretPos, new Rectangle(265, 331, 32, 32), color, (shootAngle * (float)Math.PI/180), new Vector2(turretHitBox.Width/2, turretHitBox.Height/2), 1.0f, SpriteEffects.None, Z+0.01f);
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
                    Hp = 2;
                    MaxAnimationCount = 8;
                    MaxFrame = 4;
                    Angle = random.Next(-180, 180);
                    Speed = 1.5f;
                    turnSpeed = 2f;
                    SpriteCoords = new Point(1, 331);
                    SetSize(32);
                    break;
                case 1:
                    rotated = true;
                    SetSize(64);
                    Hp = 10;
                    SpriteCoords = new Point(199, 331);
                    Speed = 0.5f;
                    maxAttackCount = 32;
                    bloodColor = Color.Orange;
                    break;
                case 2:
                    MaxFrame = 6;
                    SpriteCoords = new Point(232, 265);
                    SetSize(16);
                    animationOffset = (short)(SpriteCoords.X - 1);
                    Hp = 1;
                    Z = 0.01f;
                    MaxAnimationCount = 8;
                    break;
                case 3:
                    SpriteCoords = new Point(67, 496);
                    SetSize(24);
                    MaxFrame = 14;
                    MaxAnimationCount = 4;
                    animationOffset = (short)(SpriteCoords.X - 1);
                    Speed = 2;
                    Hp = 2;
                    attackDistance = 8*2;
                    maxAttackCount = 16;
                    bloodColor = Color.Orange;
                    break;
                case 4:
                    SpriteCoords = new Point(199, 430);
                    SetSize(32);
                    MinFrame = 6;
                    MaxFrame = (short)(MinFrame + 6);
                    MaxAnimationCount = 8;
                    CurrentFrame = MinFrame;
                    Hp = 2;
                    attackDistance = 128+32;
                    maxAttackCount = 64;
                    Speed = 1;
                    rotated = true;
                    break;
            }
        }
    }
}
