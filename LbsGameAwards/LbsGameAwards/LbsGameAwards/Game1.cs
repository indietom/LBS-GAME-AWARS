using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LbsGameAwards
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 640;
            Content.RootDirectory = "Content";
        }

        static internal List<Player> players = new List<Player>();
        static internal List<Projectile> projectiles = new List<Projectile>();
        static internal List<Explosion> explosions = new List<Explosion>();
        static internal List<Enemy> enemies = new List<Enemy>(); 

        protected override void Initialize()
        {
            players.Add(new Player());
            base.Initialize();
        }

        Texture2D spritesheet;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spritesheet = Content.Load<Texture2D>("spritesheet");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            
            foreach (Player p in players)
                p.Update();

            foreach (Projectile p in projectiles)
                p.Update();

            foreach (Explosion e in explosions)
                e.Update();

            foreach (Enemy e in enemies)
                e.Update();

            if(Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                if(enemies.Count == 0) enemies.Add(new Enemy(new Vector2(320, 240), 2));
            }
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if(explosions.Count == 0) explosions.Add(new Explosion(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 32, Color.Red, true, true));
            }

            for (int i = projectiles.Count() - 1; i >= 0; i--)
                if (projectiles[i].destroy) projectiles.RemoveAt(i);

            for (int i = explosions.Count - 1; i >= 0; i--)
                if (explosions[i].destroy) explosions.RemoveAt(i);

            for (int i = enemies.Count - 1; i >= 0; i--)
                if (enemies[i].destroy) enemies.RemoveAt(i);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null);
            foreach (Player p in players)
                p.DrawSprite(spriteBatch, spritesheet);
            foreach (Projectile p in projectiles)
                p.DrawSprite(spriteBatch, spritesheet);
            foreach (Explosion e in explosions)
                e.DrawSprite(spriteBatch, spritesheet);

            foreach (Enemy e in enemies)
            {
                e.DrawSprite(spriteBatch, spritesheet);
                e.Draw(spriteBatch, spritesheet);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
