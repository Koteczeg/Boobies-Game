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
    public class FlyingEnemyAnimation
    {
        public List<Texture2D> textures;
        public FlyingEnemy flyingEnemy;
        public int current = 1;
        public TimeSpan delay = new TimeSpan(0, 0, 0, 0, 150);
        public TimeSpan time = new TimeSpan(0);

        public FlyingEnemyAnimation(FlyingEnemy flyingEnemy)
        {
            this.flyingEnemy = flyingEnemy;
            textures = new List<Texture2D>();
        }

        public void LoadContent(ContentManager content)
        {
            textures.Add(content.Load<Texture2D>("Enemies/FlyingEnemy/fly1"));
            textures.Add(content.Load<Texture2D>("Enemies/FlyingEnemy/fly2"));
            textures.Add(content.Load<Texture2D>("Enemies/FlyingEnemy/fly3"));
            textures.Add(content.Load<Texture2D>("Enemies/FlyingEnemy/fly4"));
            textures.Add(content.Load<Texture2D>("Enemies/FlyingEnemy/fly5"));
            textures.Add(content.Load<Texture2D>("Enemies/FlyingEnemy/fly6"));
        }

        public void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime;
            if (time > delay)
            {
                time -= delay;
                current++;
                if (current >= textures.Count)
                    current = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch, int shift)
        {
            Vector2 onWindowPositon = new Vector2(flyingEnemy.position.X - shift, flyingEnemy.position.Y);
            spriteBatch.Draw(textures[current], flyingEnemy.convertToRectangle(onWindowPositon, flyingEnemy.size), Color.White);
        }

    }
}
