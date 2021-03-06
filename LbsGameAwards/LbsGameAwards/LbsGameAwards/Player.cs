﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LbsGameAwards
{
    class Player : GameObject
    {
        public int Score { set; get; }

        float friction;
        float orginalFricton;
        float maxSpeed;

        public bool inputActive;
        public bool dead;
        public bool invisible = true;
        public bool transitioning;

        byte respawnCount;
        byte maxRespawnCount;
        byte direction;
        public byte ShootDirection { get; private set; }
        public sbyte Lives { set; get; }
        public byte GunType { get; private set; }

        public short FireRate { get; private set; }
        short maxFireRate;
        short invisibleCount;
        short maxInvisibleCount = 128*2;
        short spawnCount;
        short maxSpawnCount = 128*2;
        short transitionCount;
        short maxTransitionCount;
        public short Ammo { get; private set; }
        public short MaxAmmo { get; private set; }

        Keys walkLeft = Keys.A;
        Keys walkRight = Keys.D;
        Keys walkDown = Keys.S;
        Keys walkUp = Keys.W;

        Keys shootLeft = Keys.Left;
        Keys shootRight = Keys.Right;
        Keys shootDown = Keys.Down;
        Keys shootUp = Keys.Up;

        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        public Player()
        {
            Pos = new Vector2(320, 240);
            SetSize(32);
            SpriteCoords = new Point(1, 1);
            Z = 0.1f;
            Speed = 0.5f;
            maxSpeed = 3;
            friction = 0.90f;
            inputActive = true;
            GunType = 2;
            MaxFrame = 4;
            MaxAmmo = 50;
            Ammo = MaxAmmo;
            MaxAnimationCount = 4;
            Lives = 20;
            orginalFricton = friction;
        }

        public void Movment()
        {
            VelX *= friction;
            VelY *= friction;

            if (VelX >= 0.1f || VelX <= -0.1f)
                Pos += new Vector2(VelX, 0);
            if (VelY >= 0.1f || VelY <= -0.1f)
                Pos += new Vector2(0, VelY);
        }

        public void Input()
        {
            Random random = new Random();

            prevKeyboard = keyboard;
            keyboard = Keyboard.GetState();

            if(inputActive)
            {
                if(keyboard.IsKeyDown(walkLeft) && VelX > -maxSpeed)
                {
                    VelX -= Speed;
                    if(!keyboard.IsKeyDown(walkDown) && !keyboard.IsKeyDown(walkUp)) AnimationCount += 1;
                }
                if (keyboard.IsKeyDown(walkRight) && VelX < maxSpeed)
                {
                    VelX += Speed;
                    if (!keyboard.IsKeyDown(walkDown) && !keyboard.IsKeyDown(walkUp)) AnimationCount += 1;
                }
                if (keyboard.IsKeyDown(walkUp) && VelY > -maxSpeed)
                {
                    VelY -= Speed;
                    AnimationCount += 1;
                }
                if (keyboard.IsKeyDown(walkDown) && VelY < maxSpeed)
                {
                    VelY += Speed;
                    AnimationCount += 1;
                }

                if (!keyboard.IsKeyDown(walkLeft) && !keyboard.IsKeyDown(walkRight) && !keyboard.IsKeyDown(walkDown) && !keyboard.IsKeyDown(walkUp))
                {
                    CurrentFrame = MinFrame;
                    AnimationCount = 0;
                }

                if(keyboard.IsKeyDown(shootRight))
                {
                    if(keyboard.IsKeyDown(shootUp))
                    {
                        ShootDirection = 1;
                    }
                    else if(keyboard.IsKeyDown(shootDown))
                    {
                        ShootDirection = 7;
                    }
                    else
                    {
                        ShootDirection = 0;
                    }
                }
                if (keyboard.IsKeyDown(shootLeft))
                {
                    if (keyboard.IsKeyDown(shootUp))
                    {
                        ShootDirection = 3;
                    }
                    else if (keyboard.IsKeyDown(shootDown))
                    {
                        ShootDirection = 5;
                    }
                    else
                    {
                        ShootDirection = 4;
                    }
                }
                if(!keyboard.IsKeyDown(shootLeft) && !keyboard.IsKeyDown(shootRight))
                {
                    if (keyboard.IsKeyDown(shootDown)) ShootDirection = 6;
                    if (keyboard.IsKeyDown(shootUp)) ShootDirection = 2;
                }
                
                if ((keyboard.IsKeyDown(shootDown) || keyboard.IsKeyDown(shootUp) || keyboard.IsKeyDown(shootRight) || keyboard.IsKeyDown(shootLeft)))
                {
                    if(GunType == 0 && FireRate <= 0)
                    {
                        Game1.projectiles.Add(new Projectile(GetCenter + new Vector2(-4, -8), ShootDirection * -45, 7, 1, 0, 0, false));
                        FireRate = 1;
                    }

                    if (GunType == 1)
                    {
                        if (FireRate == 0 || FireRate == 8 || FireRate == 8 * 2)
                        {
                            Game1.projectiles.Add(new Projectile(GetCenter + new Vector2(-4, -8), (ShootDirection * -45) + random.Next(-8, 9), 7, 1, 0, 0, false));
                            SoundManager.shoot.Play();
                        }
                        FireRate += 1;
                    }

                    if (GunType == 2 && FireRate <= 0)
                    {
                        SoundManager.shoot.Play();
                        for (int i = -1; i < 2; i++ )
                            Game1.projectiles.Add(new Projectile(GetCenter + new Vector2(-4, -8),(ShootDirection * -45)+i*8, 7, 1, 0, 0, false));
                        FireRate = 1;
                    }

                    if (GunType == 3 && FireRate <= 0)
                    {
                        SoundManager.shoot.Play();
                        Game1.projectiles.Add(new Projectile(GetCenter + new Vector2(-4, -8), (ShootDirection * -45) + random.Next(-16, 17), 15 + random.Next(-8, 5), 1, 1, 1, false));
                        FireRate = 1;
                    }
                    if (GunType == 4)
                    {
                        if (FireRate <= maxFireRate / 2) Game1.projectiles.Add(new Projectile(Pos+new Vector2(0, -5), (ShootDirection * -45) + random.Next(-8, 9), random.Next(5, 11), 1, 0, 2, false));
                        FireRate += 1;
                    }
                    if (GunType == 5 && FireRate <= 0)
                    {
                        SoundManager.shoot.Play();
                        Game1.projectiles.Add(new Projectile(GetCenter, (ShootDirection * -45) + random.Next(-4, 4), random.Next(1, 3), 100, 2, 3, false));
                        FireRate += 1;
                    }
                }
                else
                {
                    if(GunType == 1)
                    {
                        FireRate = 0;
                    }
                }
            }
        }

        public void CheckHealth()
        {
            foreach(Projectile p in Game1.projectiles)
            {
                if (p.HitBox().Intersects(HitBox()) && p.enemy && !invisible)
                {
                    dead = true;
                    p.destroy = true;
                }
            }

            if (spawnCount == 2 && Lives - 1 >= 0)
            {
                SoundManager.dead.Play();
                Game1.textEffects.Add(new TextEffect(new Vector2(250, 700), "GET READY!", new Vector2(250, 240), 0.06f, 2, 1, Color.White, 128+64, true));
            }

            spawnCount = (dead) ? (short)(spawnCount + 1) : spawnCount;

            if(spawnCount == maxSpawnCount/2-1)
            {
                if (Lives - 1 >= 0)
                {
                    Game1.explosions.Add(new Explosion(Pos, 32, Color.Red));
                    Pos = new Vector2(-64, 240 - 16);
                    CurrentFrame = 0;
                    spawnCount += 1;
                }
                else
                {
                    Globals.gameState = GameStates.gameOver;
                }
            }
            if(dead && spawnCount < maxSpawnCount/2)
            {
                GunType = 0;
                Ammo = 0;
                spawnCount += 1;

                inputActive = false;
                SpriteCoords = new Point(Frame(CurrentFrame), 265);

                MaxAnimationCount = 8;
                AnimationCount += 1;

                MaxFrame = 7;
                if (CurrentFrame == MaxFrame-1)
                    AnimationCount = 0;
            }
            if (dead && spawnCount > maxSpawnCount / 2)
            {
                MaxFrame = 4;
                MaxAnimationCount = (short)(Lerp(Pos.X, 320, 0.009f)/10);
                SpriteCoords = new Point(Frame(CurrentFrame), 1);
                AnimationCount += 1;
                Pos = new Vector2(Lerp(Pos.X, 320, 0.009f), Pos.Y);
                invisible = true;
                invisibleCount = 1;
                foreach(Door d in Game1.doors)
                {
                    if(d.Tag == 0)
                    {
                        d.open = true;
                    }
                }
            }

            if (spawnCount >= maxSpawnCount)
            {
                SpriteCoords = new Point(Frame(CurrentFrame), 1);
                CurrentFrame = 0;
                dead = false;
                inputActive = true;
                MaxAnimationCount = 4;
                Lives -= 1;
                spawnCount = 0;
            }
        }

        public void AssignFireRates()
        {
            switch(GunType)
            {
                case 0:
                    maxFireRate = 24;
                    break;
                case 1:
                    maxFireRate = 64;
                    break;
                case 2:
                    maxFireRate = 32;
                    break;
                case 3:
                    maxFireRate = 4;
                    break;
                case 4:
                    maxFireRate = 32;
                    break;
                case 5:
                    maxFireRate = 48;
                    break;
            }
        }

        public void PowerUpLogic()
        {
            foreach(PowerUp p in Game1.powerUps)
            {
                if(p.HitBox().Intersects(HitBox()))
                {
                    if (!p.special)
                    {
                        if (GunType != p.Type+1)
                        {
                            GunType = (byte)(p.Type + 1);
                            Ammo = MaxAmmo;
                        }
                        else
                        {
                            Score += 1000;
                            Ammo = MaxAmmo;
                        }
                    }
                    else
                    {
                        if (p.Type == 0) Lives += 1;
                        if (p.Type == 2) invisible = true;
                        if (p.Type == 1) Game1.helpers.Add(new Helper(Pos + new Vector2(32, 32)));
                    }
                    p.destroy = true;
                }
            }
        }

        public void TileCollisionLogic()
        {
            Rectangle playerBox = new Rectangle((int)Pos.X, (int)Pos.Y + 32-20, 32, 20);

            // Funkar perfekt i blitzplus, fan vad jag hatar programmering ibland :^)

            foreach(Door d in Game1.doors)
            {
                if (!d.open || !Game1.currentRoom.cleard)
                {
                    if (d.HitBox().Intersects(new Rectangle((int)Pos.X, (int)((Pos.Y + 12) + VelY), 32, 20)))
                    {
                        Pos += new Vector2(0, -VelY);
                        VelY = 0;
                    }
                    if (d.HitBox().Intersects(new Rectangle((int)(Pos.X + VelX), (int)Pos.Y + 12, 32, 20)))
                    {
                        Pos += new Vector2(-VelX, 0);
                        VelX = 0;
                    }
                }
            }

            if (Game1.currentRoom.tileIntersection(new Rectangle((int)Pos.X+10, (int)Pos.Y+29, 15, 4), 6) && !invisible)
            {
                dead = true;
            }

            if (Game1.currentRoom.tileIntersection(playerBox, 5))
            {
                friction = 0.999f;
            }
            else
            {
                friction = orginalFricton;
            }

            if (Game1.currentRoom.tileIntersection(new Rectangle((int)Pos.X, (int)((Pos.Y + 12) + VelY), 32, 20), 4))
            {
                Pos += new Vector2(0, -VelY);
                VelY = 0;
            }

            if (Game1.currentRoom.tileIntersection(new Rectangle((int)(Pos.X + VelX), (int)Pos.Y + 12, 32, 20), 4))
            {
                Pos += new Vector2(-VelX, 0);
                VelX = 0;
            }
        }
        
        void TransitionUpdate()
        {
            if(transitionCount >= 1)
            {
                if (direction == 0) SpriteCoords = new Point(Frame(CurrentFrame), Frame(4));
                if (direction == 1) SpriteCoords = new Point(Frame(CurrentFrame), Frame(2));
                if (direction == 2) SpriteCoords = new Point(Frame(CurrentFrame), Frame(0));
                if (direction == 3) SpriteCoords = new Point(Frame(CurrentFrame), Frame(6));

                Pos = new Vector2(Lerp(Pos.X, 320, 0.01f), Lerp(Pos.Y, 240, 0.01f));
                
                transitionCount += 1;

                AnimationCount += 1;

                if(transitionCount >= 128)
                {
                    inputActive = true;
                    transitioning = false;
                    transitionCount = 0;
                }
            }

            if (Game1.currentRoom.cleard && !transitioning && !dead)
            {
                foreach(Door d in Game1.doors)
                {
                    if(new Rectangle((int)d.Pos.X, (int)d.Pos.Y, 32, 32).Intersects(HitBox()) && d.open)
                    {
                        inputActive = false;
                        // TODO: SAVE CLEARED ROOMS IN A LIST
                        Globals.currentRoom = Game1.currentRoom.doorLeadsTo[d.Tag];
                        if(d.Tag == 0)
                        {
                            Pos = new Vector2(640 + 228, 480/2-16);
                        }
                        if (d.Tag == 2)
                        {
                            Pos = new Vector2(-228, 480 / 2 - 16);
                        }
                        if (d.Tag == 1)
                        {
                            Pos = new Vector2(640/2-16, 480 + 228);
                        }
                        if (d.Tag == 3)
                        {
                            Pos = new Vector2(640 / 2 - 16, -228);
                        }
                        direction = d.Tag;
                        transitioning = true;
                        Globals.transition = true;
                        CurrentFrame = 0;
                        transitionCount = 1;
                    }
                }
            }
        }

        public void DrawShield(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            if(invisible)
            {
                short shieldX = (invisibleCount >= (short)(maxInvisibleCount / 2)) ? (short)34 : (short)1;
                spriteBatch.Draw(spritesheet, Pos, new Rectangle(shieldX, 298, 32, 32), Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, Z + 0.01f);
            }
        }

        public void Update()
        {
            Z = ZOrder();

            if (GunType == 0) Ammo = 0;

            if (invisible && invisibleCount < 1) invisibleCount = 1;
            if (invisibleCount >= 1) invisibleCount += 1;
            if(invisibleCount >= maxInvisibleCount)
            {
                invisibleCount = 0;
                invisible = false;
            }

            TransitionUpdate();
            TileCollisionLogic();
            AssignFireRates();
            Movment();
            Input();
            CheckHealth();
            PowerUpLogic();

            foreach(Loot l in Game1.loots)
            {
                if(l.HitBox().Intersects(HitBox()) && !l.pickedUp)
                {
                    Score += l.Worth;
                    Game1.textEffects.Add(new TextEffect(l.Pos, "+" + l.Worth.ToString(), l.Pos - new Vector2(3, 32), 0.05f, 1, 1, Color.White, 100, false));
                    l.pickedUp = true;
                }
            }

            if(!dead) SpriteCoords = new Point(Frame(CurrentFrame), Frame(ShootDirection));
            Animate();

            GunType = (GunType != 0 && Ammo <= 0) ? (byte)0 : GunType;

            if(GunType != 1) FireRate = (FireRate >= 1) ? (short)(FireRate + 1) : FireRate;

            if(FireRate == 2 && GunType != 4)
            {
                Ammo -= 1;
            }

            if(FireRate >= maxFireRate)
            {
                if (GunType == 4) Ammo -= 1;
                FireRate = 0;
            }
        }
    }
}
