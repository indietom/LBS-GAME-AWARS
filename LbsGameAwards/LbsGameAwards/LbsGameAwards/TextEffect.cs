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
        public TextEffect()
        {

        }

        public Color LerpColor(Color targetColor, float speed)
        {
            return new Color(Lerp(color.R, targetColor.R, Speed), Lerp(color.G, targetColor.G, Speed), Lerp(color.B, targetColor.B, Speed));
        }
    }
}
