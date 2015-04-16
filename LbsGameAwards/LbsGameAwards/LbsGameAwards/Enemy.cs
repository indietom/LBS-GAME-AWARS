using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Enemy : GameObject
    {
        sbyte hp;

        byte type;
        byte direction;

        bool attacking;
        bool invisible;

        short attackCount;
        short maxAttackCount;
        short hurtCount;
        short maxHurtCount;

        public Enemy(Vector2 pos2, byte type2)
        {
            Pos = pos2;
            type = type2;
            AssignType();
            OrginalColor = color;
            Z = 1.0f;
            maxHurtCount = 8;
        }

        public void Update()
        {
            HurtUpdate();
        }

        public void HurtUpdate()
        {
            if(hurtCount >= 1)
            {
                color = Color.Red;
                invisible = true;
            }
            if (hurtCount >= maxHurtCount)
            {
                color = OrginalColor;
                invisible = false;
            }
        }

        public void AssignType()
        {
            switch(type)
            {
                case 0:
                    maxAttackCount = 16;
                    hp = 1;
                    MaxAnimationCount = 4;
                    MaxFrame = 4;
                    SpriteCoords = new Point(1, 331);
                    SetSize(32);
                    break;
            }
        }
    }
}
