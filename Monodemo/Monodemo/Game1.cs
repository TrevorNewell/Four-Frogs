using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;

namespace Monodemo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        Texture2D mainBackground;
        Rectangle rectBackground;

        Enemy enemy;
        List<Enemy> enemies;
        Texture2D enemyTexture;
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;

        private Song gameMusic;

        List<Block> blocks;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1000;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 500;   // set this value to the desired height of your window
            graphics.ApplyChanges();

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            player = new Player();
            blocks = new List<Block>();
            blocks.Add(new Block());
            rectBackground = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            
            enemies = new List<Enemy>();
            enemies.Add(new Enemy());
            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
                                                 GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(Content.Load<Texture2D>("Graphics\\player"), playerPosition);

            //mainBackground = Content.Load<Texture2D>("Graphics\\Glow-Frog Floor(Prototype Placement)");
            mainBackground = Content.Load<Texture2D>("Graphics\\BG");
            blocks[0].Initialize(Content.Load<Texture2D>("graphics\\block0"), new Vector2(200f, 200f));

            gameMusic = Content.Load<Song>("Sounds\\bgm");
            MediaPlayer.Play(gameMusic);

            enemyTexture = Content.Load<Texture2D>("graphics\\Dustbunny01");
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Initialize(enemyTexture, Vector2.Zero);
            }
            // TODO: use this.Content to load your game content here
        }

        private void AddEnemy()
        {
            Enemy e1 = new Enemy();
            Enemy e2 = new Enemy();
            Enemy e3 = new Enemy();
            Enemy e4 = new Enemy();
            Enemy e5 = new Enemy();
            Enemy e6 = new Enemy();
            Enemy e7 = new Enemy();
            Vector2 p1 = new Vector2(830, 25);
            Vector2 p2 = new Vector2(300, 150);
            Vector2 p3 = new Vector2(910, 270);
            Vector2 p4 = new Vector2(180, 350);
            Vector2 p5 = new Vector2(660, 260);
            Vector2 p6 = new Vector2(415, 0);
            Vector2 p7 = new Vector2(350, 0);



            e1.Initialize(enemyTexture, p1);
            enemies.Add(e1);
            e2.Initialize(enemyTexture, p2);
            enemies.Add(e2);
            e3.Initialize(enemyTexture, p3);
            enemies.Add(e3);
            e4.Initialize(enemyTexture, p4);
            enemies.Add(e4);
            e5.Initialize(enemyTexture, p5);
            enemies.Add(e5);
            e6.Initialize(enemyTexture, p6);
            enemies.Add(e6);
            e7.Initialize(enemyTexture, p7);
            enemies.Add(e7);





            //Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2);
            //ran.Next(100, (GraphicsDevice.Viewport.Height - 100));


            //enemy.Initialize(enemyTexture, p3);

            //enemies.Add(enemy);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                AddEnemy();

            }

            for (int i = enemies.Count - 1; i >= 0; i--)
             {

                enemies[i].Update(gameTime);

                if (enemies[i].Active == false)

                {

                    enemies.RemoveAt(i);

                }

            }

        }





        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Save the previous state of the keyboard and game pad so we can determine single key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            //Update the player            
            player.Update();
            UpdatePlayer(gameTime);
            for(int i = 0; i<blocks.Count; i++)
            {
                blocks[i].Update();
                player.DetectCol(blocks[i]);
            }
           
            //Update the enemies
            UpdateEnemies(gameTime);


            base.Update(gameTime);
        }



        private void UpdatePlayer(GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                player.TurnLeft();
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                player.TurnRight();
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                player.GoStraight();
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                player.GoBack();
            }
            //player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            //player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(mainBackground, rectBackground, Color.White);
            //spriteBatch.Draw(mainBackground, new Rectangle(0, 0, 800, 480), Color.White);
            player.Draw(spriteBatch);
            blocks[0].Draw(spriteBatch);

            for (int i = 0; i < enemies.Count; i++)
                { 
                    enemies[i].Draw(spriteBatch);
                }



            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
