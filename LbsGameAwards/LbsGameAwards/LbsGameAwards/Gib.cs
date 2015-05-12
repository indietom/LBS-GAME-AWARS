﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LbsGameAwards
{
    class Gib : GameObject
    {
        bool falling;

        float peak;

        public Gib(Vector2 pos2, float ang2, float speed2, float peak2, byte frame, bool metal)
        {
            Pos = pos2;
            Angle = ang2;
            Speed = speed2;
            peak = peak2;
            Scale = 0;

            rotated = true;

            SetSize(16);

            if(metal)
            {
                SpriteCoords = new Point(Frame(frame, 16) + 332, 232);
            }
            else
            {
                SpriteCoords = new Point(Frame(frame, 16) + 233, 232);
            }
        }

        public void Update()
        {
            Z = Scale / 10;

            AngleMath();
            Pos += Vel;

            Rotation += Speed*2;
            
            if(!falling)
            {
                Scale = Lerp(Scale, peak, 0.2f);
                if (Scale >= peak-0.05f) falling = true;
            }
            else
            {
                Scale = Lerp(Scale, 0, 0.1f);
            }
        }
    }
}
