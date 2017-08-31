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
        enum AiState
        {
            Chasing,

            Wander
        }
        Vector2 enemyPosition;
        float orientation = 0;
        Vector2 enemyTextureCenter;
        float enemyChaseDistance = 250.0f;
        float enemyHysteresis = 15.0f;
        float maxEnemySpeed = 5.0f;
        float enemySpeed = 2.0f;
        Vector2 wanderDirection;
        Random ran = new Random();

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

            Viewport vp = graphics.GraphicsDevice.Viewport;
            enemies = new List<Enemy>();
            enemies.Add(new Enemy());
            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            enemyPosition = new Vector2(vp.Width / 4, vp.Height / 2);
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
            enemyTextureCenter = new Vector2(enemyTexture.Width / 2, enemyTexture.Height / 2);
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


            //if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            //{
               // previousSpawnTime = gameTime.TotalGameTime;

               AddEnemy();

            //}

            for (int i = enemies.Count - 1; i >= 0; i--)
            {

                enemies[i].Update(gameTime);

                if (enemies[i].Active == false)

                {

                    enemies.RemoveAt(i);

                }

            }

            
            
            AiState enemyState = AiState.Wander;
            float enemyChaseThreshold = enemyChaseDistance;
            Enemy e = new Enemy();
            
    
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                e = enemies[i];
                enemyPosition = e.Position;                
                

                if (enemyState == AiState.Wander)
                {
                    enemyChaseThreshold -= enemyHysteresis / 2;
                }
                else if(enemyState == AiState.Chasing)
                {
                    enemyChaseThreshold += enemyHysteresis / 2;
                }

                if (enemyState == AiState.Wander)
                {

                    Wander(enemyPosition, ref wanderDirection, ref orientation,
                        e.enemyTurnSpeed);
                    enemySpeed = .25f * maxEnemySpeed;

                }
                else if(enemyState == AiState.Chasing)
                {
                    orientation = TurnToFace(enemyPosition, player.Position, orientation, e.enemyTurnSpeed);
                }
                Vector2 heading = new Vector2(
                    (float)Math.Cos(orientation), (float)Math.Sin(orientation));
                enemyPosition += heading * enemySpeed;

            }
        }

            private void Wander(Vector2 position, ref Vector2 wanderDirection, ref float orientation, float turnSpeed)
            {
                wanderDirection.X += MathHelper.Lerp(-.25f, .25f, (float)ran.NextDouble());
                wanderDirection.Y += MathHelper.Lerp(-.25f, .25f, (float)ran.NextDouble());

                if (wanderDirection != Vector2.Zero)
                {
                 wanderDirection.Normalize();
                }

                orientation = TurnToFace(position, position + wanderDirection, orientation, .15f * turnSpeed);
                Vector2 screenCenter = Vector2.Zero;
                screenCenter.X = graphics.GraphicsDevice.Viewport.Width / 2;
                screenCenter.Y = graphics.GraphicsDevice.Viewport.Width / 2;

                float distanceFromScreenCenter = Vector2.Distance(screenCenter, position);
                float MaxDistanceFromScreenCenter = Math.Min(screenCenter.Y, screenCenter.X);
                float normalizedDistance = distanceFromScreenCenter / MaxDistanceFromScreenCenter;
                float turnToCenterSpeed = .3f * normalizedDistance * normalizedDistance * turnSpeed;

                orientation = TurnToFace(position, screenCenter, orientation, turnToCenterSpeed);
            }


            private static float TurnToFace(Vector2 position, Vector2 faceThis, float currentAngle, float turnSpeed)
            {

                float x = faceThis.X - position.X;
                float y = faceThis.Y - position.Y;


                float desiredAngle = (float)Math.Atan2(y, x);


                float difference = WrapAngle(desiredAngle - currentAngle);


                difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

                return WrapAngle(currentAngle + difference);
            }

            private static float WrapAngle(float radians)
            {
                while (radians < -MathHelper.Pi)
                {
                    radians += MathHelper.TwoPi;
                }
                while (radians > MathHelper.Pi)
                {
                    radians -= MathHelper.TwoPi;
                }
                return radians;
            }

            private Vector2 ClampToViewport(Vector2 vector)
            {
                Viewport vp = graphics.GraphicsDevice.Viewport;
                vector.X = MathHelper.Clamp(vector.X, vp.X, vp.X + vp.Width);
                vector.Y = MathHelper.Clamp(vector.Y, vp.Y, vp.Y + vp.Height);
                return vector;
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
                enemies[i].DetectCol(blocks[i]);
            }
            
            for(int i = 0; i <enemies.Count; i++)
            {
                enemies[i].DetectColPlayer(player);
            }
           
            //Update the enemies

            for(int i = enemies.Count-1; i>=0; i--)
            { 
                enemy = enemies[i];
                enemy.Update(gameTime);
            }
            UpdateEnemies(gameTime);
            enemyPosition = ClampToViewport(enemyPosition);

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
