using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LbsGameAwards
{
    class Room : GameObject
    {
        public byte Tag { private set; get; }
        byte amountOfTypes;
        byte[] enemyTypes;
        byte[] amountOfEnemies;
        byte doorToSpawnAt;
        byte randomWaveSize;

        short[] enemySpawnCount;
        short[] enemySpawnDelay;

        short totalAmountOfEnemy;

        public byte[] doorLeadsTo = new byte[4];

        int[,] map;

        string mapPath;
        string doorLine;
        string[] enemyLine;

        public bool cleard;
        bool hasSpawnedClearText;
        bool spawnMines;

        public Room(string mapPath2, byte tag2)
        {
            mapPath = mapPath2;
            Tag = tag2;

            Globals.ClearScreen();

            if (mapPath != "")
            {
                map = LoadLevel(mapPath);
                LoadRoom(mapPath + "Room");

                for (int i = 0; i < amountOfTypes; i++)
                {
                    totalAmountOfEnemy += amountOfEnemies[i];
                }

                for(int i = 0; i < 5; i++)
                {
                    Game1.doors.Add(new Door(new Vector2(0, 160 + 32 * i), false));
                    Game1.doors.Add(new Door(new Vector2(640-32, 160 + 32 * i), false));

                    Game1.doors.Add(new Door(new Vector2(240 + 32 * i, 0), true));
                    Game1.doors.Add(new Door(new Vector2(240 + 32 * i, 480-32), true));
                }
            }
        }

        public void SpawnEnemies()
        {
            Random random = new Random();
            for(int i = 0; i < amountOfTypes; i++)
            {
                enemySpawnCount[i] += 1;
                if(enemySpawnCount[i] >= enemySpawnDelay[i])
                {
                    doorToSpawnAt = (byte)random.Next(0, 4);
                    if (enemyTypes[i] != 1)
                    {
                        randomWaveSize = (byte)random.Next(1, 5);
                        for (int j = 0; j < randomWaveSize; j++)
                        {
                            doorToSpawnAt = (byte)random.Next(0, 4);
                            if (doorToSpawnAt == 0)
                            {
                                Game1.enemies.Add(new Enemy(new Vector2(-32, 240 - 16), enemyTypes[i]));
                            }
                            if (doorToSpawnAt == 1)
                            {
                                Game1.enemies.Add(new Enemy(new Vector2(320 - 16, -32), enemyTypes[i]));
                            }
                            if (doorToSpawnAt == 2)
                            {
                                Game1.enemies.Add(new Enemy(new Vector2(640 + 32, 240 - 16), enemyTypes[i]));
                            }
                            if (doorToSpawnAt == 3)
                            {
                                Game1.enemies.Add(new Enemy(new Vector2(320 - 16, 480 + 32), enemyTypes[i]));
                            }
                        }
                    }
                    else
                    {
                        if (doorToSpawnAt == 0)
                        {
                            Game1.enemies.Add(new Enemy(new Vector2(-64, 240 - 32), enemyTypes[i]));
                        }
                        if (doorToSpawnAt == 1)
                        {
                            Game1.enemies.Add(new Enemy(new Vector2(320 - 32, -64), enemyTypes[i]));
                        }
                        if (doorToSpawnAt == 2)
                        {
                            Game1.enemies.Add(new Enemy(new Vector2(640 + 64, 240 - 32), enemyTypes[i]));
                        }
                        if (doorToSpawnAt == 3)
                        {
                            Game1.enemies.Add(new Enemy(new Vector2(320 - 32, 480 + 64), enemyTypes[i]));
                        }
                    }
                    foreach(Door d in Game1.doors)
                    {
                        if(d.Tag == doorToSpawnAt)
                        {
                            d.open = true;
                        }
                    }
                    totalAmountOfEnemy -= 1;
                    enemySpawnCount[i] = 0;
                }
            }
        }

        public void Update()
        {
            if(totalAmountOfEnemy > 0) SpawnEnemies();
            else if(Game1.enemies.Count <= 0) cleard = true;

            if(cleard)
            { 
                if(!hasSpawnedClearText)
                {
                    Game1.textEffects.Add(new TextEffect(new Vector2(250, -64), "ROOM CLEARED", new Vector2(250, 240), 0.07f, 0, 1, Color.Gold, 0, true));
                    hasSpawnedClearText = true;
                }
                foreach (Door d in Game1.doors)
                {
                    d.open = true;
                }
            }
        }

        public void Draw(SpriteBatch spritebatch, Texture2D spritesheet, SpriteFont font)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                for(int y = 0; y < map.GetLength(0); y++)
                {
                    spritebatch.Draw(spritesheet, new Vector2(x * 16, y * 16), new Rectangle(map[y, x] * 16, 562, 16, 16), Color.White);
                    //spritebatch.DrawString(font, map[y, x].ToString(), new Vector2(x * 16, y * 16), new Color(map[y, x]*50, 0, 0));
                }
            }
        }

        public bool tileIntersection(Rectangle hitBox, byte tileId)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    if (map[y, x] == tileId && hitBox.Intersects(new Rectangle(x * 16, y * 16, 16, 16)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void LoadRoom(string path)
        {
            StreamReader sr = new StreamReader(path);
            amountOfTypes = byte.Parse(sr.ReadLine());

            amountOfEnemies = new byte[amountOfTypes];
            enemyTypes = new byte[amountOfTypes];
            enemySpawnDelay = new short[amountOfTypes];
            enemySpawnCount = new short[amountOfTypes];

            enemyLine = new string[amountOfTypes];

            for (int i = 0; i < amountOfTypes; i++)
            {
                enemyLine[i] = sr.ReadLine();
            }

            for (int i = 0; i < amountOfTypes; i++)
            {
                enemyTypes[i] = byte.Parse(enemyLine[i].Split('-')[0]);
                amountOfEnemies[i] = byte.Parse(enemyLine[i].Split('-')[1]);
                enemySpawnDelay[i] = byte.Parse(enemyLine[i].Split('-')[2]);
            }

            doorLine = sr.ReadLine();

            for (int i = 0; i < 4; i++)
            {
                doorLeadsTo[i] = byte.Parse(doorLine.Split('-')[i]);
            }

            spawnMines = Convert.ToBoolean(sr.ReadLine());

            sr.Dispose();
        }

        public void PrintRoom()
        {
            for(int i = 0; i < amountOfTypes; i++)
            {
                Console.Write(enemyTypes[i] + "-");
                Console.Write(amountOfEnemies[i] + "-");
                Console.Write(enemySpawnDelay[i]);
                Console.WriteLine("\n");
            }

            Console.WriteLine("\n");

            for(int i = 0; i < 4; i++)
            {
                if(i != 3) Console.Write(doorLeadsTo[i] + "-");
                else Console.Write(doorLeadsTo[i]);
            }

            Console.WriteLine("\n");

            Console.WriteLine(spawnMines);
        }

        public int[,] LoadLevel(string name)
        {
            int[,] map;
            string mapData = name + ".txt";
            int width = 0;
            int height = File.ReadLines(mapData).Count();

            StreamReader sReader = new StreamReader(mapData);
            string line = sReader.ReadLine();
            string[] tileNo;
            tileNo = line.Split(',');

            width = tileNo.Count();

            map = new int[height, width];

            sReader = new StreamReader(mapData);

            for (int y = 0; y < height; y++)
            {
                line = sReader.ReadLine();
                tileNo = line.Split(',');

                for (int x = 0; x < width; x++)
                {
                    if (tileNo[x] != "" || tileNo[x] != " ")
                    {
                        map[y, x] = Convert.ToInt32(tileNo[x]);
                    }
                }
            }
            sReader.Close();

            return map;
        }
    }
}
