using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monodemo
{
    class Player
    {
        public Texture2D PlayerTexture;
        public Vector2 Position;
        public Vector2 nextPoi;
        public Vector2 center;
        public bool Active;
        public bool isCol = false;
        public float maxSpeed;
        public float speed;
        public Vector2 velocity;
        public Vector2 maxVelocity;
        public float rotation;
        public int Width
        { get { return PlayerTexture.Width; } }
        public int Height
        { get { return PlayerTexture.Height; } }
        public Rectangle playerRec;

        public Texture2D AnimeTexture;
        public Rectangle animeRec;
        KeyboardState currentKBState;
        KeyboardState previousKBState;
        float timer = 0f;
        float interval = 5f;
        int currentFrame = 0;
        int end = 14;

        public float health;
        private const int maxHealth = 100;

        public void Initialize(Texture2D texture, Texture2D animeTexture, Vector2 position)
        {
            maxSpeed = 1;
            maxVelocity.X = (float)Math.Sin(rotation) * maxSpeed;
            maxVelocity.Y = (float)Math.Cos(rotation) * maxSpeed;
            speed = 1;
            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)Math.Cos(rotation) * speed;
            rotation = 0;
            PlayerTexture = texture;
            AnimeTexture = animeTexture;
            Position = position;
            nextPoi = position;
            Active = true;
            center = new Vector2(Width / 2, Height / 2);
            playerRec = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

            health = 195f;
              
        }

        public void Update(GameTime gameTime)

        {
            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)Math.Cos(rotation) * speed;
            maxVelocity.X = (float)Math.Sin(rotation) * maxSpeed;
            maxVelocity.Y = (float)Math.Cos(rotation) * maxSpeed;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                nextPoi.X = Position.X + velocity.X * 3f;
                nextPoi.Y = Position.Y - velocity.Y * 3f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                nextPoi.X = Position.X - velocity.X * 3f;
                nextPoi.Y = Position.Y + velocity.Y * 3f;
            }
            nextPoi.X = Position.X + maxVelocity.X * 10f;
            nextPoi.Y = Position.Y - maxVelocity.Y * 10f;

            playerRec.X = (int)nextPoi.X - Width/2;
            playerRec.Y = (int)nextPoi.Y - Height/2;
            UpdateAnime(gameTime);

            health -= 0.02f;    
        }

        private void UpdateAnime(GameTime gameTime)
        {
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();
            animeRec = new Rectangle(currentFrame * 32, 0, 32, 32);
            if (currentKBState.IsKeyDown(Keys.Up))
                AnimateHop(gameTime);
            if (currentKBState.IsKeyDown(Keys.Down))
                AnimateHopBackwards(gameTime);              
        }

        public void Draw(SpriteBatch spriteBatch)

        {
            spriteBatch.Draw(AnimeTexture, Position, animeRec, Color.White, rotation, center, 1f, SpriteEffects.None, 0f);
        }


        #region movement
        public void TurnLeft(GameTime gameTime)
        {
            rotation -= 0.05f;
        }    

        public void TurnRight(GameTime gameTime)
        {
            rotation += 0.05f;
        }

        public void GoStraight(GameTime gameTime)
        {
            Position.X += velocity.X;
            Position.Y -= velocity.Y;
            AnimateHop(gameTime);
        }

        public void GoBack(GameTime gameTime)
        {
            Position.X -= velocity.X;
            Position.Y += velocity.Y;
        }
        #endregion


        #region animation
        public void AnimateHop(GameTime gameTime)
        {
            // If we're just barely starting to move set our fram to the first image of our hop cycle
            if (currentKBState != previousKBState)
            {
                currentFrame = 0;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                // Advance our frame forward in our spritesheet
                currentFrame++;

                // If we've reached the end of our spritesheet, reset to the start of the spritesheet.
                if (currentFrame > end - 1)
                {
                    currentFrame = 0;
                }
                timer = 0f;
            }
        }

        public void AnimateHopBackwards(GameTime gameTime)
        {
            // If we're just barely starting to move set our fram to the last image of our hop cycle
            if (currentKBState != previousKBState)
            {
                currentFrame = end;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                // Advance our frame backwards in our spritesheet
                currentFrame--;

                // If we've reached the beginning of our spritesheet, reset to the end of the spritesheet.
                if (currentFrame < 0)
                {
                    currentFrame = end - 1;
                }
                timer = 0f;
            }
        }
        #endregion


        #region collision detection
        public bool isColBall(Block ball)
        {
            isCol = DetectPixelCol(ball);
            return isCol;
        }

        public void DetectCol(Block block)
        {            
            if (playerRec.Intersects(block.blockRec))
            {
                isCol = DetectPixelCol(block);
                //Debug.WriteLine("Rec Collision");                
            }
        }

        private bool DetectPixelCol(Block block)
        {
            bool isCol = false;
            Color[] sourceColors = new Color[PlayerTexture.Width * PlayerTexture.Height];
            PlayerTexture.GetData(sourceColors);
            Color[] targetColors = new Color[block.blockTexture.Width * block.blockTexture.Height];
            block.blockTexture.GetData(targetColors);

            int left = Math.Max(playerRec.Left, block.blockRec.Left);
            int top = Math.Max(playerRec.Top, block.blockRec.Top);
            int width = Math.Min(playerRec.Right, block.blockRec.Right) - left;
            int height = Math.Min(playerRec.Bottom, block.blockRec.Bottom) - top;

            Rectangle intersectingRec = new Rectangle(left, top, width, height);

            for(int x = intersectingRec.Left; x < intersectingRec.Right; x++)
            {
                for(int y = intersectingRec.Top; y < intersectingRec.Bottom; y++)
                {
                    Color sourceColor = sourceColors[(x - playerRec.Left) + (y - playerRec.Top) * Width];
                    Color targetColor = targetColors[(x - block.blockRec.Left) + (y - block.blockRec.Top) * block.Width];
                    if (sourceColor.A > 20 && targetColor.A > 50)
                    {
                        Debug.WriteLine("Pixel Collision");

                        isCol = true;
                        break;
                    }
                    else
                        isCol = false;
                }
                break;
            }
            return isCol;
        }
        #endregion
    }
}
