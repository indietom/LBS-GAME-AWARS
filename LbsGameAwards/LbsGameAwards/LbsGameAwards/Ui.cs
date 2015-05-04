using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LbsGameAwards
{
    class Ui : GameObject
    {
        const byte MAX_AMOUNT_OF_CASES = 5;

        byte currentAmountOfCases;

        float displayScore;

        bool writePlayerLives;
        public bool hideUi;

        public void Update()
        {
            currentAmountOfCases = (byte)((Game1.players[0].Ammo / 10));
            writePlayerLives = (Game1.players[0].Lives <= 5) ? false : true;

            foreach(Player p in Game1.players)
            {
                Console.WriteLine(p.Score);
                displayScore = Lerp(displayScore, p.Score, 0.1f);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D spritesheet, SpriteFont smallFont, SpriteFont bigFont)
        {
            if(!hideUi)
            {
                spriteBatch.Draw(spritesheet, new Vector2(10, 10), new Rectangle(265, 1, 32, 32), Color.White);
                if(Game1.players[0].GunType > 0) spriteBatch.Draw(spritesheet, new Vector2(10, 10), new Rectangle(265+Frame(Game1.players[0].GunType), 1, 32, 32), Color.White);
                for(int i = 0; i < MAX_AMOUNT_OF_CASES; i++)
                {
                    spriteBatch.Draw(spritesheet, new Vector2(42+i*26, 10), new Rectangle(232, 34, 26, 16), Color.White);
                }
                for(int i = 0; i < currentAmountOfCases; i++)
                {
                    spriteBatch.Draw(spritesheet, new Vector2(45 + i * (20 + 6), 12), new Rectangle(232, 50, 20, 12), Color.White);
                }
                if(writePlayerLives)
                {
                    spriteBatch.Draw(spritesheet, new Vector2(42, 12+14), new Rectangle(265, 34, 15, 13), Color.White);
                    spriteBatch.DrawString(smallFont, "x" + Game1.players[0].Lives.ToString(), new Vector2(42 + 16, 12 + 14), Color.Yellow, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                }
                else
                {
                    for(int i = 0; i < Game1.players[0].Lives; i++)
                    {
                        spriteBatch.Draw(spritesheet, new Vector2(42+i*15, 12+14), new Rectangle(265, 34, 15, 13), Color.White);
                    }
                }
                spriteBatch.DrawString(bigFont, "SCORE: " + Convert.ToInt32(displayScore).ToString(), new Vector2(8, 50), Color.White);
            }
        }
    }
}
