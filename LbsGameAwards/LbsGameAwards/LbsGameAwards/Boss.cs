using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Boss : GameObject
    {
        byte currentStage;
        byte direction;

        short hp;
        short maxHp;
        short fireRate;
        short invisibiltyCount;
        short maxInvisibiltyCount = 16;
        short spawnExplosions;
        short maxFireRate;

        Vector2 target;

        public Boss(Vector2 pos2)
        {
            SetSize(127);
            Pos = pos2;

            maxHp = 80;
            hp = maxHp;
        }

        public void Movment()
        {
            switch(currentStage)
            {

            }
        }

        public void Attack()
        {
            Random random = new Random();

            switch(currentStage)
            {
                case 0:
                    maxFireRate = 64+32;

                    fireRate += 1;
                    if(fireRate == maxFireRate-32 || fireRate == maxFireRate- 32+8)
                    {
                        Game1.projectiles.Add(new Projectile(Pos+new Vector2(49, 70), -180+random.Next(-16, 16), random.Next(5, 8), 1, 0, 2, true));
                    }
                    if (fireRate >= maxFireRate) fireRate = 0;
                    break;
                case 1:
                    maxFireRate = 64;

                    fireRate += 1;
                    if(fireRate == maxFireRate-32 || fireRate == maxFireRate- 32+8)
                    {
                        Game1.projectiles.Add(new Projectile(Pos+new Vector2(49, 70), -180+random.Next(-16, 16), random.Next(5, 8), 1, 0, 2, true));
                    }
                    if (fireRate >= maxFireRate) fireRate = 0;
                    break;
                case 2:
                    maxFireRate = 64+16;

                    fireRate += 1;
                    if (fireRate == maxFireRate - 8 || fireRate == maxFireRate - 4)
                    {
                        Game1.projectiles.Add(new Projectile(Pos + new Vector2(49, 70), -180 + random.Next(-16, 16), random.Next(5, 8), 1, 2, 3, true));
                    }
                    if(fireRate == maxFireRate-32 || fireRate == maxFireRate- 32+8)
                    {
                        Game1.projectiles.Add(new Projectile(Pos+new Vector2(49, 70), -180+random.Next(-16, 16), random.Next(5, 8), 1, 0, 2, true));
                    }
                    if (fireRate >= maxFireRate) fireRate = 0;
                    break;
            }
        }

        public void CheckHealth()
        {
            Random random = new Random();

            Rectangle hitBox = new Rectangle((int)Pos.X+30, (int)Pos.Y+11, 61, 83);

            if (hp <= maxHp / 2)
            {
                currentStage = 1;
                spawnExplosions += 1;
            }
            if (hp <= maxHp / 3)
            {
                currentStage = 2;
                spawnExplosions += 1;
            }
            if (hp <= 0) currentStage = 3;

            if(spawnExplosions >= 1 && spawnExplosions < 32)
            {
                Game1.explosions.Add(new Explosion(Pos + new Vector2(random.Next(Size.X), random.Next(Size.Y)), 32, Color.LightGreen));
            }

            foreach(Projectile p in Game1.projectiles)
            {
                if(p.HitBox().Intersects(hitBox) && !p.enemy && p.Damege > 0 && currentStage != 3)
                {
                    if(invisibiltyCount <= 4)
                    {
                        if (hp - p.Damege <= maxHp / 3) spawnExplosions = 1;
                        hp -= p.Damege;
                        invisibiltyCount = 1;
                    }
                    p.destroy = true;
                }
            }

            if (invisibiltyCount >= 1)
            {
                invisibiltyCount += 1;
                color = Color.Red;
            }
            if(invisibiltyCount >= maxInvisibiltyCount)
            {
                color = Color.White;
                invisibiltyCount = 0;
            }
        }

        public void Update()
        {
            Z = ZOrder();

            SpriteCoords = new Point(Frame(CurrentFrame, 127), Frame(currentStage, 127));

            Movment();
            Attack();
            CheckHealth();

            switch(currentStage)
            {

            }
        }
    }
}
