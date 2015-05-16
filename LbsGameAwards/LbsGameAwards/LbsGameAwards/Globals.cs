using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Globals : GameObject
    {
        internal static List<int> completedRooms = new List<int>();

        public static int CurrentEnemyTag { get; set; }

        public static byte currentRoom = 2;

        public static Vector2 transitionScreenPos = new Vector2(0, -480);

        static short transistionCount;
        public static bool transition;
        public static bool spawnPlayer;

        public static void TransitionUpdate()
        {
            if(transition)
            {
                if (transitionScreenPos.Y <= -1)
                {
                    transitionScreenPos += new Vector2(0, 8);
                }
                else
                {
                    transistionCount += 1;
                }
                if (transistionCount >= 16)
                {
                    // TODO: Change room
                    Game1.currentRoom = new Room("Room" + CurrentEnemyTag, currentRoom);
                    transition = false;
                    transistionCount = 0;
                    spawnPlayer = true;
                }
            }
            else
            {
                if (transitionScreenPos.Y >= -480)
                {
                    transitionScreenPos -= new Vector2(0, 15);
                }
            }
        }

        public static void ClearScreen()
        {
            Game1.textEffects.Clear();
            Game1.enemies.Clear();
            Game1.powerUps.Clear();
            Game1.loots.Clear();
        }
    }
}
