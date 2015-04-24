using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Explosion : GameObject 
    {
        short animationOffset;

        byte size;

        public bool dangerous;
        public bool dangerousToPlayer;

        public Explosion(Vector2 pos2, byte size2, Color color2)
        {
            Z = 0.9999f;
            Pos = pos2;
            size = size2;
            SetSize(size);
            color = color2;
            MaxAnimationCount = 4;
            AssignSprite();
            CurrentFrame = MinFrame;
        }

        public Explosion(Vector2 pos2, byte size2, Color color2, bool dangerous2, bool dangerousToPlayer2)
        {
            Z = 0.9999f;
            Pos = pos2;
            size = size2;
            SetSize(size);
            color = color2;
            MaxAnimationCount = 4;
            AssignSprite();
            CurrentFrame = MinFrame;
            dangerous = dangerous2;
            dangerousToPlayer = dangerousToPlayer2;
        }

        public void Update()
        {
            Animate();
            AnimationCount += 1;
            destroy = (CurrentFrame >= MaxFrame - 1) ? true : destroy;
            dangerous = (CurrentFrame < MaxFrame / 2) ? dangerous : false;
            SpriteCoords = new Point(Frame(CurrentFrame, Size.X)+animationOffset, SpriteCoords.Y);
            
            if(dangerous && dangerousToPlayer)
            {
                foreach(Player p in Game1.players)
                {
                    if(p.HitBox().Intersects(HitBox()))
                    {
                        p.dead = true;
                    }
                }
            }
            if(dangerous && !dangerousToPlayer)
            {
                foreach(Enemy e in Game1.enemies)
                {
                    if (e.HitBox().Intersects(HitBox()))
                    {
                        e.Hp = 0;
                    }
                }
            }
        }

        public void AssignSprite()
        {
            switch(size)
            {
                case 32:
                    MinFrame = 7;
                    MaxFrame = 14;
                    SpriteCoords = new Point(Frame(MinFrame), 166);
                    break;
                case 64:
                    MaxFrame = 6;
                    SpriteCoords = new Point(166, 100);
                    SetSize(64);
                    animationOffset = (short)(SpriteCoords.X - 1);
                    break;
            }
        }
    }
}
