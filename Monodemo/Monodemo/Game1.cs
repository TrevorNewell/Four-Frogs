using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;



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

        private Song gameMusic;

        List<Block> blocks;
        DataTable blockTable;
        CSVUtil csv;

        List<Enemy> enemies;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";           
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
            csv = new CSVUtil();
            blocks = new List<Block>();
            blocks.Add(new Block());
            blockTable = csv.ReadCSV("blocksPoi.csv");
            Debug.Write(blockTable.Rows[0][0]);
            
            enemies = new List<Enemy>();
            enemies.Add(new Enemy());
            rectBackground = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
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
            Vector2 playerPosition = new Vector2(400, 400);
            player.Initialize(Content.Load<Texture2D>("Graphics\\player"), playerPosition);

            //mainBackground = Content.Load<Texture2D>("Graphics\\bg");

            blocks[0].Initialize(Content.Load<Texture2D>("Graphics\\block0"), new Vector2(200f, 200f));
            enemies[0].Initialize(Content.Load<Texture2D>("Graphics\\block0"), new Vector2(200f, 200f));

            gameMusic = Content.Load<Song>("Sounds\\bgm");
            MediaPlayer.Play(gameMusic);
            // TODO: use this.Content to load your game content here
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
            //spriteBatch.Draw(mainBackground, rectBackground, Color.White);
            player.Draw(spriteBatch);
            blocks[0].Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
