using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Loot : GameObject
    {
        public short Worth { private set; get; }

        byte type;

        public bool pickedUp;

        public Loot(Vector2 pos2, byte type2)
        {
            Pos = pos2;
            type = type2;
            SetSize(16);
            SpriteCoords = new Point(330 + Frame(type, 16), 298);
            Worth = (short)((type+1) * 100);
        }

        public void Update()
        {
            Random random = new Random();

            if (pickedUp) Z = ZOrder();
            else Z = 1;
            
            if(pickedUp)
            {
                Game1.particles.Add(new Particle(Pos, 0, 1, random.Next(360), 0));
                Pos = new Vector2(Lerp(Pos.X, -64, 0.05f), Lerp(Pos.Y, -64, 0.05f));
                if (Pos.Y <= -16) destroy = true;
            }
        }
    }
}
