using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Monodemo
{
    class GUI
    {
        private Texture2D GUITexture;
        public Rectangle GUIRectangle;
        private string assetName;
        private bool isShow;
        public Vector2 position;
        public float rotation;
        public Vector2 origin;
        public float scale;
        public int width;
        public int height;
        public Vector2 offset;

        public GUI(GraphicsDeviceManager graphics)
        {
            isShow = true;
            assetName = string.Empty;
            width = graphics.PreferredBackBufferWidth;
            height = graphics.PreferredBackBufferHeight;
            position = new Vector2(0, 0);
            rotation = 0;
            origin = new Vector2(width / 2, height / 2);
            scale = 1f/3f;
            offset = new Vector2(0, 0);
        }

        public void setSize(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void setOffset(int X, int Y)
        {
            offset.X = X;
            offset.Y = Y;
        }

        public void Initialize()
        {

        }

        public void LoadContent(Texture2D texture)
        {

            GUITexture = texture;
            GUIRectangle = new Rectangle(0, 0, GUITexture.Width, GUITexture.Height);
        }

        public void Draw(SpriteBatch sprite, Vector2 cameraPosition)
        {
             sprite.Draw(GUITexture, cameraPosition + offset, GUIRectangle, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
        }
    }
}
