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