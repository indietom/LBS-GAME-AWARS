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

        short[] enemySpawnCount;
        short[] enemySpawnDelay;

        byte[] doorLeadsTo = new byte[4];

        int[,] map;

        string mapPath;
        string doorLine;
        string[] enemyLine;

        bool cleard;
        bool spawnMines;

        public Room(string mapPath2, byte tag2)
        {
            mapPath = mapPath2;
            Tag = tag2;
            if (mapPath != "")
            {
                map = LoadLevel(mapPath);
                LoadRoom(mapPath + "Room");
            }

        }

        public void Draw(SpriteBatch spritebatch, Texture2D spritesheet)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                for(int y = 0; y < map.GetLength(0); y++)
                {
                    spritebatch.Draw(spritesheet, new Vector2(x * 16, y * 16), new Rectangle(map[x, y] * 16, 562, 16, 16), Color.White);
                }
            }
        }

        public bool tileIntersection(Rectangle hitBox)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    if(map[x, y] == 4)
                    {
                        if(hitBox.Intersects(new Rectangle(x*16, y*16, 16, 16)))
                        {
                            return true;
                        }
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
