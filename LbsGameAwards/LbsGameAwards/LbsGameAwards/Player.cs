using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LbsGameAwards
{
    class Player : GameObject
    {
        float friction;

        public bool inputActive;
        public bool dead;

        byte respawnCount;
        byte maxRespawnCount;
        byte direction;
        byte shootDirection;

        Keys walkLeft = Keys.A;
        Keys walkRight = Keys.D;
        Keys walkDown = Keys.S;
        Keys walkUp = Keys.W;

        Keys shootLeft = Keys.Left;
        Keys shootRight = Keys.Right;
        Keys shootDown = Keys.Down;
        Keys shootUp = Keys.Up;

        KeyboardState keyboard;
        KeyboardState prevKeyboard;

        public Player()
        {
            Pos = new Vector2(320, 240);
            SetSize(32);
            SpriteCoords = new Point(1, 1);
            Z = 0;
            Speed = 0.5f;
            friction = 0.90f;
            inputActive = true;
        }

        public void Movment()
        {
            VelX *= friction;
            VelY *= friction;

            if (VelX >= 0.1f || VelX <= -0.1f)
                Pos += new Vector2(VelX, 0);
            if (VelY >= 0.1f || VelY <= -0.1f)
                Pos += new Vector2(0, VelY);
        }

        public void Input()
        {
            prevKeyboard = keyboard;
            keyboard = Keyboard.GetState();

            if(inputActive)
            {
                if(keyboard.IsKeyDown(walkLeft))
                {
                    VelX -= Speed;
                }
                if(keyboard.IsKeyDown(walkRight))
                {
                    VelX += Speed;
                }
                if (keyboard.IsKeyDown(walkUp))
                {
                    VelY -= Speed;
                }
                if (keyboard.IsKeyDown(walkDown))
                {
                    VelY += Speed;
                }

                if(keyboard.IsKeyDown(shootRight))
                {
                    if(keyboard.IsKeyDown(shootUp))
                    {
                        shootDirection = 1;
                    }
                    else if(keyboard.IsKeyDown(shootDown))
                    {
                        shootDirection = 7;
                    }
                    else
                    {
                        shootDirection = 0;
                    }
                }
                if (keyboard.IsKeyDown(shootLeft))
                {
                    if (keyboard.IsKeyDown(shootUp))
                    {
                        shootDirection = 3;
                    }
                    else if (keyboard.IsKeyDown(shootDown))
                    {
                        shootDirection = 5;
                    }
                    else
                    {
                        shootDirection = 4;
                    }
                }
                if(!keyboard.IsKeyDown(shootLeft) && !keyboard.IsKeyDown(shootRight))
                {
                    if (keyboard.IsKeyDown(shootDown)) shootDirection = 6;
                    if (keyboard.IsKeyDown(shootUp)) shootDirection = 2;
                }
            }
        }

        public void CheckHealth()
        {
            
        }

        public void Update()
        {
            Movment();
            Input();
            SpriteCoords = new Point(SpriteCoords.X, Frame(shootDirection));
            Console.WriteLine(shootDirection);
        }
    }
}
