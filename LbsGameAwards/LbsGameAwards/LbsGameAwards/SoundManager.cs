using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace LbsGameAwards
{
    class SoundManager
    {
        public static SoundEffect shoot, dead;

        public static void Load(ContentManager content)
        {
            shoot = content.Load<SoundEffect>("muffeldShoot");
            dead = content.Load<SoundEffect>("dead");
        }
    }
}
