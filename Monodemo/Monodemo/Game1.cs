using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
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

        List<Block> blocks;
        const int NUM_OF_BLOCKS = 20;
        DataTable blocksTable;
        CSVUtil csv;
        Block ball;
        Block wall;

        List<GUI> GUIs;
        GUI currentGUI;
        const int NUM_OF_GUIS = 2;
        int index = 0;
        bool isKeyPressed = false;

        GUI healthBar;
        Rectangle healthBarRec;
        GUI healthBarBorders;

        GUI failScreen;
        GUI winScreen;

        Camera camera;

        public Song gameMusic;
        public SoundEffect meetEnemy;
        public SoundEffectInstance meetEnemyInstance;
        public SoundEffect goldenBall;
        public SoundEffectInstance goldenBallInstance;
        public SoundEffect collisionSound;
        public SoundEffectInstance collisionSoundInstance;

        private bool gameStarted = false;

        // Lighting variables
        Texture2D lightMask;
        Texture2D ballGlow;
        float mouseX;
        float mouseY;

        RenderTarget2D lightsTarget;
        RenderTarget2D mainTarget;

        Effect lightingEffect;

        float scale; // Current scale of our glow
        float maxScale; // Max size of our glow
        float minScale; // Minimum size of our glow
        float rate; // How fast we lose our glow


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferWidth = 1440;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 900;   // set this value to the desired height of your window
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
            maxScale = 1.5f;
            minScale = 0.6f;
            rate = 0.0001f;

            scale = maxScale;
            player = new Player();

            blocks = new List<Block>();
            for(int i = 0; i < NUM_OF_BLOCKS; i++)
            {
                blocks.Add(new Block());
            }    
            blocksTable = new DataTable();
            csv = new CSVUtil();
            blocksTable = csv.ReadCSV("Content\\Data\\blockPoi.csv");
            ball = new Block();
            wall = new Monodemo.Block();
            
            enemies = new List<Enemy>();
            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            enemiesTable = csv.ReadCSV("Content\\Data\\enePoi.csv");

            GUIs = new List<GUI>();
            for(int i = 0; i<NUM_OF_GUIS; i++)
            {
                GUIs.Add(new GUI(graphics));
            }
            currentGUI = new GUI(graphics);
            currentGUI = GUIs[index];

            healthBar = new GUI(graphics);
            healthBar.setOffset(10, -130);
            healthBarBorders = new GUI(graphics); 
            healthBarBorders.setOffset(10, -100);
            failScreen = new GUI(graphics);
            winScreen = new Monodemo.GUI(graphics);

            camera = new Camera(GraphicsDevice.Viewport);
            rectBackground = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

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

            Vector2 playerPosition = new Vector2(100, 100);
            player.Initialize(Content.Load<Texture2D>("Graphics\\player"), Content.Load<Texture2D>("Graphics\\hopSheet"), playerPosition);

            mainBackground = Content.Load<Texture2D>("Graphics\\bg");

            lightMask = Content.Load<Texture2D>("Graphics\\sampleLightMask");
            ballGlow = Content.Load<Texture2D>("Graphics\\sampleLightMask");
            lightingEffect = Content.Load<Effect>("lighteffect");

            var pp = GraphicsDevice.PresentationParameters;
            lightsTarget = new RenderTarget2D(
                GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            mainTarget = new RenderTarget2D(
                GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);

           /* for (int i = 0; i < blocks.Count; i++)
            {
                Vector2 poi = new Vector2(float.Parse(blocksTable.Rows[i+1][1].ToString()), float.Parse(blocksTable.Rows[i+1][2].ToString()));
                blocks[i].Initialize(Content.Load<Texture2D>("graphics\\block" + Convert.ToString(i+1)), poi);
            }*/
            ball.Initialize(Content.Load<Texture2D>("graphics\\goldenball"), new Vector2(750f, 400f));
            wall.Initialize(Content.Load<Texture2D>("graphics\\wall"), new Vector2(720f, 450f));
            

            gameMusic = Content.Load<Song>("Sounds\\bgm");
            MediaPlayer.Play(gameMusic);
            meetEnemy = Content.Load<SoundEffect>("Sounds\\meetEnemy");
            meetEnemyInstance = meetEnemy.CreateInstance();
            goldenBall = Content.Load<SoundEffect>("Sounds\\goldenBall");
            goldenBallInstance = goldenBall.CreateInstance();
            collisionSound = Content.Load<SoundEffect>("Sounds\\collisionSound");
            collisionSoundInstance = collisionSound.CreateInstance();

            enemyTexture = Content.Load<Texture2D>("graphics\\Dustbunny01");
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Initialize(enemyTexture, Vector2.Zero);
            }

            for (int i = 0; i < NUM_OF_GUIS; i++)
            {
                GUIs[i].LoadContent(Content.Load<Texture2D>("graphics\\GUI"+i.ToString()));                
            }

            healthBar.LoadContent(Content.Load<Texture2D>("graphics\\healthBar"));
            healthBar.GUIRectangle = new Rectangle(0, 0, 200, 20);
            healthBarBorders.LoadContent(Content.Load<Texture2D>("graphics\\healthBarBorder"));
            failScreen.LoadContent(Content.Load<Texture2D>("graphics\\FAILscreen"));
            winScreen.LoadContent(Content.Load<Texture2D>("graphics\\victory"));
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
            mouseX = Mouse.GetState().Position.X;
            mouseY = Mouse.GetState().Position.Y;

            if (scale > minScale) scale -= rate;

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
            /* for (int i = 0; i < blocks.Count; i++)
             {
                 blocks[i].Update();
                 player.DetectCol(blocks[i]);
             }*/
            player.DetectCol(ball);
            player.DetectCol(wall);
            UpdatePlayer(gameTime);            
           
            //Update the enemies
            UpdateEnemies(gameTime);

            //zoom
            camera.Update(player.Position);
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                camera.zoom += 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                camera.zoom -= 0.1f;

            updateGUI(gameTime);

            base.Update(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                player.TurnLeft(gameTime);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                player.TurnRight(gameTime);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) && (!player.isCol))
            {
                player.GoStraight(gameTime);
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) && (!player.isCol))
            {
                player.GoBack(gameTime);
            }

            //controller
            if(Math.Abs(currentGamePadState.ThumbSticks.Left.X)>0.2f || Math.Abs(currentGamePadState.ThumbSticks.Left.Y) > 0.2f)
            {
                double X = currentGamePadState.ThumbSticks.Left.X;
                double Y = currentGamePadState.ThumbSticks.Left.Y;
                double R = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));                
                player.rotation = (float)Math.Acos(Y/R);
                if(X < 0)
                {
                    player.rotation = -player.rotation;
                }

                if (!player.isCol)
                {
                    player.GoStraight(gameTime);
                }                
            }
            
            //detect the collision with border
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, 1440 - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, 900 - player.Height);
        }

        private void updateGUI(GameTime gameTime)
        {            
            if ((currentGamePadState.IsButtonDown(Buttons.A) && (!isKeyPressed)))
            {
                isKeyPressed = true;
                if (index < 1)
                {
                    index++;
                    currentGUI = GUIs[index];
                }
                else
                {
                    startGame(); 
                }                
            }
            if ((currentGamePadState.IsButtonUp(Buttons.A) && (isKeyPressed)))
                isKeyPressed = false;

            if(player.health <= 0f)
            {
                currentGUI = failScreen;
                healthBar.setOffset(-100, 13);
                healthBarBorders.setOffset(-100, 10);
            }

            if (player.isColBall(ball))
            {
                currentGUI = winScreen;
                healthBar.setOffset(-100, 13);
                healthBarBorders.setOffset(-100, 10);
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Create a Light Mask to pass to the pixel shader
            GraphicsDevice.SetRenderTarget(lightsTarget);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, null, null);

            // This is our light.
            //spriteBatch.Draw(lightMask, new Vector2(player.Position.X - ((lightMask.Bounds.Width) * scale), player.Position.Y - ((lightMask.Bounds.Height) * scale)), null, Color.White, 0, new Vector2((lightMask.Bounds.Width/2)*scale, (lightMask.Bounds.Height/2))*scale, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(lightMask, new Vector2(player.Position.X - ((lightMask.Bounds.Width / 2) * scale), player.Position.Y - ((lightMask.Bounds.Height / 2) * scale)), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(ballGlow, new Vector2(ball.position.X - ballGlow.Bounds.Width/5, ball.position.Y - ballGlow.Bounds.Height/5), null, Color.White, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);

            spriteBatch.End();


            // Our main scene.
            GraphicsDevice.SetRenderTarget(mainTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null,null,null,null);
            spriteBatch.Draw(mainBackground, rectBackground, Color.White);
            player.Draw(spriteBatch);
            
           /* for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Draw(spriteBatch);
            }*/

            ball.Draw(spriteBatch);
            wall.Draw(spriteBatch);

            /*for (int i = 0; i < enemies.Count; i++)
            { 
                enemies[i].Draw(spriteBatch);
            }*/

            spriteBatch.End();


            // Draw the main scene with a pixel
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            lightingEffect.Parameters["lightMask"].SetValue(lightsTarget);
            lightingEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
            spriteBatch.End();

            // UI
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);

            currentGUI.Draw(spriteBatch, camera.center);
            if (gameStarted == true)
                healthBar.GUIRectangle.Width = (int)player.health;
            healthBar.Draw(spriteBatch, camera.center);
            healthBarBorders.Draw(spriteBatch, camera.center);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void startGame()
        {
            currentGUI.origin = new Vector2(500f, 2000f);
            healthBar.setOffset(10, 13);
            healthBarBorders.setOffset(10, 10);
            gameStarted = true;
        }
    }
}