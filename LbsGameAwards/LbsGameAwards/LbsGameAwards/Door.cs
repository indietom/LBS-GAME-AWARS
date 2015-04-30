using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LbsGameAwards
{
    class Door : GameObject
    {
        byte openingSpeed = 1;

        bool vertical;
        bool open = true;

        public Door(Vector2 pos2, bool vertical2)
        {
            Pos = pos2;
            vertical = vertical2;

            SetSize(32);

            MaxAnimationCount = 8;
            MinFrame = 7;
            MaxFrame = (short)(MinFrame + 10);
            CurrentFrame = MinFrame;

            if (!vertical)
                SpriteCoords = new Point(232, 529);
            else
                SpriteCoords = new Point(232, 562);

            if(Pos.X > 320 && !vertical)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            if (Pos.Y > 240 && vertical)
            {
                spriteEffects = SpriteEffects.FlipVertically;
            }
        }

        public void Update()
        {
            Animate();
            AnimationCount += 1;
            SpriteCoords = new Point(Frame(CurrentFrame), SpriteCoords.Y);

            if(open)
            {
                if(vertical && Size.Y > 0)
                {
                    if(spriteEffects == SpriteEffects.None)
                        SetSize(Size.X, Size.Y - openingSpeed);
                    else
                    {
                        SetSize(Size.X, Size.Y - openingSpeed);
                        Pos += new Vector2(0, openingSpeed);
                    }
                }
                if (!vertical && Size.X > 0)
                {
                    if (spriteEffects == SpriteEffects.None)
                        SetSize(Size.X - openingSpeed, Size.Y);
                    else
                    {
                        SetSize(Size.X - openingSpeed, Size.Y);
                        Pos += new Vector2(openingSpeed, 0);
                    }
                }
            }
            else
            {
                if (vertical && Size.Y < 32)
                {
                    if (spriteEffects == SpriteEffects.None)
                        SetSize(Size.X, Size.Y + openingSpeed);
                    else
                    {
                        SetSize(Size.X, Size.Y + openingSpeed);
                        Pos += new Vector2(0, -openingSpeed);
                    }
                }
                
                if (!vertical && Size.X < 32)
                {
                    if (spriteEffects == SpriteEffects.None)
                        SetSize(Size.X + openingSpeed, Size.Y);
                    else
                    {
                        SetSize(Size.X + openingSpeed, Size.Y);
                        Pos += new Vector2(-openingSpeed, 0);
                    }
                }
            }
        }
    }
}
