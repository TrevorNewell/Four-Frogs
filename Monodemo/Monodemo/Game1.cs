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

        List<GUI> GUIs;
        GUI currentGUI;
        const int NUM_OF_GUIS = 5;
        int index = 0;
        bool isKeyPressed = false;

        GUI healthBar;
        Rectangle healthBarRec;
        GUI healthBarBorders;

        Camera camera;

        float timer;
        int tcounter = 50;
        SpriteFont timeFont;
        Vector2 textPos;

        public Song gameMusic;
        public SoundEffect meetEnemy;
        public SoundEffectInstance meetEnemyInstance;
        public SoundEffect goldenBall;
        public SoundEffectInstance goldenBallInstance;
        public SoundEffect collisionSound;
        public SoundEffectInstance collisionSoundInstance;

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
            healthBar.setOffset(10, 13);
            healthBarBorders = new GUI(graphics); 
            healthBarBorders.setOffset(10, 10);       

            camera = new Camera(GraphicsDevice.Viewport);
            rectBackground = new Rectangle(0, 0, 1440, 900);

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

            timeFont = Content.Load<SpriteFont>("text\\Times New Roman");
            textPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
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
            }
            UpdatePlayer(gameTime);            
           
            //Update the enemies
            UpdateEnemies(gameTime);

            //zoom
            camera.Update(player.Position);
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                camera.zoom += 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                camera.zoom -= 0.1f;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            tcounter -= (int)timer;
            if (timer >= 1.0f) timer = 0f;

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
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, graphics.PreferredBackBufferWidth - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, graphics.PreferredBackBufferHeight - player.Height);
        }

        private void updateGUI(GameTime gameTime)
        {            
            if (currentKeyboardState.IsKeyDown(Keys.Space) && (!isKeyPressed))
            {
                isKeyPressed = true;
                if (index < 4)
                {
                    index++;
                    currentGUI = GUIs[index];
                }
                else
                {
                    currentGUI.origin = new Vector2(500f, 1000f);
                }                
            }
            if (currentKeyboardState.IsKeyUp(Keys.Space) && (isKeyPressed))
                isKeyPressed = false;
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

            currentGUI.Draw(spriteBatch, camera.center);
            healthBar.Draw(spriteBatch, camera.center);
            healthBarBorders.Draw(spriteBatch, camera.center);
            string time = "Timer: " + tcounter.ToString();
            //Vector2 origin = timeFont.MeasureString(time);
            //spriteBatch.DrawString(timeFont, time, textPos, Color.AliceBlue, 0, origin, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}