using System;
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
        sbyte hp;
        
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
                for (int i = 0; i < 10; i++)
                {
                    Game1.explosions.Add(new Explosion(Pos+new Vector2(random.Next(-Size.X/2, Size.X/2), random.Next(-Size.Y/2, Size.Y/2)), (byte)Size.X, bloodColor));
                }
                destroy = true;
            }
            foreach (Projectile p in Game1.projectiles)
            {
                if (type == 1 && hp > 5)
                {
                    if (p.HitBox().Intersects(turretHitBox) && !p.enemy)
                    {
                        if (hurtCount <= 0)
                        {
                            hp -= (sbyte)p.Damege;
                            hurtCount = 1;
                        }
                        p.destroy = true;
                    }
                }
                else
                {
                    if (p.HitBox().Intersects(HitBox()) && !p.enemy)
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
        }

        public void AttackUpdate()
        {
            Random random = new Random();
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
                case 1:
                    if (hp > 5) attackCount += 1;
                    else attackCount = 0;

                    foreach(Player p in Game1.players)
                    {
                        target = new Vector2(Lerp(target.X, p.GetCenter.X, 0.05f), Lerp(target.Y, p.GetCenter.Y, 0.05f));
                    }

                    shootAngle = AimAt(target, false);

                    if (attackCount == maxAttackCount || attackCount == maxAttackCount + 8 || attackCount == maxAttackCount + 16)
                    {
                        Game1.projectiles.Add(new Projectile(turretPos + new Vector2(-4, -3.5f), shootAngle+random.Next(-16, 17), 5, 1, 0, 0, true, Z+0.001f));
                    }
                    attackCount = (attackCount >= (short)(maxAttackCount + 32)) ? (short)0 : attackCount;
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

            Z = GetCenter.Y / 1000;
            AttackUpdate();
            CheckHealth();
            HurtUpdate();
            Animate();
            AnimationCount += 1;
            if(MaxFrame > 0) SpriteCoords = new Point(Frame(CurrentFrame), SpriteCoords.Y);

            switch(type)
            {
                case 1:
                    turretHitBox = new Rectangle((int)turretPos.X-16, (int)turretPos.Y-16, 32, 32);
                    turretPos = Pos;

                    Rotation = AimAt(tankTarget, false);

                    tankTarget = new Vector2(Lerp(tankTarget.X, newTankTarget.X, 0.007f), Lerp(tankTarget.Y, newTankTarget.Y, 0.007f));

                    changeDirectionCount += 1;
                    if(changeDirectionCount >= 128)
                    {
                        newTankTarget = new Vector2(random.Next(640 - 64), random.Next(480 - 64));
                        if (DistanceTo(newTankTarget) >= 128)
                            changeDirectionCount = 0;
                    }

                    AngleMath();
                    Angle = Rotation;
                    Pos += Vel;

                    break;
            }

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

        public void Draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            if(type == 1)
            {
                if(hp > 5) spriteBatch.Draw(spritesheet, turretPos, new Rectangle(265, 331, 32, 32), color, (shootAngle * (float)Math.PI/180), new Vector2(turretHitBox.Width/2, turretHitBox.Height/2), 1.0f, SpriteEffects.None, Z+0.01f);
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
                case 1:
                    rotated = true;
                    SetSize(64);
                    hp = 10;
                    SpriteCoords = new Point(199, 331);
                    Speed = 0.5f;
                    maxAttackCount = 32;
                    break;
            }
        }
    }
}
