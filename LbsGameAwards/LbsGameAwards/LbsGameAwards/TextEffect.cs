using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LbsGameAwards
{
    class TextEffect : GameObject
    {
        short intervalCount;
        short lifeTime;
        short maxLifeTime;

        byte currentColor;
        byte movmentType;
        byte tag;

        bool useBigFont;

        string text;

        Vector2 target;

        public TextEffect(Vector2 pos2, string text2, float angle2, float speed2, byte tag2, byte movmentType2, Color color2, short maxLifeTime2)
        {
            color = color2;
            Pos = pos2;
            Angle = angle2;
            Speed = speed2;
            tag = tag2;
            movmentType = movmentType2;
            text = text2;
            maxLifeTime = maxLifeTime2;
        }

        public TextEffect(Vector2 pos2, string text2, Vector2 target2, float speed2, byte tag2, byte movmentType2, Color color2, short maxLifeTime2)
        {
            color = color2;
            Pos = pos2;
            target = target2;
            Speed = speed2;
            tag = tag2;
            movmentType = movmentType2;
            text = text2;
            maxLifeTime = maxLifeTime2;
        }

        public void Update()
        {
            color = FlashColors(new Color[4]{Color.White, Color.Red, Color.Blue, Color.Green}, 8, true);

            if(maxLifeTime != 0) lifeTime += 1;
            
            if(lifeTime >= maxLifeTime)
            {
                destroy = true;
            }

            switch(movmentType)
            {
                case 0:
                    AngleMath();
                    Pos += Vel;
                    break;
                case 1:
                    Pos = new Vector2(Lerp(Pos.X, target.X, Speed), Lerp(Pos.Y, target.Y, Speed));
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont bigFont, SpriteFont smallFont)
        {
            if(useBigFont)
            {
                spriteBatch.DrawString(bigFont, text, Pos, color);
            }
            else
            {
                spriteBatch.DrawString(smallFont, text, Pos, color);
            }
        }

        // untestetd code is best code
        public Color FlashColors(Color[] colors, short interval, bool randomColor)
        {
            Random random = new Random();
            intervalCount += 1;
            if(intervalCount >= interval)
            {
                if(randomColor)
                {
                    currentColor = (byte)random.Next(colors.Count());
                }
                else
                {
                    if (currentColor > colors.Count())
                    {
                        currentColor = 0;
                    }
                    else
                    {
                        currentColor += 1;
                    }
                }
                intervalCount = 0;
            }
            return colors[currentColor];
        }

        public Color LerpColor(Color targetColor, float speed)
        {
            return new Color(Lerp(color.R, targetColor.R, Speed), Lerp(color.G, targetColor.G, Speed), Lerp(color.B, targetColor.B, Speed));
        }
    }
}
