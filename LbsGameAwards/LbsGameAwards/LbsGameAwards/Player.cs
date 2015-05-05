using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LbsGameAwards
{
    class Player : GameObject
    {
        public int Score { private set; get; }

        float friction;
        float maxSpeed;

        public bool inputActive;
        public bool dead;
        public bool invisible;

        byte respawnCount;
        byte maxRespawnCount;
        byte direction;
        byte shootDirection;
        public byte Lives { private set; get; }
        public byte GunType { get; private set; }

        short fireRate;
        short maxFireRate;
        short invisibleCount;
        short maxInvisibleCount;
        short spawnCount;
        short maxSpawnCount = 128*2;
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
            GunType = 5;
            MaxFrame = 4;
            MaxAmmo = 50;
            Ammo = MaxAmmo;
            MaxAnimationCount = 4;
            Lives = 7;
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
                        shootDirection = 1;
                    }
                    else if(keyboard.IsKeyDown(shootDown))
                    {
                        shootDirection = 7;
                    }
                    else
                    {
                        shootDirection = 0;
                    }
                }
                if (keyboard.IsKeyDown(shootLeft))
                {
                    if (keyboard.IsKeyDown(shootUp))
                    {
                        shootDirection = 3;
                    }
                    else if (keyboard.IsKeyDown(shootDown))
                    {
                        shootDirection = 5;
                    }
                    else
                    {
                        shootDirection = 4;
                    }
                }
                if(!keyboard.IsKeyDown(shootLeft) && !keyboard.IsKeyDown(shootRight))
                {
                    if (keyboard.IsKeyDown(shootDown)) shootDirection = 6;
                    if (keyboard.IsKeyDown(shootUp)) shootDirection = 2;
                }
                
                if ((keyboard.IsKeyDown(shootDown) || keyboard.IsKeyDown(shootUp) || keyboard.IsKeyDown(shootRight) || keyboard.IsKeyDown(shootLeft)))
                {
                    if(GunType == 0 && fireRate <= 0)
                    {
                        Game1.projectiles.Add(new Projectile(GetCenter + new Vector2(-4, -8), shootDirection * -45, 7, 1, 0, 0, false));
                        fireRate = 1;
                    }

                    if (GunType == 1)
                    {
                        if(fireRate == 0 || fireRate == 8 || fireRate == 8*2 ) Game1.projectiles.Add(new Projectile(GetCenter + new Vector2(-4, -8), (shootDirection * -45)+random.Next(-8, 9), 7, 1, 0, 0, false));
                        fireRate += 1;
                    }

                    if (GunType == 2 && fireRate <= 0)
                    {
                        for (int i = -1; i < 2; i++ )
                            Game1.projectiles.Add(new Projectile(GetCenter + new Vector2(-4, -8),(shootDirection * -45)+i*8, 7, 1, 0, 0, false));
                        fireRate = 1;
                    }

                    if (GunType == 3 && fireRate <= 0)
                    {
                        Game1.projectiles.Add(new Projectile(GetCenter + new Vector2(-4, -8), (shootDirection * -45) + random.Next(-16, 17), 15 + random.Next(-8, 5), 1, 1, 1, false));
                        fireRate = 1;
                    }
                    if (GunType == 4)
                    {
                        if (fireRate <= maxFireRate / 2) Game1.projectiles.Add(new Projectile(Pos+new Vector2(0, -5), (shootDirection * -45) + random.Next(-8, 9), random.Next(5, 11), 1, 0, 2, false));
                        fireRate += 1;
                    }
                    if (GunType == 5 && fireRate <= 0)
                    {
                        Game1.projectiles.Add(new Projectile(GetCenter, (shootDirection * -45) + random.Next(-4, 4), random.Next(1, 3), 1, 2, 3, false));
                        fireRate += 1;
                    }
                }
                else
                {
                    if(GunType == 1)
                    {
                        fireRate = 0;
                    }
                }
            }
        }

        public void CheckHealth()
        {
            foreach(Projectile p in Game1.projectiles)
            {
                if(p.HitBox().Intersects(HitBox()) && p.enemy)
                {
                    dead = true;
                    p.destroy = true;
                }
            }

            spawnCount = (dead) ? (short)(spawnCount + 1) : spawnCount;
            if(spawnCount == maxSpawnCount/2-1)
            {
                Game1.explosions.Add(new Explosion(Pos, 32, Color.Red));
                Pos = new Vector2(-64, 240 - 16);
                CurrentFrame = 0;
                spawnCount += 1;
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
                        if (GunType != p.Type)
                        {
                            GunType = (byte)(p.Type + 1);
                            Ammo = MaxAmmo;
                        }
                    }
                    else
                    {
                        if (p.Type == 0) Lives += 1;
                    }
                    p.destroy = true;
                }
            }
        }

        public void TileCollisionLogic()
        {
            Rectangle playerBox = new Rectangle((int)Pos.X, (int)Pos.Y + 32-20, 32, 20);

            // Funkar perfekt i blitzplus, fan vad jag hatar programmering ibland :^)

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

        public void Update()
        {
            Z = ZOrder();

            TileCollisionLogic();
            AssignFireRates();
            Movment();
            Input();
            CheckHealth();
            PowerUpLogic();

            foreach(Loot l in Game1.loots)
            {
                if(l.HitBox().Intersects(HitBox()))
                {
                    Score += l.Worth;
                    Game1.textEffects.Add(new TextEffect(l.Pos, "+" + l.Worth.ToString(), l.Pos - new Vector2(3, 32), 0.05f, 1, 1, Color.White, 100, false));
                    l.destroy = true;
                }
            }

            if(!dead) SpriteCoords = new Point(Frame(CurrentFrame), Frame(shootDirection));
            Animate();

            GunType = (GunType != 0 && Ammo <= 0) ? (byte)0 : GunType;

            if(GunType != 1) fireRate = (fireRate >= 1) ? (short)(fireRate + 1) : fireRate;

            if(fireRate == 1 && GunType != 4)
            {
                Ammo -= 1;
            }

            if(fireRate >= maxFireRate)
            {
                if (GunType == 4) Ammo -= 1;
                fireRate = 0;
            }
        }
    }
}
