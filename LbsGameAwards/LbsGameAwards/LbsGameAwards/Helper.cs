using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Helper : GameObject 
    {
        short lifeTime;
        short maxLifeTime = 128*3;

        float maxDistance = 32;

        public Helper(Vector2 pos2)
        {
            Pos = pos2;
            SetSize(12);
            SpriteCoords = new Point(166, 34);
        }

        public void Update()
        {
            Random random = new Random();

            Z = ZOrder();

            lifeTime += 1;
            if(lifeTime >= maxLifeTime)
            {
                Game1.explosions.Add(new Explosion(Pos+new Vector2(-16, -16), 32, Color.Orange));
                destroy = true;
            }

            foreach(Player p in Game1.players)
            {
                if (p.dead) lifeTime = maxLifeTime;
                if(DistanceTo(p.Pos) > maxDistance)
                {
                    Pos = new Vector2(Lerp(Pos.X, p.Pos.X, 0.01f), Lerp(Pos.Y, p.Pos.Y, 0.01f));
                }
                if (p.FireRate == 2)
                {
                    if (p.GunType != 3)
                    for (int i = -1; i < 2; i++)
                        Game1.projectiles.Add(new Projectile(GetCenter + new Vector2(-4, -8), (p.ShootDirection * -45) + i * 8, 7, 1, 0, 0, false));
                    else
                    {
                        Game1.projectiles.Add(new Projectile(GetCenter + new Vector2(-4, -8), (p.ShootDirection * -45) + random.Next(-16, 17), 15 + random.Next(-8, 5), 1, 1, 1, false));
                    }

                }
            }
        }
    }
}
