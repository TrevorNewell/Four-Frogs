using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monodemo
{
    class Block
    {
        public Texture2D blockTexture;
        public Vector2 position;
        public Vector2 center;
        public float rotation;
        public Rectangle blockRec;
        public int Width
        { get { return blockTexture.Width; } }
        public int Height
        { get { return blockTexture.Height; } }

        public void Initialize(Texture2D texture, Vector2 position)
        {
            rotation = 0;
            blockTexture = texture;
            this.position = position;
            center = new Vector2(Width / 2, Height / 2);
            blockRec = new Rectangle((int)position.X - Width/2, (int)position.Y - Height/2, Width, Height);          

        }

        public void Update()

        {
            blockRec.X = (int)position.X - Width/2;
            blockRec.Y = (int)position.Y - Height/2;
        }

        public void Draw(SpriteBatch spriteBatch)

        {
            Vector2 drawPosition = new Vector2(position.X + (float)(Width * 0.08), position.Y+(float)(Height * 0.01));
            spriteBatch.Draw(blockTexture, position, null, Color.White, rotation, center, 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(blockRec, new Vector2(blockRec.X, blockRec.Y), Color.Wheat);
        }
    }
}
