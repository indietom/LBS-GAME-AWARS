using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace RoomCreator
{
    class Program
    {
        static void ColoredText(string text, ConsoleColor consoleColor, bool jumpLine)
        {
            Console.ForegroundColor = consoleColor;
            if (jumpLine)
                Console.WriteLine(text);
            else
                Console.Write(text);
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            string fileName = "";

            int amountOfTypes = 0;

            byte type = 0;
            byte enemySpawnDelay;

            short amountOfEnemies = 0;

            bool spawnWithMines;

            byte[] doorLeadsTo = new byte[4];

            while(true)
            {
                ColoredText("FILE NAME: ", ConsoleColor.Green, false);
                fileName = Console.ReadLine();

                File.Create(fileName).Dispose();

                ColoredText("HOW MANY TYPES: ", ConsoleColor.Yellow, false);
                amountOfTypes = int.Parse(Console.ReadLine());

                StreamWriter sw = new StreamWriter(fileName);
                sw.WriteLine(amountOfTypes);
                for(int i = 0; i < amountOfTypes; i++)
                {
                    ColoredText("ENEMY TYPE: ", ConsoleColor.Magenta, false);
                    type = byte.Parse(Console.ReadLine());
                    ColoredText("AMOUNT OF ENEMY TYPE: ", ConsoleColor.Green, false);
                    amountOfEnemies = short.Parse(Console.ReadLine());
                    ColoredText("DELAY BETWEEN ENEMIES: ", ConsoleColor.Yellow, false);
                    enemySpawnDelay = byte.Parse(Console.ReadLine());

                    sw.Write(type + "-" + amountOfEnemies + "-" +  enemySpawnDelay + Environment.NewLine);
                }

                Console.WriteLine(" \n");

                for (int i = 0; i < doorLeadsTo.Count(); i++)
                {
                    ColoredText("DOOR "+ (i+1)  +": ", ConsoleColor.Yellow, false);
                    doorLeadsTo[i] = byte.Parse(Console.ReadLine());
                }

                sw.WriteLine(doorLeadsTo[0] + "-" + doorLeadsTo[1] + "-" +doorLeadsTo[2] + "-" + doorLeadsTo[3]);

                Console.WriteLine("\n");

                ColoredText("SPAWN ROOM WITH MINES: ", ConsoleColor.Cyan, false);
                spawnWithMines = Convert.ToBoolean(Console.ReadLine().ToLower());

                sw.WriteLine(spawnWithMines);

                sw.Dispose();
                Console.WriteLine("JOBS DONE");
            }
        }
    }
}
