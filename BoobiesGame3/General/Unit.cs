using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.ComponentModel;

namespace BoobiesGame3
{
    public class Unit : GameObject
    {
        public Vector2 velocity;

        [XmlAttribute(AttributeName = "velocity")]
        [EditorBrowsable(EditorBrowsableState.Never)] // IDE nie będzie podpowiadać, że takie pole istnieje
        public string VelocityAttribute
        {
            get { return this.velocity.X + "," + this.velocity.Y; }
            set
            {
                var parts = value.Split(',');
                this.velocity = new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
            }
        }

        public Unit(int x = 0, int y = 0, int width = 0, int height = 0)
            : base(x, y, width, height) { }

        public bool canMoveRight(GameWindow gameWindow, Platforms platforms, int shift)
        {
            foreach (var platform in platforms.platforms)
            {
                if (platform.isOnScreen(gameWindow, shift) && platform.BlocksToMoveRight(this))
                    return false;
            }
            return true;
        }

        public bool canMoveLeft(GameWindow gameWindow, Platforms platforms, int shift)
        {
            Rectangle thisObjectRectangle = convertToRectangle(this.position, this.size);
            foreach (var platform in platforms.platforms)
            {
                if (platform.isOnScreen(gameWindow, shift) && platform.BlocksToMoveLeft(this))
                    return false;
            }
            return this is Enemy || this.position.X - shift > 0;
        }

        public bool canMoveUp(GameWindow gameWindow, Platforms platforms, int shift)
        {
            Rectangle thisObjectRectangle = convertToRectangle(this.position, this.size);
            foreach (var platform in platforms.platforms)
            {
                if (platform.isOnScreen(gameWindow, shift) && platform.BlocksToMoveUp(this))
                    return false;
            }
            return true;
        }

        public bool canMoveDown(GameWindow gameWindow, Platforms platforms, int shift)
        {
            Rectangle thisObjectRectangle = convertToRectangle(this.position, this.size);
            foreach (var platform in platforms.platforms)
            {
                if (platform.isOnScreen(gameWindow, shift) && platform.BlocksToMoveDown(this))
                    return false;
            }
            return true;// thisObjectRectangle.Y < gameWindow.ClientBounds.Height - thisObjectRectangle.Height;
        }

    }
}
