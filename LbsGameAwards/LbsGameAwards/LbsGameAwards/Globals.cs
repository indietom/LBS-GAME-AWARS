using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    enum GameStates { startScreen, game, gameOver, end };

    class Globals : GameObject
    {
        internal static List<int> completedRooms = new List<int>();

        internal static GameStates gameState;

        public static string reason;

        public static int CurrentEnemyTag { get; set; }
        public static int CurrentHightScore { get; private set; }

        public static void UpdateHighScore()
        {
            if (Game1.players[0].Score > CurrentHightScore) CurrentHightScore = Game1.players[0].Score;
        }

        public static byte currentRoom = 1;

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
                    Console.WriteLine(Globals.currentRoom);
                    Game1.currentRoom = new Room(@"Content\levels\Room" + currentRoom, currentRoom);
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
            Game1.projectiles.Clear();
            Game1.powerUps.Clear();
            Game1.loots.Clear();
            Game1.explosions.Clear();
        }
    }
}
