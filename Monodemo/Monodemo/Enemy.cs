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

            public float enemyMoveSpeed;
            public float maxEnemyTurnSpeed;
            public float enemyTurnSpeed;


            public Rectangle enemyRec;
            public Vector2 velocity = new Vector2();
            public Random ran = new Random();

            Vector2 enemyPosition;
            float orientation = 0;
            Vector2 enemyTextureCenter;


        


        public void Initialize(Texture2D texture, Vector2 position)

            {
                EnemyTexture = texture;

            // Set the position of the enemy

                Position = position;


                Active = true;

                Health = 10;

                Damage = 10;

                enemyMoveSpeed = 2f;

                enemyTurnSpeed = 0.10f;

                Value = 100;

                enemyRec = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

                enemyTextureCenter = new Vector2(texture.Width / 2, texture.Height / 2);

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


            public void DetectColPlayer(Player p)
            {
                if (enemyRec.Intersects(p.playerRec))
                {
                    DetectPlayerCol(p);
                    Debug.WriteLine("Enemy Rec Collision With Player");

                }
            }

            private void DetectPlayerCol(Player p)
            {
                Color[] sourceColors = new Color[EnemyTexture.Width * EnemyTexture.Height];
                EnemyTexture.GetData(sourceColors);
                Color[] targetColors = new Color[p.PlayerTexture.Width * p.PlayerTexture.Height];
                p.PlayerTexture.GetData(targetColors);

                int left = Math.Max(enemyRec.Left, p.playerRec.Left);
                int top = Math.Max(enemyRec.Top, p.playerRec.Top);
                int width = Math.Min(enemyRec.Right, p.playerRec.Right) - left;
                int height = Math.Min(enemyRec.Bottom, p.playerRec.Bottom) - top;

                Rectangle intersectingRec = new Rectangle(left, top, width, height);

             for (int x = intersectingRec.Left; x < intersectingRec.Right; x++)
             {
                 for (int y = intersectingRec.Top; y < intersectingRec.Bottom; y++)
                 {
                     Color sourceColor = sourceColors[(x - enemyRec.Left) + (y - enemyRec.Top) * Width];
                     Color targetColor = targetColors[(x - p.playerRec.Left) + (y - p.playerRec.Top) * p.Width];
                     if (sourceColor.A > 0 && targetColor.A > 0)
                     {
                         Debug.WriteLine("Enemy Pixel Collision With Player");
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
                //Position.X -= enemyMoveSpeed;
                
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
            //spriteBatch.Draw(EnemyTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
             spriteBatch.Draw(EnemyTexture, Position, null, Color.White, orientation, enemyTextureCenter, 1.0f, SpriteEffects.None, 0.0f);
        }
        }
    }