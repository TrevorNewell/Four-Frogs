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
        public Texture2D AnimeTexture;
        public Vector2 Position;
        public Vector2 center;
        public bool Active;
        public bool isCol = false;
        public float speed;
        public Vector2 velocity;
        public float rotation;
        public int Width
        { get { return PlayerTexture.Width; } }
        public int Height
        { get { return PlayerTexture.Height; } }
        public Rectangle playerRec;
        public Rectangle animeRec;

        KeyboardState currentKBState;
        KeyboardState previousKBState;
        float timer = 0f;
        float interval = 5f;
        int currentFrame = 0;

        public void Initialize(Texture2D texture, Texture2D animeTexture, Vector2 position)
        {
            speed = 0;
            rotation = 0;
            PlayerTexture = texture;
            AnimeTexture = animeTexture;
            Position = position;
            Active = true;
            center = new Vector2(Width / 2, Height / 2);
            playerRec = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);    
        }

        public void Update(GameTime gameTime)

        {
            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)Math.Cos(rotation) * speed;
            playerRec.X = (int)Position.X - Width/2;
            playerRec.Y = (int)Position.Y - Height/2;
            UpdateAnime(gameTime);        
        }

        private void UpdateAnime(GameTime gameTime)
        {
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();
            animeRec = new Rectangle(currentFrame * 32, 0, 32, 32);

        }

        public void Draw(SpriteBatch spriteBatch)

        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, rotation, center, 1f, SpriteEffects.None, 0f);
        }   
        
        public void TurnLeft()
        {
            rotation -= 0.1f;
        }    

        public void TurnRight()
        {
            rotation += 0.1f;
        }

        public void GoStraight()
        {
            speed = 2f;
            Position.X += velocity.X;
            Position.Y -= velocity.Y;
        }

        public void GoBack()
        {
            speed = 2f;
            Position.X -= velocity.X;
            Position.Y += velocity.Y;
        }

        public void DetectCol(Block block)
        {            
            if (playerRec.Intersects(block.blockRec))
            {
                this.isCol = DetectPixelCol(block);
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
                    Color sourceColor = sourceColors[(x - playerRec.Left) + (y - playerRec.Top) * (Width-1)];
                    Color targetColor = targetColors[(x - block.blockRec.Left) + (y - block.blockRec.Top) * (block.Width-1)];
                    if(sourceColor.A > 0 && targetColor.A > 0)
                    {
                        //Debug.WriteLine("Pixel Collision");
                        Position.X -= velocity.X;
                        Position.Y += velocity.Y;
                        isCol = true;
                        break;
                    }
                }
                break;
            }
            return isCol;
        }
    }
}
