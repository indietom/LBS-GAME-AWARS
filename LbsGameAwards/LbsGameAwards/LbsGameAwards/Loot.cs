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
            Z = ZOrder();
        }
    }
}
