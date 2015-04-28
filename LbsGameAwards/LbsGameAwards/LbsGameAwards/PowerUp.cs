using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LbsGameAwards
{
    class PowerUp : GameObject
    {
        public bool special;

        public byte Type { private set; get; }

        float cosCount;

        Point itemSprite;

        public PowerUp(Vector2 pos2, byte type2, bool special2)
        {
            Pos = pos2;
            Type = type2;
            special = special2;
            AssignSprite();
        }

        public void Update()
        {
            SpriteCoords = new Point(Frame(CurrentFrame, Size.X) + 231, SpriteCoords.Y);
            Animate();
            AnimationCount += 1;

            cosCount += 0.01f;

            Pos += new Vector2(0, (float)Math.Sin(20 * cosCount + 30));

            Z = ZOrder();
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D spritesheet)
        {
            DrawSprite(spriteBatch, spritesheet);;
            spriteBatch.Draw(spritesheet, new Vector2(Pos.X + 4, Pos.Y + 3), new Rectangle(itemSprite.X, itemSprite.Y, 18, 10), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, Z + 0.01f);
        }

        public void AssignSprite()
        {
            MaxAnimationCount = 8;
            MaxFrame = 11;

            SetSize(24, 16);

            SpriteCoords = new Point(232, 199);

            if (!special)
                itemSprite = new Point(331 + Frame(Type, 18), 34);
            else
                itemSprite = new Point(331 + Frame(Type, 18), 45);
        }
    }
}
