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
    interface IAnimation
    {
        void LoadContentForAnimation(ContentManager content);
        void UpdateAnimation(GameTime gameTime, ContentManager content);
        void DrawAnimation(SpriteBatch spriteBatch, int shift);
    }
}