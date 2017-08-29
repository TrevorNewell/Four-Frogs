using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monodemo
{
    class Player
    {
        public Texture2D PlayerTexture;
        public Vector2 Position;
        public Vector2 center;
        public bool Active;
        public float speed;
        public Vector2 velocity;
        public float rotation;
        public int Width
        { get { return PlayerTexture.Width; } }
        public int Height
        { get { return PlayerTexture.Height; } }
        public Rectangle playerRec;

        public void Initialize(Texture2D texture, Vector2 position)
        {
            speed = 0;
            rotation = 0;
            PlayerTexture = texture;
            Position = position;
            Active = true;
            center = new Vector2(Width / 2, Height / 2);
            playerRec = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);    
        }

        public void Update()

        {
            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)Math.Cos(rotation) * speed;
            playerRec.X = (int)Position.X - Width/2;
            playerRec.Y = (int)Position.Y - Height/2;           
        
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
            speed = 4f;
            Position.X += velocity.X;
            Position.Y -= velocity.Y;
        }

        public void GoBack()
        {
            speed = 4f;
            Position.X -= velocity.X;
            Position.Y += velocity.Y;
        }

        public void DetectCol(Block block)
        {
            if (playerRec.Intersects(block.blockRec))
            {
                DetectPixelCol(block);
                Debug.WriteLine("Rec Collision");
                
            }
        }

        private void DetectPixelCol(Block block)
        {
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
                    if(sourceColor.A > 0 && targetColor.A > 0)
                    {
                        Debug.WriteLine("Pixel Collision");
                        Position.X -= velocity.X;
                        Position.Y += velocity.Y;
                        break;
                    }
                }
                break;
            }
        }
    }
}
