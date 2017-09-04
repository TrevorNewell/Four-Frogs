using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monodemo
{
    class Camera
    {
        //use matrix to transform, rotate and scale
        public Matrix transform;
        public Vector2 center;
        public Viewport viewport;
        public float zoom = 3;
        private float rotation = 0;

        public Camera(Viewport newViewport)
        {
            viewport = newViewport;
        }

        public void Update(Vector2 position)
        {
            center = new Vector2(position.X, position.Y);
            //restrict camera moving range, make sure it won't move out of border
            if (center.X - viewport.Width / 6 < 0)//zoom = 3
                center.X = viewport.Width / 6;
            else if (center.X + viewport.Width / 6 > 1440)
                center.X = 1440 - viewport.Width / 6;
            if (center.Y - viewport.Height / 6 < 0)
                center.Y = viewport.Height / 6;
            else if (center.Y + viewport.Height / 6 > 900)
                center.Y = 900 - viewport.Height / 6;

            transform = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0)) *
                Matrix.CreateRotationZ(rotation) *
                Matrix.CreateScale(new Vector3(zoom, zoom, 0)) *
                Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));
        }
    }
}
