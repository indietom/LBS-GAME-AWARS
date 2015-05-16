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
        static internal List<PowerUp> powerUps = new List<PowerUp>();
        static internal List<Door> doors = new List<Door>();
        static internal List<Loot> loots = new List<Loot>();
        static internal List<TextEffect> textEffects = new List<TextEffect>();
        static internal List<Particle> particles = new List<Particle>();
        static internal List<Gib> gibs = new List<Gib>();
        static internal List<Helper> helpers = new List<Helper>();
        static internal List<Boss> bosses = new List<Boss>();

        static internal Ui ui = new Ui();
        static internal SpawnManager spawnManager = new SpawnManager();

        static internal Room currentRoom = new Room("test", 1);

        protected override void Initialize()
        {
            players.Add(new Player());
            helpers.Add(new Helper(new Vector2(320, 240)));
            //bosses.Add(new Boss(new Vector2(500, 240)));
            Globals.completedRooms.Add(3);
            base.Initialize();
        }

        Texture2D spritesheet;
        Texture2D transitionScreen;
        Texture2D bossSheet;

        SpriteFont font;
        SpriteFont smallFont;
        SpriteFont bigFont;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spritesheet = Content.Load<Texture2D>("spritesheet");
            transitionScreen = Content.Load<Texture2D>("transitionScreen");
            bossSheet = Content.Load<Texture2D>("bossSheet");
            font = Content.Load<SpriteFont>("font");
            bigFont = Content.Load<SpriteFont>("BigFont");
            smallFont = Content.Load<SpriteFont>("SmallFont");

            SoundManager.Load(Content);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            Random random = new Random();

            for (int i = 0; i < Globals.completedRooms.Count; i++)
            {
                Console.WriteLine(Globals.completedRooms[i]);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            spawnManager.UpdatePowerUpSpawn();
            spawnManager.UpdateLootSpawn();

            Globals.TransitionUpdate();

            foreach (Player p in players)
                p.Update();

            foreach (Projectile p in projectiles)
                p.Update();

            foreach (Explosion e in explosions)
                e.Update();

            foreach (Enemy e in enemies)
                e.Update();

            foreach (PowerUp p in powerUps)
                p.Update();

            foreach (Door d in doors)
                d.Update();

            foreach (Loot l in loots)
                l.Update();

            foreach (TextEffect t in textEffects)
                t.Update();

            foreach (Particle p in particles)
                p.Update();

            foreach (Gib g in gibs)
                g.Update();

            foreach (Helper h in helpers)
                h.Update();
            foreach (Boss b in bosses)
                b.Update();

            currentRoom.Update();

            ui.Update();

            if(Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                //if(enemies.Count == 0) enemies.Add(new Enemy(new Vector2(320, 240), 4));
                if (powerUps.Count == 0) powerUps.Add(new PowerUp(new Vector2(320, 240), 2, false));
                //if (doors.Count == 0) doors.Add(new Door(new Vector2(320 + 128, 240+3), true));
                enemies.Clear();
            }
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if(loots.Count <= 1) spawnManager.spawnLootPile(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), 64, -1);
                //gibs.Add(new Gib(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), random.Next(360), random.Next(16)/5, random.Next(20, 30)/12, (byte)random.Next(5), true));
                //Globals.transition = true;
            }

            //Console.WriteLine(loots.Count());

            for (int i = projectiles.Count() - 1; i >= 0; i--)
                if (projectiles[i].destroy) projectiles.RemoveAt(i);

            for (int i = helpers.Count() - 1; i >= 0; i--)
                if (helpers[i].destroy) helpers.RemoveAt(i);

            for (int i = explosions.Count - 1; i >= 0; i--)
                if (explosions[i].destroy) explosions.RemoveAt(i);

            for (int i = enemies.Count - 1; i >= 0; i--)
                if (enemies[i].destroy) enemies.RemoveAt(i);

            for (int i = powerUps.Count - 1; i >= 0; i--)
                if (powerUps[i].destroy) powerUps.RemoveAt(i);

            for (int i = doors.Count - 1; i >= 0; i--)
                if (doors[i].destroy) doors.RemoveAt(i);

            for (int i = loots.Count - 1; i >= 0; i--)
                if (loots[i].destroy) loots.RemoveAt(i);

            for (int i = textEffects.Count - 1; i >= 0; i--)
                if (textEffects[i].destroy) textEffects.RemoveAt(i);

            for (int i = particles.Count - 1; i >= 0; i--)
                if (particles[i].destroy) particles.RemoveAt(i);

            for (int i = gibs.Count - 1; i >= 0; i--)
                if (gibs[i].destroy) gibs.RemoveAt(i);
            for (int i = bosses.Count - 1; i >= 0; i--)
                if (bosses[i].destroy) bosses.RemoveAt(i);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null);

            currentRoom.Draw(spriteBatch, spritesheet, font);

            foreach (Helper h in helpers)
                h.DrawSprite(spriteBatch, spritesheet);
            foreach (Boss b in bosses)
                b.DrawSprite(spriteBatch, bossSheet);
            foreach (Player p in players)
            {
                p.DrawSprite(spriteBatch, spritesheet);
                p.DrawShield(spriteBatch, spritesheet);
            }
            foreach (Projectile p in projectiles)
                p.DrawSprite(spriteBatch, spritesheet);
            foreach (Explosion e in explosions)
                e.DrawSprite(spriteBatch, spritesheet);
            foreach (PowerUp p in powerUps)
                p.Draw(spriteBatch, spritesheet);
            foreach (Door d in doors)
                d.DrawSprite(spriteBatch, spritesheet);
            foreach (Loot l in loots)
                l.DrawSprite(spriteBatch, spritesheet);
            foreach (Particle p in particles)
                p.DrawSprite(spriteBatch, spritesheet);
            foreach (Gib g in gibs)
                g.DrawSprite(spriteBatch, spritesheet);

            foreach (Enemy e in enemies)
            {
                e.DrawSprite(spriteBatch, spritesheet);
                e.Draw(spriteBatch, spritesheet);
            }

            //currentRoom.Draw(spriteBatch, spritesheet);

            spriteBatch.End();

            spriteBatch.Begin();
            foreach(TextEffect t in textEffects) t.Draw(spriteBatch, bigFont, smallFont);
            ui.Draw(spriteBatch, spritesheet, smallFont, bigFont);
            spriteBatch.Draw(transitionScreen, Globals.transitionScreenPos, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
