using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BoobiesGame3
{
    public class Background : GameObject
    {
        public Background() { }

        public override void Draw(SpriteBatch spriteBatch,int shift)
        {
            Vector2 onWindowPositon = new Vector2(position.X - shift, position.Y);
            spriteBatch.Draw(this.texture, Vector2.Zero, new Rectangle((int)position.X + shift, (int)position.Y, (int)size.X, (int)size.Y), Color.White);
        }

        public void Initialize(Vector2 resolution)
        {
            this.size = resolution;
            this.position = Vector2.Zero;
        }

        public void Update(GameTime gameTime, int shift)
        {

        }


        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(texture_path);
        }
    }
}
