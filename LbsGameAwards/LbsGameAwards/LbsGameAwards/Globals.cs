using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LbsGameAwards
{
    class Globals
    {
        public static int CurrentEnemyTag { get; set; }

        public static void ClearScreen()
        {
            Game1.textEffects.Clear();
            Game1.enemies.Clear();
            Game1.powerUps.Clear();
            Game1.loots.Clear();
        }
    }
}
