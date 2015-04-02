using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BoobiesGame3
{
    public abstract class Platform : GameObject
    {
        public Platform(int x = 0, int y = 0, int width = 0, int height = 0)
            : base(x, y, width, height) { }

        public Platform() { ;}

        public override void Draw(SpriteBatch spriteBatch, int shift)
        {
            Vector2 onWindowPositon = new Vector2(position.X - shift, position.Y);
            Rectangle source = convertToRectangle(Vector2.Zero, this.size);
            spriteBatch.Draw(texture, onWindowPositon, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None,0);
        }

        public virtual bool BlocksToMoveRight(GameObject obj)
        {
            Rectangle thisObjectRectangle = obj.convertToRectangle();
            Rectangle platformRectangle = this.convertToRectangle();
            if (Collision.Intersect(thisObjectRectangle, platformRectangle))
            {
                Collision.FixOvelap(obj, this);
                if (Math.Abs(platformRectangle.Left - obj.position.X - obj.size.X) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool BlocksToMoveLeft(GameObject obj)
        {
            Rectangle thisObjectRectangle = obj.convertToRectangle();
            Rectangle platformRectangle = this.convertToRectangle();
            if (Collision.Intersect(thisObjectRectangle, platformRectangle))
            {
                Collision.FixOvelap(obj, this);
                if (Math.Abs(platformRectangle.Right - obj.position.X) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool BlocksToMoveUp(GameObject obj)
        {
            Rectangle thisObjectRectangle = obj.convertToRectangle();
            Rectangle platformRectangle = this.convertToRectangle();
            if (Collision.Intersect(thisObjectRectangle, platformRectangle))
            {
                Collision.FixOvelap(obj, this);
                if (Math.Abs(obj.position.Y - platformRectangle.Bottom) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool BlocksToMoveDown(GameObject obj)
        {
            Rectangle thisObjectRectangle = obj.convertToRectangle();
            Rectangle platformRectangle = this.convertToRectangle();
            if (Collision.Intersect(thisObjectRectangle, platformRectangle))
            {
                Collision.FixOvelap(obj, this);
                if (Math.Abs(obj.position.Y + obj.size.Y - platformRectangle.Top) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool ChangeVelocity(ref Vector2 velocity)
            // changes volocity and returns true if it was changed 
        {
            return false;
        }

        public virtual void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(texture_path);
        }
        
        public virtual bool ShouldBeRemoved(Platforms platforms, Unit Unit, GameWindow gameWindow, GameTime gameTime, int shift)
        {
            return false;
        }

        public virtual void SetStats(Platform statsPlatform)
        {
            this.texture_path = statsPlatform.texture_path;
        }

        public virtual void MoveUnit(Unit unit, GameWindow gameWindow, Platforms platforms, int shift) { }
    }
}
