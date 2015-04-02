using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoobiesGame3
{
    public class CrustyPlatformAnimation : IAnimation
    {
        public List<Texture2D> textures;
        public CrustyPlatform platform;
        public Texture2D currentTexture;
        public const int textureCount = 13;

        public CrustyPlatformAnimation(CrustyPlatform platform)
        {
            this.platform = platform;
            textures = new List<Texture2D>();
        }

        public void LoadContent(ContentManager content)
        {
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/1"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/2"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/3"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/4"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/5"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/6"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/7"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/8"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/9"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/10"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/11"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/12"));
            textures.Add(content.Load<Texture2D>("Platforms/CrustyPlatform/13"));
            currentTexture = textures[0];
        }

        public void Update(GameTime gameTime)
        {
            if (platform.timeSinceActivation == TimeSpan.Zero)
                currentTexture = textures[0];
            else
            {
                int index = (int)platform.timeSinceActivation.TotalMilliseconds * textureCount / (int)platform.maxTime.TotalMilliseconds;
                currentTexture = textures[index];
            }
        }

        public void Draw(SpriteBatch spriteBatch, int shift)
        {
            Vector2 onWindowPositon = new Vector2(platform.position.X - shift, platform.position.Y);
            Rectangle source = platform.convertToRectangle(Vector2.Zero, platform.size);
            spriteBatch.Draw(currentTexture, onWindowPositon, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }

        public void LoadContentForAnimation(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public void UpdateAnimation(GameTime gameTime, ContentManager content)
        {
            throw new NotImplementedException();
        }

        public void DrawAnimation(SpriteBatch spriteBatch, int shift)
        {
            throw new NotImplementedException();
        }
    }
}
