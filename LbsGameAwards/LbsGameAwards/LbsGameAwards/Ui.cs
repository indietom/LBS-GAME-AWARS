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

        byte currentAmountOfCases = 5; 

        public bool hideUi;

        public void Update()
        {
            currentAmountOfCases = (byte)((Game1.players[0].Ammo / MAX_AMOUNT_OF_CASES)/MAX_AMOUNT_OF_CASES+1);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D spritesheet, SpriteFont font)
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
            }
        }
    }
}
