using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monodemo
{
    class Player
    {
        //SpriteEffect playerEffect;

        // Animation representing the player
        public Texture2D PlayerTexture;
        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;
        // Rotation
        public float Rotation;
        public Vector2 Center;
        public Rectangle playerRec;
        public float speed;
        public float speedX;
        public float speedY;
        // State of the player

        public bool Active;
        // Amount of hit points that player has
        public int Health;
        // Get the width of the player ship
        public int Width
        { get { return PlayerTexture.Width; } }
        // Get the height of the player ship
        public int Height
        { get { return PlayerTexture.Height; } }

        public void Initialize(Texture2D texture, Vector2 position)

        {
            PlayerTexture = texture;
            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;
            Rotation = 1.57f;

            // Set the player to be active
            Active = true;
            // Set the player health
            Health = 100;
            speed = 4f;
            playerRec = new Rectangle((int)Position.X, (int)Position.Y, PlayerTexture.Width, PlayerTexture.Height);
            Center = new Vector2(playerRec.Width/2, playerRec.Height/2);

        }

        public void Update(GameTime gameTime)
        {
            speedX = (float)Math.Sin(Rotation) * speed;
            speedY = (float)Math.Cos(Rotation) * speed;
        }

        public void Draw(SpriteBatch spriteBatch)

        {
            spriteBatch.Draw(PlayerTexture, Position, null, Color.White, Rotation, Center, 1f, SpriteEffects.None, 0f);
        }

       
    }
}
