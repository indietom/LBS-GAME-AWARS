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

        short spawnLootCount;
        short maxSpawnLootCount = 128;

        sbyte specifcType;

        byte chanceOfSpecial;

        public void UpdateLootSpawn()
        {
            Random random = new Random();

            if(!Game1.currentRoom.cleard) spawnLootCount += 1;
            if(spawnLootCount >= maxSpawnLootCount && Game1.loots.Count() <= 150)
            {
                spawnLootPile(new Vector2(320 - random.Next(-200, 200), 240 - random.Next(-150, 150)), (byte)random.Next(16, 32), (sbyte)random.Next(-1, 3));
                spawnLootCount = 0;
                maxSpawnLootCount = (short)random.Next(128*2, 128 * 3);
            }
        }

        public void UpdatePowerUpSpawn()
        {
            Random random = new Random();

            if (!Game1.currentRoom.cleard) spawnPowerUpCount += 1;
            if(spawnPowerUpCount >= maxSpawnPowerUpCount)
            {
                chanceOfSpecial = (byte)random.Next(11);
                if(chanceOfSpecial == 7)
                {
                    Game1.powerUps.Add(new PowerUp(new Vector2(random.Next(64, 640-64), random.Next(64, 480-64)), (byte)random.Next(0, 3), true));
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
            else
            {
                for (int i = 0; i < maxRadius / 2; i++)
                {
                    poses[i] = pos2 + new Vector2(random.Next(-maxRadius, maxRadius + 1), random.Next(-maxRadius, maxRadius + 1));
                    Game1.loots.Add(new Loot(poses[i], (byte)specifcType));
                }
            }
        }
    }
}
