using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Monodemo
    {
        class Enemy
        {
            public Texture2D EnemyTexture;

            public Vector2 Position;

            public bool Active;

            public int Health;

            public int Damage;

            public int Value;

            public int Width
        {
            get { return EnemyTexture.Width; }
        }

            public int Height
        {
            get { return EnemyTexture.Height; }
        }

            float enemyMoveSpeed;

        public Rectangle enemyRec;
        public Vector2 velocity;

            public void Initialize(Texture2D texture, Vector2 position)

            {
                EnemyTexture = texture;

            // Set the position of the enemy

                Position = position;


                Active = true;

                Health = 10;

                Damage = 10;

                enemyMoveSpeed = 0f;

                Value = 100;

            }

            public void DetectCol(Block block)
        {
            if (enemyRec.Intersects(block.blockRec))
            {
                DetectPixelCol(block);
                Debug.WriteLine("Enemy Rec Collision");

            }
        }

        private void DetectPixelCol(Block block)
        {
            Color[] sourceColors = new Color[EnemyTexture.Width * EnemyTexture.Height];
            EnemyTexture.GetData(sourceColors);
            Color[] targetColors = new Color[block.blockTexture.Width * block.blockTexture.Height];
            block.blockTexture.GetData(targetColors);

            int left = Math.Max(enemyRec.Left, block.blockRec.Left);
            int top = Math.Max(enemyRec.Top, block.blockRec.Top);
            int width = Math.Min(enemyRec.Right, block.blockRec.Right) - left;
            int height = Math.Min(enemyRec.Bottom, block.blockRec.Bottom) - top;

            Rectangle intersectingRec = new Rectangle(left, top, width, height);

            for (int x = intersectingRec.Left; x < intersectingRec.Right; x++)
            {
                for (int y = intersectingRec.Top; y < intersectingRec.Bottom; y++)
                {
                    Color sourceColor = sourceColors[(x - enemyRec.Left) + (y - enemyRec.Top) * Width];
                    Color targetColor = targetColors[(x - block.blockRec.Left) + (y - block.blockRec.Top) * block.Width];
                    if (sourceColor.A > 0 && targetColor.A > 0)
                    {
                        Debug.WriteLine("Enemy Pixel Collision");
                        Position.X -= velocity.X;
                        Position.Y += velocity.Y;
                        break;
                    }
                }
                break;
            }
        }


            public void Update(GameTime gametime)

            {
                Position.X -= enemyMoveSpeed;
                
                //EnemyAnimation.Position = Position;

                //EnemyAnimation.Update(gameTime);

                if (Position.X < -Width || Health <= 0)

                {
                    Active = false;
                }


            }

            public void Draw(SpriteBatch spriteBatch)

            {
                //enemy.Draw(spriteBatch);
                spriteBatch.Draw(EnemyTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }