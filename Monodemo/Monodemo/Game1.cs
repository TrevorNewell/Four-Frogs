using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
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

        List<Enemy> enemies;
        Texture2D enemyTexture;
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;
        DataTable enemiesTable;
        const int NUM_OF_ENE = 7;
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
        Enemy enemy = new Enemy();
        Random ran = new Random();

        private Song gameMusic;

        List<Block> blocks;
        const int NUM_OF_BLOCKS = 20;
        DataTable blocksTable;
        CSVUtil csv;

        Camera camera;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 720;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 450;   // set this value to the desired height of your window
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
            for(int i = 0; i < NUM_OF_BLOCKS; i++)
            {
                blocks.Add(new Block());
            }    
            blocksTable = new DataTable();
            csv = new CSVUtil();
            blocksTable = csv.ReadCSV("Content\\Data\\blockPoi.csv"); 
            
            blocks.Add(new Block());
            rectBackground = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            Viewport vp = graphics.GraphicsDevice.Viewport;
            enemies = new List<Enemy>();
            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            enemiesTable = csv.ReadCSV("Content\\Data\\enePoi.csv");

            camera = new Camera(GraphicsDevice.Viewport);
            rectBackground = new Rectangle(0, 0, 1440, 900);

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
            Vector2 playerPosition = new Vector2(50, 100);
            player.Initialize(Content.Load<Texture2D>("Graphics\\player"), Content.Load<Texture2D>("Graphics\\hopSheet"), playerPosition);

            mainBackground = Content.Load<Texture2D>("Graphics\\BG");

            for(int i = 0; i < blocks.Count; i++)
            {
                Vector2 poi = new Vector2(float.Parse(blocksTable.Rows[i+1][1].ToString()), float.Parse(blocksTable.Rows[i+1][2].ToString()));
                blocks[i].Initialize(Content.Load<Texture2D>("graphics\\block" + Convert.ToString(i+1)), poi);
            }

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
            /*Enemy e1 = new Enemy();
            Enemy e2 = new Enemy();
            Enemy e3 = new Enemy();
            Enemy e4 = new Enemy();
            Enemy e5 = new Enemy();
            Enemy e6 = new Enemy();
            Enemy e7 = new Enemy();
            Vector2 p1 = new Vector2(float.Parse(enemiesTable.Rows[2][1].ToString()), float.Parse(enemiesTable.Rows[2][2].ToString()));
            Vector2 p2 = new Vector2(float.Parse(enemiesTable.Rows[3][1].ToString()), float.Parse(enemiesTable.Rows[3][2].ToString()));
            Vector2 p3 = new Vector2(float.Parse(enemiesTable.Rows[4][1].ToString()), float.Parse(enemiesTable.Rows[4][2].ToString()));
            Vector2 p4 = new Vector2(float.Parse(enemiesTable.Rows[5][1].ToString()), float.Parse(enemiesTable.Rows[5][2].ToString()));
            Vector2 p5 = new Vector2(float.Parse(enemiesTable.Rows[6][1].ToString()), float.Parse(enemiesTable.Rows[6][2].ToString()));
            Vector2 p6 = new Vector2(float.Parse(enemiesTable.Rows[7][1].ToString()), float.Parse(enemiesTable.Rows[7][2].ToString()));
            Vector2 p7 = new Vector2(float.Parse(enemiesTable.Rows[8][1].ToString()), float.Parse(enemiesTable.Rows[8][2].ToString()));

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
            enemies.Add(e7);*/

            for(int i = 0; i < NUM_OF_ENE; i++)
            {
                Enemy enemy = new Enemy();
                Vector2 poi = new Vector2(float.Parse(enemiesTable.Rows[i + 1][1].ToString()), float.Parse(enemiesTable.Rows[i + 1][2].ToString()));
                enemy.Initialize(enemyTexture, poi);
                enemies.Add(enemy);
            }

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
            player.Update(gameTime);
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Update();
                player.DetectCol(blocks[i]);
                enemies[i].DetectCol(blocks[i]);
            }
            
            for(int i = 0; i <enemies.Count; i++)
            {
                enemies[i].DetectColPlayer(player);
            }
            UpdatePlayer(gameTime);            
           
            //Update the enemies

            for(int i = enemies.Count-1; i>=0; i--)
            { 
                enemy = enemies[i];
                enemy.Update(gameTime);
            }
            UpdateEnemies(gameTime);

            //zoom
            camera.Update(player.Position);
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                camera.zoom += 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                camera.zoom -= 0.1f;
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
            if (currentKeyboardState.IsKeyDown(Keys.Up) && (!player.isCol))
            {
                player.GoStraight();
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                player.GoBack();
            }
            
            //detect the collision with border
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, 1440 - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, 900 - player.Height);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null,null,null,null,camera.transform);
            spriteBatch.Draw(mainBackground, rectBackground, Color.White);
            player.Draw(spriteBatch);
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Draw(spriteBatch);
            }

            
            for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Draw(spriteBatch);
                }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}