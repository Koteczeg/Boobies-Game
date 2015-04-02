using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BoobiesGame3
{
    public class StaticEnemyAnimation
    {
        public List<Texture2D> textures;
        public StaticEnemy staticEnemy;
        public int current = 0;
        public TimeSpan delay = new TimeSpan(0, 0, 0, 0, 150);
        public TimeSpan time = new TimeSpan(0);
        private bool activeShot = false;

        public StaticEnemyAnimation(StaticEnemy staticEnemy)
        {
            this.staticEnemy = staticEnemy;
            textures = new List<Texture2D>();
        }

        public void LoadContent(ContentManager content)
        {
            textures.Add(content.Load<Texture2D>("Enemies/StaticEnemy/1"));
            textures.Add(content.Load<Texture2D>("Enemies/StaticEnemy/2"));
            textures.Add(content.Load<Texture2D>("Enemies/StaticEnemy/3"));
            textures.Add(content.Load<Texture2D>("Enemies/StaticEnemy/4"));
            textures.Add(content.Load<Texture2D>("Enemies/StaticEnemy/5"));
            textures.Add(content.Load<Texture2D>("Enemies/StaticEnemy/6"));
            textures.Add(content.Load<Texture2D>("Enemies/StaticEnemy/7"));
        }

        public void Update(GameTime gameTime)
        {
            if (staticEnemy.JustShot)
            {
                current = 0;
                activeShot = true;
            }

            if (activeShot)
            {
                time += gameTime.ElapsedGameTime;
                if (time > delay)
                {
                    time -= delay;
                    current++;
                    if (current >= textures.Count)
                    {
                        activeShot = false;
                        current = textures.Count - 1;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int shift)
        {
            Vector2 onWindowPositon = new Vector2(staticEnemy.position.X - shift, staticEnemy.position.Y);
            spriteBatch.Draw(textures[current], staticEnemy.convertToRectangle(onWindowPositon, staticEnemy.size), null, Color.White, staticEnemy.rotation, Vector2.Zero, SpriteEffects.None, 0);
        }

    }
}
