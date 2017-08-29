using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpriteSheetRunner
{
    public class AnimatedSprite
    {
        KeyboardState currentKBState;
        KeyboardState previousKBState;

        Texture2D spriteTexture;
  
        float timer = 0f;
        float interval = 5f;
        int currentFrame = 0;
        int spriteWidth = 32;
        int spriteHeight = 48;
        int spriteSpeed = 2;
        Rectangle sourceRect;
        Vector2 position;
        Vector2 origin;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Texture2D Texture
        {
            get { return spriteTexture; }
            set { spriteTexture = value; }
        }

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }

        public AnimatedSprite(Texture2D texture, int currentFrame, int spriteWidth, int spriteHeight)
        {
            this.spriteTexture = texture;
            this.currentFrame = currentFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
        }

        public void HandleSpriteMovement(GameTime gameTime)
        {
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);

            if (currentKBState.GetPressedKeys().Length == 0)
            {
                // Currently, frames 0 to 14 are the hop animations for our spritesheet.  If we aren't moving, stop our spritesheet from animating if it is, and display the first frame (which is the static image of the frog, motionless.)
                if (currentFrame > 0 && currentFrame < 14)
                {
                    currentFrame = 0;
                }

                /*
                if (currentFrame > 14 && currentFrame < ?)
                {
                    currentFrame = 14;
                }
                */
            }

            if (currentKBState.IsKeyDown(Keys.Down) == true)
            {
                AnimateHopBackwards(gameTime);

                // Translate Frog here as well.  Will be Different when we merge our projects
                if (position.Y < 575)
                {
                    position.Y += spriteSpeed;
                }
            }

            if (currentKBState.IsKeyDown(Keys.Up) == true)
            {
                AnimateHop(gameTime);
                // Translate Frog here as well.  Will be Different when we merge our projects
                if (position.Y > 25)
                {
                    position.Y -= spriteSpeed;
                }
            }

            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }

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
                if (currentFrame > 13)
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
                currentFrame = 14;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                // Advance our frame backwards in our spritesheet
                currentFrame--;

                // If we've reached the beginning of our spritesheet, reset to the end of the spritesheet.
                if (currentFrame < 0)
                {
                    currentFrame = 13;
                }
                timer = 0f;
            }
        }
    }
}
