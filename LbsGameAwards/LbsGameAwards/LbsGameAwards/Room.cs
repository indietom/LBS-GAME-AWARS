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
        byte amountOfMines;

        byte[] doorLeadsTo;

        int[,] map;

        string mapPath;

        bool cleard;
        bool spawnMines;

        public Room(string mapPath2, byte tag2)
        {
            mapPath = mapPath2;
            Tag = tag2;
            map = LoadLevel(mapPath);
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
