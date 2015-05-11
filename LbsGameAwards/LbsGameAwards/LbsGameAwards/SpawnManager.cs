using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class SpawnManager : GameObject
    {
        short spawnPowerUpCount;
        short maxSpawnPowerUpCount = 128*2;

        byte chanceOfSpecial;

        public void UpdatePowerUpSpawn()
        {
            Random random = new Random();

            spawnPowerUpCount += 1;
            if(spawnPowerUpCount >= maxSpawnPowerUpCount)
            {
                chanceOfSpecial = (byte)random.Next(11);
                if(chanceOfSpecial == 7)
                {
                    Game1.powerUps.Add(new PowerUp(new Vector2(random.Next(64, 640-64), random.Next(64, 480-64)), (byte)random.Next(0, 2), true));
                }
                else
                {
                    Game1.powerUps.Add(new PowerUp(new Vector2(random.Next(64, 640 - 64), random.Next(64, 480 - 64)), (byte)random.Next(0, 5), false));
                }
                spawnPowerUpCount = 0;
                maxSpawnPowerUpCount = (short)random.Next(128, 128*5);
            }
        }

        public void spawnLootPile(Vector2 pos2, byte maxRadius, sbyte specifcType)
        {
            Random random = new Random();

            // This used to be useful
            Vector2[] poses = new Vector2[maxRadius/2];

            if(specifcType < 0)
            {
                for(int i = 0; i < maxRadius/2; i++)
                {
                    poses[i] = pos2 + new Vector2(random.Next(-maxRadius, maxRadius + 1), random.Next(-maxRadius, maxRadius + 1));
                    Game1.loots.Add(new Loot(poses[i], (byte)random.Next(6)));
                }
            }
        }
    }
}
