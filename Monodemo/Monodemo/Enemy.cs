using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monodemo
{
    class Enemy
    {

        public Texture2D enemyTexture;
        public Vector2 Position;
        public Vector2 center;
        public bool Active;
        public float speed;
        public Vector2 velocity;
        public float rotation;
        public int Width
        { get { return enemyTexture.Width; } }
        public int Height
        { get { return enemyTexture.Height; } }
        public Rectangle enemyRec;

        public void Initialize(Texture2D texture, Vector2 position)
        {
            speed = 0;
            rotation = 0;
            enemyTexture = texture;
            Position = position;
            Active = true;
            center = new Vector2(Width / 2, Height / 2);
            enemyRec = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void Update()

        {
            velocity.X = (float)Math.Sin(rotation) * speed;
            velocity.Y = (float)Math.Cos(rotation) * speed;
            enemyRec.X = (int)Position.X - Width / 2;
            enemyRec.Y = (int)Position.Y - Height / 2;

        }

        public void Draw(SpriteBatch spriteBatch)

        {
            spriteBatch.Draw(enemyTexture, Position, null, Color.White, rotation, center, 1f, SpriteEffects.None, 0f);
        }
    }
}
